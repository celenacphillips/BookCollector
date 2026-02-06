namespace BookCollector.CustomPicker
{
    public static class ServiceHelper
    {
        public static T GetService<T>() =>
            Current.GetService<T>();

        public static IServiceProvider Current =>
            IPlatformApplication.Current?.Services
            ?? throw new InvalidOperationException("Unable to find MAUI service provider.");
    }

}
