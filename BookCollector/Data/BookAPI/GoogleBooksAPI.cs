// <copyright file="GoogleBooksAPI.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BookCollector.Data.BookAPI
{
    /// <summary>
    /// Google Books API class.
    /// </summary>
    public class GoogleBooksAPI : BaseViewModel
    {
        private static IConfiguration configuration;

        public static void Initialize(IConfiguration config)
        {
            configuration = config;
        }

        public static async Task<(ObservableCollection<Item>?, int)> Search(string input)
        {
            var items = new ObservableCollection<Item>();
            var totalItemCount = 0;

            var baseURI = configuration.GetRequiredSection("Settings").GetRequiredSection("BaseURI").Value;
            var apiKey = configuration.GetRequiredSection("Settings").GetRequiredSection("APIKey").Value;

            HttpClient client = new ()
            {
                BaseAddress = new Uri(baseURI),
            };
            var endpoint = $"{client.BaseAddress}?key={apiKey}&q=isbn:{input}";

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
                                    if (item.VolumeInfo != null &&
                                        item.VolumeInfo.ImageLinks != null &&
                                        item.VolumeInfo.ImageLinks.thumbnail != null &&
                                        item.VolumeInfo.ImageLinks.thumbnail.StartsWith("http://"))
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
