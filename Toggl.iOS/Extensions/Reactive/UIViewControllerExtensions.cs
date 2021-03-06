using CoreGraphics;
using System;
using Toggl.Core.UI.Reactive;
using UIKit;

namespace Toggl.iOS.Extensions.Reactive
{
    public static class UIViewControllerExtensions
    {
        public static Action<CGSize> PreferredContentSize(this IReactive<UIViewController> reactive)
            => preferredContentSize => reactive.Base.PreferredContentSize = preferredContentSize;

        public static Action<bool> ModalInPresentation(this IReactive<UIViewController> reactive)
            => modalInPresentation => reactive.Base.ModalInPresentation = modalInPresentation;
    }
}
