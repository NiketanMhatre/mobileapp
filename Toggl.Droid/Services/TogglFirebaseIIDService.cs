using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.App;
using Firebase.Iid;
using Toggl.Core;
using Toggl.Networking;
using Toggl.Shared.Extensions;

namespace Toggl.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class TogglFirebaseIIDService : FirebaseInstanceIdService
    {
        private CompositeDisposable disposeBag = new CompositeDisposable();
        
        public override void OnTokenRefresh()
        {
            AndroidDependencyContainer.EnsureInitialized(Application.Context);

            var token = FirebaseInstanceId.Instance.Token;

            var dependencyContainer = AndroidDependencyContainer.Instance;
            dependencyContainer.InteractorFactory.StoreNewTokenInteractor(token).Execute();

            registerTokenIfNecessary(dependencyContainer);
        }

        private void registerTokenIfNecessary(AndroidDependencyContainer dependencyContainer)
        {
            var userLoggedIn = dependencyContainer.UserAccessManager.CheckIfLoggedIn();
            if (!userLoggedIn) return;
            
            var subscribeToPushNotificationsInteractor = dependencyContainer.InteractorFactory.SubscribeToPushNotifications();
            subscribeToPushNotificationsInteractor
                .Execute()
                .ObserveOn(dependencyContainer.SchedulerProvider.BackgroundScheduler)
                .Subscribe(_ => StopSelf())
                .DisposedBy(disposeBag);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;
            
            disposeBag?.Dispose();
        }
    }
}