// <copyright file="GoogleBooksAPI.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using Newtonsoft.Json;

namespace BookCollector.Data.BookAPI
{
    /// <summary>
    /// Google Books API class.
    /// </summary>
    public class GoogleBooksAPI : BaseViewModel
    {
        public static async Task<(ObservableCollection<Item>?, int)> Search(string input)
        {
            var items = new ObservableCollection<Item>();
            var totalItemCount = 0;

            HttpClient client = new ()
            {
                BaseAddress = new Uri("https://www.googleapis.com/books/v1/volumes?q=isbn:"),
            };
            var endpoint = $"{client.BaseAddress}{input}";

            var response = client.GetAsync(endpoint).GetAwaiter().GetResult();

            ISBNLookup? isbnResponse = new ();

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;

                try
                {
                    if (result != null)
                    {
                        isbnResponse = JsonConvert.DeserializeObject<ISBNLookup>(result);

                        if (isbnResponse != null)
                        {
                            if (isbnResponse.items != null)
                            {
                                foreach (var item in isbnResponse.items)
                                {
                                    if (item.VolumeInfo.ImageLinks.thumbnail.StartsWith("http://"))
                                    {
                                        item.VolumeInfo.ImageLinks.thumbnail = item.VolumeInfo.ImageLinks.thumbnail.Replace("http://", "https://");
                                    }
                                }

                                items = [.. isbnResponse.items];
                            }

                            totalItemCount = isbnResponse.totalItems;

                            if (totalItemCount == 0)
                            {
                                return (items, totalItemCount);
                            }

                            if (items != null)
                            {
                                foreach (var item in items)
                                {
                                    if (item.VolumeInfo?.ImageLinks != null &&
                                        item.VolumeInfo.ImageLinks.thumbnail != null)
                                    {
                                        var image = $"{item.VolumeInfo.ImageLinks.thumbnail}.jpg";
                                        item.VolumeInfo.ImageLinks.ImageSource = new UriImageSource
                                        {
                                            Uri = new Uri(image),
                                            CachingEnabled = true,
                                            CacheValidity = TimeSpan.FromDays(14),
                                        };
                                        item.VolumeInfo.HasBookCover = true;
                                    }
                                    else
                                    {
                                        item.VolumeInfo?.HasBookCover = false;
                                    }

                                    item.VolumeInfo?.HasNoBookCover = !item.VolumeInfo.HasBookCover;

                                    if (item.VolumeInfo?.Authors != null && item.VolumeInfo.Authors.Count != 0)
                                    {
                                        item.VolumeInfo.AuthorList = string.Empty;

                                        for (int i = 0; i < item.VolumeInfo.Authors.Count; i++)
                                        {
                                            item.VolumeInfo.AuthorList += $"{item.VolumeInfo.Authors[i]}";

                                            if (i != item.VolumeInfo.Authors.Count - 1)
                                            {
                                                item.VolumeInfo.AuthorList += ", ";
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        return (items, totalItemCount);
                    }
                    else
                    {
                        return (items, totalItemCount);
                    }
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Cleartext HTTP traffic"))
                    {
                        await DisplayMessage(AppStringResources.ErrorParsingDataFromBook, null);
                    }

                    return (items, totalItemCount);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return (items, totalItemCount);
            }
        }
    }
}
