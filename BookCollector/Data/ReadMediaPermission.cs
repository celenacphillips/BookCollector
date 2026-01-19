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
            new[] { ReadMediaImages, UserSelectedImages },
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

        var readMediaImagesRationale = ActivityCompat.ShouldShowRequestPermissionRationale(
            activity!,
            ReadMediaImages);

        var userSelectedImagesRationale = ActivityCompat.ShouldShowRequestPermissionRationale(
            activity!,
            ReadMediaImages);

        return readMediaImagesRationale || userSelectedImagesRationale;
#else
        throw new NotImplementedException();
#endif
    }
}