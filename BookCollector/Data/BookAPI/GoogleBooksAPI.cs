using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;

namespace BookCollector.Data.BookAPI
{
    public class GoogleBooksAPI
    {
        public static async Task<(ObservableCollection<Item>, int)> SearchAsync(string input)
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri("https://www.googleapis.com/books/v1/volumes?q=isbn:")
            };
            var endpoint = $"{client.BaseAddress}{input}";

            var response = client.GetAsync(endpoint).GetAwaiter().GetResult();

            ISBNLookup? isbnResponse = new();

            if (response.IsSuccessStatusCode)
            {
                var isbnItems = new ObservableCollection<Item>();
                var totalItems = 0;

                var result = response.Content.ReadAsStringAsync().Result;

                try
                {
                    if (result != null)
                    {
                        isbnResponse = JsonConvert.DeserializeObject<ISBNLookup>(result);

                        if (isbnResponse != null)
                        {
                            isbnItems = [.. isbnResponse.items];
                            totalItems = isbnResponse.totalItems;

                            if (totalItems == 0)
                                throw new Exception();

                            foreach (var item in isbnItems)
                            {
                                if (item.VolumeInfo.ImageLinks != null &&
                                    item.VolumeInfo.ImageLinks.thumbnail != null)
                                {
                                    item.VolumeInfo.ImageLinks.ImageURL = $"{item.VolumeInfo.ImageLinks.thumbnail}.jpg";
                                    var byteArray = new WebClient().DownloadData($"{item.VolumeInfo.ImageLinks.thumbnail}.jpg");
                                    item.VolumeInfo.ImageLinks.ImageSource = ImageSource.FromStream(() => new MemoryStream(byteArray));
                                    item.VolumeInfo.HasBookCover = true;
                                }
                                else
                                    item.VolumeInfo.HasBookCover = false;

                                item.VolumeInfo.HasNoBookCover = !item.VolumeInfo.HasBookCover;

                                if (item.VolumeInfo.Authors != null && item.VolumeInfo.Authors.Count != 0)
                                {
                                    item.VolumeInfo.AuthorList = string.Empty;

                                    for (int i = 0; i < item.VolumeInfo.Authors.Count; i++)
                                    {
                                        item.VolumeInfo.AuthorList += $"{item.VolumeInfo.Authors[i]}";

                                        if (i != item.VolumeInfo.Authors.Count - 1)
                                            item.VolumeInfo.AuthorList += ", ";
                                    }
                                }
                            }
                        }

                        return (isbnItems, totalItems);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    //await Shell.Current.DisplayAlert(null, AppResources.ErrorSearchingForBook, AppResources.OK);
                    return (null, 0);
                }
            }
            else
            {
                //await Shell.Current.DisplayAlert(response.ReasonPhrase, AppResources.ErrorSearchingForBook, AppResources.OK);
                return (null, 0);
            }
        }
    }
}
