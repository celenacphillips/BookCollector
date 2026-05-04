// <copyright file="BookMainBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// BookMainBaseViewModel class.
    /// </summary>
    public abstract partial class BookMainBaseViewModel : BookBaseViewModel
    {
        /// <summary>
        /// Gets or sets the author list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<AuthorModel>? authorList;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to refresh the view or not.
        /// </summary>
        public static bool RefreshView { get; set; }

        /********************************************************/

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
        {
            if (!RefreshView)
            {
                return;
            }

            this.SetRefreshView(false);

            try
            {
                await this.SetIsBusyTrue();

                this.GetPreferences();

                await this.SetLists();

                this.SetSectionValues();

                await this.CheckBookFormat();

                List<string?> bookStrings = (List<string?>)this.GetBookData(Data.Enums.ReturnType.String) !;

                this.BookCover = await CheckBookCover(bookStrings[0], bookStrings[1]);

                await this.SetViewData();

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                await this.ViewModelCatch(ex);
            }
        }

        /********************************************************/

        /// <summary>
        /// Show edit book view with selected book.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task EditBook()
        {
            var book = this.GetBookData(Data.Enums.ReturnType.Book);

            if (book != null)
            {
                await this.SetIsBusyTrue();

                var view = this.SetEditView();

                await Shell.Current.Navigation.PushAsync(view);

                this.SetIsBusyFalse();
            }
        }

        /// <summary>
        /// Delete selected book.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task DeleteBook()
        {
            var book = this.GetBookData(Data.Enums.ReturnType.Book);
            var bookTitle = string.Empty;
            var bookUrl = string.Empty;

            if (book != null)
            {
                if (book is WishlistBookModel wishlistBookModel)
                {
                    bookTitle = wishlistBookModel.BookTitle;
                    bookUrl = wishlistBookModel.BookURL;
                }

                if (book is BookModel bookModel)
                {
                    bookTitle = bookModel.BookTitle;
                    bookUrl = bookModel.BookURL;
                }
            }

            if (book != null && !string.IsNullOrEmpty(bookTitle))
            {
                var answer = await this.DeleteCheck(bookTitle);

                if (answer)
                {
                    try
                    {
                        await this.SetIsBusyTrue();

                        await this.DeleteData();

                        await this.ConfirmDelete(bookTitle);

                        await Shell.Current.Navigation.PopAsync();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
                        await this.ViewModelCatch(ex);
                    }
                }
                else
                {
                    await this.CanceledAction();
                }
            }
        }

        /// <summary>
        /// Share book information.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ShareBook()
        {
            var book = this.GetBookData(Data.Enums.ReturnType.Book);
            var bookTitle = string.Empty;
            var bookUrl = string.Empty;
            var authorList = new List<AuthorModel>();

            if (book != null)
            {
                if (book is WishlistBookModel wishlistBookModel)
                {
                    bookTitle = wishlistBookModel.BookTitle;
                    bookUrl = wishlistBookModel.BookURL;
                    authorList = await StringManipulation.SplitAuthorListStringIntoAuthorList(wishlistBookModel.AuthorListString);
                }

                if (book is BookModel bookModel)
                {
                    bookTitle = bookModel.BookTitle;
                    bookUrl = bookModel.BookURL;
                    authorList = await StringManipulation.SplitAuthorListStringIntoAuthorList(bookModel.AuthorListString);
                }
            }

            if (book != null)
            {
                var title = bookTitle;

                string? text;
                if (authorList != null && authorList.Count > 0)
                {
                    text = $"{AppStringResources.BookTitleByAuthorName.Replace("Book Title", bookTitle).Replace("Author Name", authorList[0].FullName)}";

                    if (authorList.Count > 1)
                    {
                        text += $", {AppStringResources.EtAl}";
                    }
                }
                else
                {
                    text = $"{AppStringResources.BookTitle_Replace.Replace("Book Title", bookTitle)}";
                }

                if (!string.IsNullOrEmpty(bookUrl))
                {
                    text += $" ({bookUrl})";
                }

                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = text,
                    Title = title,
                });
            }
        }

        /********************************************************/

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        public abstract void GetPreferences();

        /// <summary>
        /// Set the view model lists.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetLists();

        /// <summary>
        /// Set section values.
        /// </summary>
        public abstract void SetSectionValues();

        /// <summary>
        /// Get book data for other methods.
        /// </summary>
        /// <param name="returnData">Return type.</param>
        /// <returns>An object of book data.</returns>
        public abstract object? GetBookData(Data.Enums.ReturnType returnData);

        /// <summary>
        /// Check book format and set values.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task CheckBookFormat();

        /// <summary>
        /// Set other view data.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetViewData();

        /// <summary>
        /// Set edit view.
        /// </summary>
        /// <returns>Page to navigate to.</returns>
        public abstract ContentPage SetEditView();

        /// <summary>
        /// Delete book data.
        /// </summary>
        /// <returns>An task.</returns>
        public abstract Task DeleteData();
    }
}
