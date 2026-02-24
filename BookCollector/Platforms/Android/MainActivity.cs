// <copyright file="MainActivity.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BookCollector
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using BookCollector.CustomPicker;

        /// <summary>
        /// Main Activity class.
        /// </summary>
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        /// <summary>
        /// The OnPermissionResult delegate is used to pass the results back to the requesting app code.
        /// Disabled the SA1401 warning, since the field is called outside the class
        /// and is not intended to be private.
        /// </summary>
        #pragma warning disable SA1401 // Fields should be private
        internal static Action<int, string[], Permission[]>? OnPermissionResult;
        #pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Called after user responds to a permission request. The results are passed to the
        /// OnPermissionResult delegate.
        /// </summary>
        /// <param name="requestCode">The value given when requesting the permission.</param>
        /// <param name="permissions">The list of permissions requested.</param>
        /// <param name="grantResults">The list of statuses for each permission (Granted or Denied).</param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            OnPermissionResult?.Invoke(requestCode, permissions, grantResults);
        }
    }
}
