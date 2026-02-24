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

    public partial class AuthorEditViewModel : AuthorBaseViewModel
    {
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public AuthorModel editedAuthor;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool authorFirstNameNotValid;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool authorLastNameNotValid;

        public AuthorEditViewModel(AuthorModel author, ContentPage view)
        {
            this.View = view;

            this.EditedAuthor = (AuthorModel)author.Clone();
        }

        public bool InsertMainViewBefore { get; set; }

        public void SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.ValidateFirstName();
                this.ValidateLastName();

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SaveAuthor()
        {
            try
            {
                this.SetIsBusyTrue();

                this.ValidateFirstName();
                this.ValidateLastName();

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

        [RelayCommand]
        public void Refresh()
        {
            this.SetRefreshTrue();
            this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public void ValidateFirstName()
        {
            this.AuthorFirstNameNotValid = string.IsNullOrEmpty(this.EditedAuthor.FirstName);
        }

        [RelayCommand]
        public void ValidateLastName()
        {
            this.AuthorLastNameNotValid = string.IsNullOrEmpty(this.EditedAuthor.LastName);
        }

        public static async Task AddToStaticList(AuthorModel author)
        {
            if (AuthorsViewModel.fullAuthorList != null)
            {
                AuthorsViewModel.RefreshView = await AddAuthorToStaticList(author, AuthorsViewModel.fullAuthorList, AuthorsViewModel.filteredAuthorList2);
            }
        }

        private static async Task<bool> AddAuthorToStaticList(AuthorModel author, ObservableCollection<AuthorModel> authorList, ObservableCollection<AuthorModel>? filteredAuthorList)
        {
            var refresh = false;

            //await Task.WhenAll(new Task[]
            //{
            //    author.SetTotalBooks(Hi),
            //    author.SetTotalCostOfBooks(true),
            //});

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
