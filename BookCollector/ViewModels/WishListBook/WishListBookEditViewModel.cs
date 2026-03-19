// <copyright file="WishListBookEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.WishListBook
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Main;
    using BookCollector.Views.WishListBook;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// WishListBookEditViewModel class.
    /// </summary>
    public partial class WishListBookEditViewModel : BookEditBaseViewModel
    {
        /// <summary>
        /// Gets or sets the book to edit.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public WishlistBookModel editedWishlistBook;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishListBookEditViewModel"/> class.
        /// </summary>
        /// <param name="book">Book to edit.</param>
        /// <param name="view">View related to view model.</param>
        public WishListBookEditViewModel(WishlistBookModel book, ContentPage view)
        {
            this.View = view;

            this.EditedWishlistBook = (WishlistBookModel)book.Clone();
            this.InfoText = $"{AppStringResources.BookEditView_InfoText.Replace("book", $"{this.EditedWishlistBook.BookTitle}")}";
            this.SelectedBookFormat = this.EditedWishlistBook.BookFormat ?? AppStringResources.SelectABookFormat;
            this.PopupWidth = DeviceWidth - 50;
            RefreshView = true;
        }

        /********************************************************/

        /// <summary>
        /// Add book to static list.
        /// </summary>
        /// <param name="book">Book to add.</param>
        /// <returns>A task.</returns>
        public static async Task AddToStaticList(WishlistBookModel book)
        {
            if (WishListViewModel.fullWishlistBookList != null)
            {
                WishListViewModel.RefreshView = AddWishListBookToStaticList(book, WishListViewModel.fullWishlistBookList, WishListViewModel.filteredWishlistBookList);
            }
        }

        /********************************************************/

        /// <summary>
        /// Add a new author to the author list.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddAuthor()
        {
            this.AuthorList ??= [];

            this.AuthorList.Add(new AuthorModel());
        }

        /// <summary>
        /// Remove author from the author list, and add it to the authors to delete list.
        /// </summary>
        /// <param name="author">Author to remove.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task RemoveAuthor(AuthorModel author)
        {
            this.AuthorList?.Remove(author);
        }

        /********************************************************/

        /// <summary>
        /// Validate data entry.
        /// </summary>
        public override void ValidateEntry()
        {
            this.BookTitleNotValid = string.IsNullOrEmpty(this.EditedWishlistBook.BookTitle);
            this.BookFormatNotValid = string.IsNullOrEmpty(this.EditedWishlistBook.BookFormat);
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        public override void GetPreferences()
        {
        }

        /// <summary>
        /// Set the view model lists.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetLists()
        {
        }

        /// <summary>
        /// Set section values.
        /// </summary>
        public override void SetSectionValues()
        {
            this.BookInfo1SectionValue = true;
            this.AuthorListSectionValue = true;
            this.BookInfoSectionValue = true;
            this.SummarySectionValue = true;
            this.CommentsSectionValue = true;
        }

        /// <summary>
        /// Get book data for other methods.
        /// </summary>
        /// <param name="returnData">Return type.</param>
        /// <returns>A list of strings of book data.</returns>
        public override object GetBookData(string? returnData)
        {
            if (returnData != null && returnData.Equals("strings"))
            {
                return (List<string?>)[this.EditedWishlistBook.BookCoverFileName, this.EditedWishlistBook.BookCoverUrl];
            }

            return this.EditedWishlistBook;
        }

        /// <summary>
        /// Check book format and set values.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task CheckBookFormat()
        {
            if (this.EditedWishlistBook.BookFormat == null || !this.EditedWishlistBook.BookFormat.Equals(AppStringResources.Audiobook))
            {
                this.ShowPages = true;
                this.ShowTime = false;
            }
            else
            {
                this.ShowTime = true;
                this.ShowPages = false;
            }
        }

        /// <summary>
        /// Set other view data.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetViewData()
        {
            var loadDataTasks = new Task[]
            {
                Task.Run(() => this.ValidateEntry()),
                Task.Run(() => this.EditedWishlistBook.SetBookPrice()),
                Task.Run(() => this.EditedWishlistBook.SetCoverDisplay()),
                Task.Run(() => this.BookInfo1Changed()),
                Task.Run(() => this.ReadingDataChanged()),
                Task.Run(() => this.ChapterListChanged()),
                Task.Run(() => this.AuthorListChanged()),
                Task.Run(() => this.BookInfoChanged()),
                Task.Run(() => this.SummaryChanged()),
                Task.Run(() => this.CommentsChanged()),
                Task.Run(() => this.EditedWishlistBook.TotalTimeSpan = WishlistBookModel.SetTime(this.EditedWishlistBook.BookHoursTotal, this.EditedWishlistBook.BookMinutesTotal)),
            };

            await Task.WhenAll(loadDataTasks);
        }

        /// <summary>
        /// Set author data.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetAuthorData()
        {
            var authors = ParseOutAuthorsFromstring(this.EditedWishlistBook.AuthorListString);

            await Task.WhenAll(authors);

            this.AuthorList = authors.Result;

            this.AuthorList ??= [];

            if (this.EditedWishlistBook.SelectedAuthors != null && this.EditedWishlistBook.SelectedAuthors.Count > 0)
            {
                foreach (var selectedAuthor in this.EditedWishlistBook.SelectedAuthors)
                {
                    if (this.AuthorList != null && selectedAuthor != null)
                    {
                        var author = new AuthorModel()
                        {
                            FirstName = selectedAuthor.FirstName,
                            LastName = selectedAuthor.LastName,
                        };

                        this.AuthorList.Add(author);
                    }
                }
            }
        }

        /// <summary>
        /// Set book format.
        /// </summary>
        /// <param name="format">Book format.</param>
        public override void SetBookFormat(string format)
        {
            this.EditedWishlistBook.BookFormat = format;
        }

        /// <summary>
        /// Set total time.
        /// </summary>
        /// <param name="time">Total time span.</param>
        /// <returns>A task.</returns>
        public override async Task SetTotalTime(TimeSpan time)
        {
            this.EditedWishlistBook.TotalTimeSpan = time;

            this.EditedWishlistBook.BookHoursTotal = SetHours(time);
            this.EditedWishlistBook.BookMinutesTotal = this.EditedWishlistBook.TotalTimeSpan.Minutes;
        }

        /// <summary>
        /// Set book cover.
        /// </summary>
        /// <param name="imageSource">Book cover image source.</param>
        /// <param name="fileName">Book cover image filename.</param>
        public override void SetBookCover(ImageSource? imageSource, string? fileName)
        {
            if (imageSource == null)
            {
                this.BookCover = null;
                this.EditedWishlistBook.BookCover = null;
                this.EditedWishlistBook.BookCoverUrl = null;
                this.EditedWishlistBook.BookCoverFileName = null;

                this.EditedWishlistBook.HasBookCover = false;
                this.EditedWishlistBook.HasNoBookCover = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    this.EditedWishlistBook.BookCoverFileName = fileName;
                }

                this.BookCover = imageSource;
                this.EditedWishlistBook.BookCover = imageSource;
                this.EditedWishlistBook.HasBookCover = true;
                this.EditedWishlistBook.HasNoBookCover = false;
            }
        }

        /// <summary>
        /// Set book data for saving.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetBookDataForSaving()
        {
            var dataTasks = new Task[]
            {
                Task.Run(() => this.EditedWishlistBook.SetCoverDisplay()),
                Task.Run(() => this.EditedWishlistBook.SetPartOfSeries()),
                Task.Run(() => this.EditedWishlistBook.SetBookPrice()),
                Task.Run(() => this.EditedWishlistBook.SetAuthorListString(this.AuthorList, false)),
            };

            await Task.WhenAll(dataTasks);
        }

        /// <summary>
        /// Save book data.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SaveData()
        {
            this.EditedWishlistBook = await Database.SaveWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(this.EditedWishlistBook));
            await AddToStaticList(this.EditedWishlistBook);
        }

        /// <summary>
        /// Set return view.
        /// </summary>
        /// <returns>Page to return to.</returns>
        public override ContentPage SetReturnView()
        {
            if (this.RemoveMainViewBefore)
            {
                Shell.Current.Navigation.RemovePage((WishListBookMainView)this.MainViewBefore!);
            }

            return new WishListBookMainView(this.EditedWishlistBook, $"{this.EditedWishlistBook.BookTitle}");
        }

        /********************************************************/

        private static bool AddWishListBookToStaticList(WishlistBookModel book, ObservableCollection<WishlistBookModel> bookList, ObservableCollection<WishlistBookModel>? filteredBookList)
        {
            var refresh = false;

            try
            {
                var oldBook = bookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                if (oldBook != null)
                {
                    var index = bookList.IndexOf(oldBook);
                    bookList.Remove(oldBook);
                    bookList.Insert(index, book);
                    refresh = true;
                }
                else
                {
                    bookList.Add(book);
                    refresh = true;
                }

                if (filteredBookList != null)
                {
                    var filteredBook = filteredBookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                    if (filteredBook != null)
                    {
                        var index = filteredBookList.IndexOf(filteredBook);
                        filteredBookList.Remove(filteredBook);
                        filteredBookList.Insert(index, book);
                        refresh = true;
                    }
                    else
                    {
                        filteredBookList.Add(book);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }
    }
}
