// <copyright file="BookEditBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// BookEditBaseViewModel class.
    /// </summary>
    public abstract partial class BookEditBaseViewModel : BookBaseViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether the book title is valid or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookTitleNotValid;

        /// <summary>
        /// Gets or sets a value indicating whether the book format is valid or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookFormatNotValid;

        /// <summary>
        /// Gets or sets a value indicating whether the first section in book info is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfo1SectionValue;

        /// <summary>
        /// Gets or sets a value indicating whether the first section in book info is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfo1Open;

        /// <summary>
        /// Gets or sets a value indicating whether the first section in book info is not open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfo1NotOpen;

        /// <summary>
        /// Gets or sets the author list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<AuthorModel>? authorList;

        /// <summary>
        /// Gets or sets the selected book format.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string selectedBookFormat;

        /// <summary>
        /// Gets or sets a value indicating whether to remove the main view before.
        /// </summary>
        public bool RemoveMainViewBefore { get; set; }

        /// <summary>
        /// Gets or sets the main view before, to return to after closing the popup.
        /// </summary>
        public object? MainViewBefore { get; set; }

        /// <summary>
        /// Show popup to replace book cover photo.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task<string?> PopupMenu_CoverPhoto()
        {
            var title = AppStringResources.AddOrReplaceCoverPhoto;
            var file = AppStringResources.UploadExistingFile;
            var url = AppStringResources.BookCoverUrl;

            var answer = await this.View.ShowPopupAsync<string>(new ChoiceDialogPopup(DeviceWidth - 50, title, string.Empty, file, url, "Options"));

            return answer.Result;
        }

        /// <summary>
        /// Sets the expander arrow boolean values on change.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookInfo1Changed()
        {
            this.BookInfo1Open = this.BookInfo1SectionValue;
            this.BookInfo1NotOpen = !this.BookInfo1SectionValue;
        }

        /// <summary>
        /// Validate book format.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ValidateBookFormat()
        {
            this.ValidateEntry();
        }

        /// <summary>
        /// Validate book title.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ValidateBookTitle()
        {
            this.ValidateEntry();
        }

        /// <summary>
        /// Set the view model list.
        /// </summary>
        /// <param name="showHidden">The show hidden list preference.</param>
        /// <returns>A task.</returns>
        public async override Task SetList(bool showHidden)
        {
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
        {
        }

        /// <summary>
        /// Validate data entry.
        /// </summary>
        public abstract void ValidateEntry();
    }
}
