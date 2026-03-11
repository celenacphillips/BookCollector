// <copyright file="GoogleBooksAPI.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.BookAPI
{
    using System.Collections.ObjectModel;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    /// <summary>
    /// GoogleBooksAPI class.
    /// </summary>
    public class GoogleBooksAPI : BaseViewModel
    {
        private static IConfiguration configuration;

        /// <summary>
        /// Initializes the GoogleBooksAPI class with the given configuration.
        /// </summary>
        /// <param name="config">The configuration to initialize with.</param>
        public static void Initialize(IConfiguration config)
        {
            configuration = config;
        }

        /// <summary>
        /// Combines search term to endpoint query and searchs for the ISBN term in the Google Books API.
        /// </summary>
        /// <param name="input">The term searched.</param>
        /// <returns>The output of the search and the amount of items in the search.</returns>
        public static async Task<(ObservableCollection<Item>?, int)> SearchIsbn(string input)
        {
            var queryString = $"isbn:{input}";
            return await Search(queryString);
        }

        /// <summary>
        /// Combines search term to endpoint query and searchs for the Title term in the Google Books API.
        /// </summary>
        /// <param name="input">The term searched.</param>
        /// <returns>The output of the search and the amount of items in the search.</returns>
        public static async Task<(ObservableCollection<Item>?, int)> SearchTitle(string input)
        {
            var queryString = $"intitle:\"{input}\"";
            return await Search(queryString);
        }

        /// <summary>
        /// Combines search term to endpoint query and searchs for the Author Name term in the Google Books API.
        /// </summary>
        /// <param name="input">The term searched.</param>
        /// <returns>The output of the search and the amount of items in the search.</returns>
        public static async Task<(ObservableCollection<Item>?, int)> SearchAuthorName(string input)
        {
            var queryString = $"inauthor:\"{input}\"";
            return await Search(queryString);
        }

        /// <summary>
        /// Combines search terms to endpoint query and searchs for the terms in the Google Books API.
        /// </summary>
        /// <param name="isbn">The ISBN term searched.</param>
        /// <param name="title">The Title term searched.</param>
        /// <param name="name">The Author Name term searched.</param>
        /// <returns>The output of the search and the amount of items in the search.</returns>
        public static async Task<(ObservableCollection<Item>?, int)> CombinedSearch(string? isbn, string? title, string? name)
        {
            var queryString = string.Empty;

            if (!string.IsNullOrEmpty(isbn))
            {
                queryString += $"isbn:{isbn}";
            }

            if (!string.IsNullOrEmpty(title))
            {
                if (!string.IsNullOrEmpty(queryString))
                {
                    queryString += "+";
                }

                queryString += $"intitle:\"{title}\"";
            }

            if (!string.IsNullOrEmpty(name))
            {
                if (!string.IsNullOrEmpty(queryString))
                {
                    queryString += "+";
                }

                queryString += $"inauthor:\"{name}\"";
            }

            return await Search(queryString);
        }

        /// <summary>
        /// Searchs for the given query string in the Google Books API.
        /// </summary>
        /// <param name="queryString">The query string for the endpoint.</param>
        /// <returns>The output of the search and the amount of items in the search.</returns>
        public static async Task<(ObservableCollection<Item>?, int)> Search(string queryString)
        {
            var items = new ObservableCollection<Item>();
            var totalItemCount = 0;

            HttpClient client = new ();

            var baseURI = configuration.GetRequiredSection("Settings").GetRequiredSection("BaseURI").Value;
            var apiKey = configuration.GetRequiredSection("Settings").GetRequiredSection("APIKey").Value;

            var endpoint = $"{baseURI}?key={apiKey}&q={queryString}&maxResults=40";

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

                                totalItemCount = isbnResponse.items.Count;
                            }

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
                    if (ex.InnerException!.InnerException!.Message.Contains("Cleartext HTTP traffic"))
                    {
                        // await DisplayMessage(AppStringResources.ErrorParsingDataFromBook, null);
                    }

                    return (items, totalItemCount);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                return (items, totalItemCount);
            }
        }
    }
}
