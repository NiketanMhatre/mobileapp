﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using Toggl.Core.Models;
using Toggl.Core.Services;
using Toggl.Networking;
using Toggl.Networking.Network;
using Toggl.Shared;
using Toggl.Shared.Extensions;
using Toggl.Shared.Models;
using Toggl.Storage;

namespace Toggl.Core.Login
{
    public sealed class UserAccessManager : IUserAccessManager
    {
        private readonly Lazy<IApiFactory> apiFactory;
        private readonly Lazy<ITogglDatabase> database;
        private readonly Lazy<IPrivateSharedStorageService> privateSharedStorageService;
        private readonly Lazy<ITimeService> timeService;

        private readonly ISubject<ITogglApi> userLoggedInSubject = new Subject<ITogglApi>();
        private readonly ISubject<Unit> userLoggedOutSubject = new Subject<Unit>();

        public IObservable<ITogglApi> UserLoggedIn => userLoggedInSubject.AsObservable();
        public IObservable<Unit> UserLoggedOut => userLoggedOutSubject.AsObservable();

        public UserAccessManager(
            Lazy<IApiFactory> apiFactory,
            Lazy<ITogglDatabase> database,
            Lazy<IPrivateSharedStorageService> privateSharedStorageService,
            Lazy<ITimeService> timeService)
        {
            Ensure.Argument.IsNotNull(database, nameof(database));
            Ensure.Argument.IsNotNull(apiFactory, nameof(apiFactory));
            Ensure.Argument.IsNotNull(privateSharedStorageService, nameof(privateSharedStorageService));
            Ensure.Argument.IsNotNull(timeService, nameof(timeService));

            this.database = database;
            this.apiFactory = apiFactory;
            this.privateSharedStorageService = privateSharedStorageService;
            this.timeService = timeService;
        }

        public IObservable<Unit> Login(Email email, Password password)
        {
            if (!email.IsValid)
                throw new ArgumentException($"A valid {nameof(email)} must be provided when trying to login");
            if (password.IsEmpty)
                throw new ArgumentException($"A non-empty {nameof(password)} must be provided when trying to login");

            return database.Value
                .Clear()
                .Select(_ => Credentials.WithPassword(email, password))
                .SelectMany(credentials => apiFactory.Value.CreateApiWith(credentials, timeService.Value).User.Get())
                .Select(User.Clean)
                .SelectMany(database.Value.User.Create)
                .Select(apiFromUser)
                .Do(userLoggedInSubject.OnNext)
                .SelectUnit();
        }

        public IObservable<Unit> LoginWithGoogle(string googleToken)
            => database.Value
                .Clear()
                .SelectMany(_ => loginWithGoogle(googleToken));

        public IObservable<Unit> SignUp(Email email, Password password, bool termsAccepted, int countryId, string timezone)
        {
            if (!email.IsValid)
                throw new ArgumentException($"A valid {nameof(email)} must be provided when trying to signup");
            if (!password.IsValid)
                throw new ArgumentException($"A valid {nameof(password)} must be provided when trying to signup");

            return database.Value
                .Clear()
                .SelectMany(_ => signUp(email, password, termsAccepted, countryId, timezone))
                .Select(User.Clean)
                .SelectMany(database.Value.User.Create)
                .Select(apiFromUser)
                .Do(userLoggedInSubject.OnNext)
                .SelectUnit();
        }

        public IObservable<Unit> SignUpWithGoogle(string googleToken, bool termsAccepted, int countryId, string timezone)
            => database.Value
                .Clear()
                .SelectMany(_ => signUpWithGoogle(googleToken, termsAccepted, countryId, timezone));

        public IObservable<string> ResetPassword(Email email)
        {
            if (!email.IsValid)
                throw new ArgumentException($"A valid {nameof(email)} must be provided when trying to reset forgotten password.");

            var api = apiFactory.Value.CreateApiWith(Credentials.None, timeService.Value);
            return api.User.ResetPassword(email).ToObservable();
        }

