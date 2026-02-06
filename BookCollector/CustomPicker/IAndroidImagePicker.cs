#if ANDROID
namespace BookCollector.CustomPicker
{
    public interface IAndroidImagePicker
    {
        Task<Android.Net.Uri> PickImageAsync();

        Task<List<Android.Net.Uri>> PickImagesAsync();
    }
}
#endif