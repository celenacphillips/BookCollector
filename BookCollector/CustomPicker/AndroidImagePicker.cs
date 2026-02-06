#if ANDROID
using Android.App;
using Android.Content;
using Android.Provider;

namespace BookCollector.CustomPicker
{
    public class AndroidImagePicker : IAndroidImagePicker
    {
        TaskCompletionSource<Android.Net.Uri> _tcs_single;

        TaskCompletionSource<List<Android.Net.Uri>> _tcs_multiple;

        public Task<Android.Net.Uri> PickImageAsync()
        {
            _tcs_single = new TaskCompletionSource<Android.Net.Uri>();

            var intent = new Intent(Intent.ActionPick);
            intent.SetType("image/*");

            var activity = Platform.CurrentActivity;
            activity.StartActivityForResult(intent, 999);

            return _tcs_single.Task;
        }

        public Task<List<Android.Net.Uri>> PickImagesAsync()
        {
            _tcs_multiple = new TaskCompletionSource<List<Android.Net.Uri>>();

            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
            intent.AddCategory(Intent.CategoryOpenable);

            var chooser = Intent.CreateChooser(intent, "Select images");
            Platform.CurrentActivity.StartActivityForResult(chooser, 998);


            return _tcs_multiple.Task;

        }

        [Java.Interop.Export("OnActivityResult")]
        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            //if (requestCode == 999 && resultCode == Result.Ok)
            //{
            //    _tcs_single?.SetResult(data.Data);
            //}

            if (requestCode == 998 && resultCode == Result.Ok)
            {
                var uris = new List<Android.Net.Uri>();

                if (data.ClipData != null)
                {
                    for (int i = 0; i < data.ClipData.ItemCount; i++)
                    {
                        var item = data.ClipData.GetItemAt(i);
                        uris.Add(item.Uri);
                    }
                }
                else if (data.Data != null)
                {
                    uris.Add(data.Data);
                }

                _tcs_multiple?.SetResult(uris);
            }

        }

    }
}
#endif