// <copyright file="ReadMediaPermission.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

#if ANDROID
using Android;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Views;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using BookCollector;
#endif
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;

namespace BookCollector.CustomPermissions
{
    public class ReadMediaPermission : Permissions.BasePermission
    {
#if ANDROID
        private const string ReadMediaImages = Manifest.Permission.ReadMediaImages;
        private const string UserSelectedImages = "android.permission.READ_MEDIA_VISUAL_USER_SELECTED";
#endif

        public override async Task<PermissionStatus> CheckStatusAsync()
        {
            this.EnsureDeclared();
#if ANDROID
            var api = (int)Build.VERSION.SdkInt;

            if (api >= 33)
            {
                var activity = Platform.CurrentActivity!;
                var context = activity.ApplicationContext;

                var status = ContextCompat.CheckSelfPermission(context, ReadMediaImages);

                if (status != Permission.Granted)
                {
                    status = ContextCompat.CheckSelfPermission(context, UserSelectedImages);
                }

                return status == Permission.Granted
                    ? PermissionStatus.Granted
                    : PermissionStatus.Denied;
            }
            else
            {
                // Android 12 and below: no permission needed or possible
                return PermissionStatus.Granted;
            }

#else
            throw new NotImplementedException();
#endif
        }

        public override void EnsureDeclared()
        {
#if ANDROID
            if (!Permissions.IsDeclaredInManifest(ReadMediaImages))
            {
                throw new PermissionException(
                    $"{ReadMediaImages} is not declared in AndroidManifest.xml");
            }

            if (!Permissions.IsDeclaredInManifest(UserSelectedImages))
            {
                throw new PermissionException(
                    $"{UserSelectedImages} is not declared in AndroidManifest.xml");
            }
#else
        throw new NotImplementedException();
#endif
        }

        public override async Task<PermissionStatus> RequestAsync()
        {
            if (this.ShouldShowRationale())
            {
                await BaseViewModel.DisplayMessage($"{AppStringResources.PleaseAllowPhotoPermissionToAutomaticallyUploadBookCoverPhotos}", null);
            }

#if ANDROID

            var api = (int)Build.VERSION.SdkInt;

            if (api >= 33)
            {
                while (Platform.CurrentActivity is not { IsFinishing: false, IsDestroyed: false } ||
               Platform.CurrentActivity?.Window?.DecorView?.WindowVisibility != ViewStates.Visible)
                {
                    await Task.Delay(50);
                }

                var activity = Platform.CurrentActivity!;
                var context = activity.ApplicationContext;

                if (ContextCompat.CheckSelfPermission(context, ReadMediaImages) == Permission.Granted ||
                    ContextCompat.CheckSelfPermission(context, UserSelectedImages) == Permission.Granted)
                {
                    return PermissionStatus.Granted;
                }

                var tcs = new TaskCompletionSource<PermissionStatus>();

                // Assign the delegate BEFORE requesting
                MainActivity.OnPermissionResult = (requestCode, permissions, grantResults) =>
                {
                    if (requestCode == 9911)
                    {
                        tcs.TrySetResult(PermissionStatus.Unknown);
                        MainActivity.OnPermissionResult = null;
                    }
                };

                ActivityCompat.RequestPermissions(
                    activity,
                    [ReadMediaImages, UserSelectedImages],
                    9911);

                var result = await tcs.Task;

                // Android may not have updated the permission yet
                await Task.Delay(100);

                if (ContextCompat.CheckSelfPermission(context, ReadMediaImages) == Permission.Granted ||
                    ContextCompat.CheckSelfPermission(context, UserSelectedImages) == Permission.Granted)
                {
                    return PermissionStatus.Granted;
                }

                return PermissionStatus.Denied;
            }
            else
            {
                // Android 12 and below: no permission needed or possible
                return PermissionStatus.Granted;
            }
#else
            throw new NotImplementedException();
#endif

        }

        public override bool ShouldShowRationale()
        {
#if ANDROID
            var api = (int)Build.VERSION.SdkInt;

            if (api >= 33)
            {
                var activity = Platform.CurrentActivity;

                var readMediaImagesRationale = ActivityCompat.ShouldShowRequestPermissionRationale(
                    activity!,
                    ReadMediaImages);

                var userSelectedImagesRationale = ActivityCompat.ShouldShowRequestPermissionRationale(
                    activity!,
                    ReadMediaImages);

                return readMediaImagesRationale || userSelectedImagesRationale;
            }
            else
            {
                return false;
            }
#else
            throw new NotImplementedException();
#endif
        }

        public static List<string> CheckForUserSelectedImages()
        {
            List<string> filePaths = new List<string>();

#if ANDROID
            var activity = Platform.CurrentActivity!;
            var context = activity.ApplicationContext;

            if (ContextCompat.CheckSelfPermission(context, ReadMediaImages) != Permission.Granted && 
                ContextCompat.CheckSelfPermission(context, UserSelectedImages) == Permission.Granted)
            {
                var collection = MediaStore.Images.Media.ExternalContentUri;

                var projection = new[]
                {
                    MediaStore.Images.Media.InterfaceConsts.Id,
                    MediaStore.Images.Media.InterfaceConsts.Data,
                };

                var cursor = context.ContentResolver!.Query(
                    collection,
                    projection,
                    null,
                    null,
                    null
                );

                while (cursor.MoveToNext())
                {
                    var id = cursor.GetLong(cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Id));
                    var path = cursor.GetString(cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data));
                    filePaths.Add(path);
                }
            }
#endif

            return filePaths;
        }
    }
}