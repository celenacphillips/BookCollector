// <copyright file="AndroidImagePicker.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

#if ANDROID
namespace BookCollector.CustomPicker
{
    using Android.App;
    using Android.Content;

    /// <summary>
    /// AndroidImagePicker class.
    /// </summary>
    public class AndroidImagePicker : IAndroidImagePicker
    {
        private TaskCompletionSource<Android.Net.Uri> tcsSingle;

        private TaskCompletionSource<List<Android.Net.Uri>> tcsMultiple;

        /// <summary>
        /// Creates a picker that allows the user to select an image from their device.
        /// </summary>
        /// <returns>The Uri of the selected image.</returns>
        public Task<Android.Net.Uri> PickImageAsync()
        {
            this.tcsSingle = new TaskCompletionSource<Android.Net.Uri>();

            var intent = new Intent(Intent.ActionPick);
            intent.SetType("image/*");

            var activity = Platform.CurrentActivity;
            activity?.StartActivityForResult(intent, 999);

            return this.tcsSingle.Task;
        }

        /// <summary>
        /// Creates a picker that allows the user to select multiple images from their device.
        /// </summary>
        /// <returns>A list of Uris for the selected images.</returns>
        public Task<List<Android.Net.Uri>> PickImagesAsync()
        {
            this.tcsMultiple = new TaskCompletionSource<List<Android.Net.Uri>>();

            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
            intent.AddCategory(Intent.CategoryOpenable);

            var chooser = Intent.CreateChooser(intent, "Select images");
            Platform.CurrentActivity?.StartActivityForResult(chooser, 998);

            return this.tcsMultiple.Task;
        }

        /// <summary>
        /// Called when StartActivityResult action is completed and processes the results.
        /// This method will determine if the Single or Multiple image picker was used,
        /// and process the results accordingly.
        /// </summary>
        /// <param name="requestCode">The value given when starting the activity.</param>
        /// <param name="resultCode">Indicate whether the activity was completed or canceled.</param>
        /// <param name="data">The data from the activity.</param>
        [Java.Interop.Export("OnActivityResult")]
        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 999 && resultCode == Result.Ok)
            {
                this.tcsSingle?.SetResult(data.Data!);
            }

            if (requestCode == 998 && resultCode == Result.Ok)
            {
                var uris = new List<Android.Net.Uri>();

                if (data.ClipData != null)
                {
                    for (int i = 0; i < data.ClipData.ItemCount; i++)
                    {
                        var item = data.ClipData.GetItemAt(i);
                        uris.Add(item?.Uri!);
                    }
                }
                else if (data.Data != null)
                {
                    uris.Add(data.Data);
                }

                this.tcsMultiple?.SetResult(uris);
            }
        }
    }
}
#endif