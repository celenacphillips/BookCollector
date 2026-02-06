namespace BookCollector.Data
{
    public class ImageInfo
    {
        public string OriginalFileName { get; set; }

        public string MediaStoreId { get; set; }

        public string MediaStorePath { get; set; } // if available

        public string UriString { get; set; }
    }
}