        public bool CheckIfLoggedIn()
        {
            if (privateSharedStorageService.Value.HasUserDataStored())
                return true;

            return
                database.Value
                .User.Single()
                .Do(storeApiInfoOnPrivateStorage)
                .SelectValue(true)
                .Catch(Observable.Return(false))
                .Wait();
        }

        public void LoginWithSavedCredentials()
        {
            if (privateSharedStorageService.Value.HasUserDataStored())
            {
                userLoggedInSubject.OnNext(apiFromSharedStorage());
                return;
            }

            database.Value
                .User.Single()
                .Do(user => userLoggedInSubject.OnNext(apiFromUser(user)))
                .Catch((Exception ex) => Observable.Empty<User>())
                .ToArray()
                .Wait();
        }

        public string GetSavedApiToken()
            => privateSharedStorageService.Value.GetApiToken();

        private ITogglApi apiFromSharedStorage()
        {
            var apiToken = privateSharedStorageService.Value.GetApiToken();
            var newCredentials = Credentials.WithApiToken(apiToken);
            var api = apiFactory.Value.CreateApiWith(newCredentials, timeService.Value);
            return api;
        }

        public IObservable<Unit> RefreshToken(Password password)
        {
            if (password.IsEmpty)
                throw new ArgumentException($"A non empty {nameof(password)} must be provided when trying to refresh token");

            return database.Value.User
                .Single()
                .Select(user => user.Email)
                .Select(email => Credentials.WithPassword(email, password))
                .Select(credentials => apiFactory.Value.CreateApiWith(credentials, timeService.Value))
                .SelectMany(api => api.User.Get())
                .Select(User.Clean)
                .SelectMany(database.Value.User.Update)
                .Select(apiFromUser)
                .Do(userLoggedInSubject.OnNext)
                .SelectUnit();
        }

        public void OnUserLoggedOut()
        {
            userLoggedOutSubject.OnNext(Unit.Default);
        }

        private ITogglApi apiFromUser(IUser user)
        {
            storeApiInfoOnPrivateStorage(user);
            var newCredentials = Credentials.WithApiToken(user.ApiToken);
            var api = apiFactory.Value.CreateApiWith(newCredentials, timeService.Value);
            return api;
        }

        private void storeApiInfoOnPrivateStorage(IUser user)
        {
            privateSharedStorageService.Value.SaveApiToken(user.ApiToken);
            privateSharedStorageService.Value.SaveUserId(user.Id);
        }

        private IObservable<Unit> loginWithGoogle(string googleToken)
        {
            var credentials = Credentials.WithGoogleToken(googleToken);

            return Observable
                .Return(apiFactory.Value.CreateApiWith(credentials, timeService.Value))
                .SelectMany(api => api.User.GetWithGoogle())
                .Select(User.Clean)
                .SelectMany(database.Value.User.Create)
                .Select(apiFromUser)
                .Do(userLoggedInSubject.OnNext)
                .SelectUnit();
        }

        private IObservable<IUser> signUp(Email email, Password password, bool termsAccepted, int countryId, string timezone)
        {
            return apiFactory.Value
                .CreateApiWith(Credentials.None, timeService.Value)
                .User
                .SignUp(email, password, termsAccepted, countryId, timezone)
                .ToObservable();
        }

        private IObservable<Unit> signUpWithGoogle(string googleToken, bool termsAccepted, int countryId, string timezone)
        {
            var api = apiFactory.Value.CreateApiWith(Credentials.None, timeService.Value);
            return api.User
                .SignUpWithGoogle(googleToken, termsAccepted, countryId, timezone)
                .ToObservable()
                .Select(User.Clean)
                .SelectMany(database.Value.User.Create)
                .Select(apiFromUser)
                .Do(userLoggedInSubject.OnNext)
                .SelectUnit();
        }
    }
}
