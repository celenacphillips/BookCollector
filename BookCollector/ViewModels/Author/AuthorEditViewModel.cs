// <copyright file="AuthorEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Author
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.Views.Author;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// AuthorEditViewModel class.
    /// </summary>
    public partial class AuthorEditViewModel : AuthorsViewModel
    {
        /// <summary>
        /// Gets or sets the author to edit.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public AuthorModel editedAuthor;

        /// <summary>
        /// Gets or sets a value indicating whether the author first name is valid or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool authorFirstNameNotValid;

        /// <summary>
        /// Gets or sets a value indicating whether the author last name is valid or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool authorLastNameNotValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorEditViewModel"/> class.
        /// </summary>
        /// <param name="author">Author to edit.</param>
        /// <param name="view">View related to view model.</param>
        public AuthorEditViewModel(AuthorModel author, ContentPage view)
            : base(view)
        {
            this.View = view;

            this.EditedAuthor = (AuthorModel)author.Clone();
        }

        /// <summary>
        /// Gets or sets a value indicating whether to insert the main view before or not.
        /// </summary>
        public bool InsertMainViewBefore { get; set; }

        /// <summary>
        /// Add author to the static list in the list view model.
        /// </summary>
        /// <param name="author">Author to add.</param>
        /// <returns>A task.</returns>
        public static async Task AddToStaticList(AuthorModel author)
        {
            if (AuthorsViewModel.fullAuthorList != null)
            {
                AuthorsViewModel.RefreshView = await AddAuthorToStaticList(author, AuthorsViewModel.fullAuthorList, AuthorsViewModel.filteredAuthorList2);
            }
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                await this.ValidateFirstName();
                await this.ValidateLastName();

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                this.SetIsBusyFalse();
            }
        }

        /// <summary>
        /// Save author to the database and returns to the previous view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SaveAuthor()
        {
            try
            {
                this.SetIsBusyTrue();

                await this.ValidateFirstName();
                await this.ValidateLastName();

                if (this.AuthorFirstNameNotValid || this.AuthorLastNameNotValid)
                {
                    await this.DisplayMessage(AppStringResources.AuthorNameNotValid, null);
                    this.SetIsBusyFalse();
                }
                else
                {
#if ANDROID
                    if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                    {
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
                    }
#endif

                    this.EditedAuthor = await Database.SaveAuthorAsync(ConvertTo<AuthorDatabaseModel>(this.EditedAuthor));
                    await AddToStaticList(this.EditedAuthor);

                    if (this.InsertMainViewBefore)
                    {
                        var view = new AuthorMainView(this.EditedAuthor, $"{this.EditedAuthor.FullName}");
                        Shell.Current.Navigation.InsertPageBefore(view, this.View);
                    }

                    await Shell.Current.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                this.SetIsBusyFalse();
            }
        }

        /// <summary>
        /// Check if the author first name is valid and set the related value.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ValidateFirstName()
        {
            this.AuthorFirstNameNotValid = string.IsNullOrEmpty(this.EditedAuthor.FirstName);
        }

        /// <summary>
        /// Check if the author last name is valid and set the related value.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ValidateLastName()
        {
            this.AuthorLastNameNotValid = string.IsNullOrEmpty(this.EditedAuthor.LastName);
        }

        private static async Task<bool> AddAuthorToStaticList(AuthorModel author, ObservableCollection<AuthorModel> authorList, ObservableCollection<AuthorModel>? filteredAuthorList)
        {
            var refresh = false;

            // await Task.WhenAll(new Task[]
            // {
            //    author.SetTotalBooks(Hi),
            //    author.SetTotalCostOfBooks(true),
            // });
            try
            {
                var oldAuthor = authorList.FirstOrDefault(x => x.AuthorGuid == author.AuthorGuid);

                if (oldAuthor != null)
                {
                    var index = authorList.IndexOf(oldAuthor);
                    authorList.Remove(oldAuthor);
                    authorList.Insert(index, author);
                    refresh = true;
                }
                else
                {
                    authorList.Add(author);
                    refresh = true;
                }

                if (filteredAuthorList != null)
                {
                    var filteredAuthor = filteredAuthorList.FirstOrDefault(x => x.AuthorGuid == author.AuthorGuid);

                    if (filteredAuthor != null)
                    {
                        var index = filteredAuthorList.IndexOf(filteredAuthor);
                        filteredAuthorList.Remove(filteredAuthor);
                        filteredAuthorList.Insert(index, author);
                        refresh = true;
                    }
                    else
                    {
                        filteredAuthorList.Add(author);
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
