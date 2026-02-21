// <copyright file="ReadMediaPermission.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

#if ANDROID
using Android;
using Android.Content.PM;
using Android.Views;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using BookCollector;
#endif
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;

namespace BookCollector.CustomPermissions
{
    public class InternetPermission : Permissions.BasePermission
    {
#if ANDROID
        private const string InternetPermissionName = Manifest.Permission.Internet;
#endif

        public override async Task<PermissionStatus> CheckStatusAsync()
        {
            this.EnsureDeclared();
#if ANDROID
            var activity = Platform.CurrentActivity!;
            var context = activity.ApplicationContext;

            var status = ContextCompat.CheckSelfPermission(context, InternetPermissionName);

            return status == Permission.Granted
                ? PermissionStatus.Granted
                : PermissionStatus.Denied;
#else
            throw new NotImplementedException();
#endif
        }

        public override void EnsureDeclared()
        {
#if ANDROID
            if (!Permissions.IsDeclaredInManifest(InternetPermissionName))
            {
                throw new PermissionException(
                    $"{InternetPermissionName} is not declared in AndroidManifest.xml");
            }
#else
            throw new NotImplementedException();
#endif
        }

        public override async Task<PermissionStatus> RequestAsync()
        {
            if (this.ShouldShowRationale())
            {
                //await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
            }

#if ANDROID
            while (Platform.CurrentActivity is not { IsFinishing: false, IsDestroyed: false } ||
               Platform.CurrentActivity?.Window?.DecorView?.WindowVisibility != ViewStates.Visible)
            {
                await Task.Delay(50);
            }

            var activity = Platform.CurrentActivity!;
            var context = activity.ApplicationContext;

            if (ContextCompat.CheckSelfPermission(context, InternetPermissionName) == Permission.Granted)
            {
                return PermissionStatus.Granted;
            }

            var tcs = new TaskCompletionSource<PermissionStatus>();

            // Assign the delegate BEFORE requesting
            MainActivity.OnPermissionResult = (requestCode, permissions, grantResults) =>
            {
                if (requestCode == 9911)
                {
                    var granted = grantResults.Length > 0 &&
                                  grantResults[0] == Permission.Granted;

                    tcs.TrySetResult(granted
                        ? PermissionStatus.Granted
                        : PermissionStatus.Denied);

                    // Clear the delegate to avoid memory leaks
                    MainActivity.OnPermissionResult = null;
                }
            };

            ActivityCompat.RequestPermissions(
                activity,
                [InternetPermissionName],
                9911);

            return await tcs.Task;
#else
            throw new NotImplementedException();
#endif

        }

        public override bool ShouldShowRationale()
        {
#if ANDROID
            var activity = Platform.CurrentActivity;

            var internetRationale = ActivityCompat.ShouldShowRequestPermissionRationale(
                activity!,
                InternetPermissionName);

            return internetRationale;
#else
            throw new NotImplementedException();
#endif
        }
    }
}