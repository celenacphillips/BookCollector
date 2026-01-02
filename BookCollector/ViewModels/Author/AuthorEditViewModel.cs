// <copyright file="AuthorEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Groupings;
using BookCollector.Views.Author;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Author
{
    public partial class AuthorEditViewModel : AuthorBaseViewModel
    {
        [ObservableProperty]
        public AuthorModel editedAuthor;

        [ObservableProperty]
        public bool authorFirstNameValid;

        [ObservableProperty]
        public bool authorLastNameValid;

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

                if (!this.AuthorFirstNameValid || !this.AuthorLastNameValid)
                {
                    await DisplayMessage(AppStringResources.AuthorNameNotValid, null);
                }
                else
                {
#if ANDROID
                    if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                    {
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
                    }
#endif

                    if (TestData.UseTestData)
                    {
                        TestData.UpdateAuthor(this.EditedAuthor);
                    }
                    else
                    {
                        this.EditedAuthor = await Database.SaveAuthorAsync(ConvertTo<AuthorDatabaseModel>(this.EditedAuthor));
                        AddToStaticList(this.EditedAuthor);
                    }

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
                await DisplayMessage("Error!", ex.Message);
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
            if (string.IsNullOrEmpty(this.EditedAuthor.FirstName))
            {
                var firstNameEditor = this.View.FindByName<Editor>("FirstNameEditor");
                firstNameEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                firstNameEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                this.AuthorFirstNameValid = false;
            }
            else
            {
                var userAppTheme = Application.Current?.UserAppTheme == AppTheme.Unspecified ? Application.Current?.PlatformAppTheme : Application.Current?.UserAppTheme;

                var firstNameEditor = this.View.FindByName<Editor>("FirstNameEditor");
                firstNameEditor.TextColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                firstNameEditor.PlaceholderColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                this.AuthorFirstNameValid = true;
            }
        }

        [RelayCommand]
        public void ValidateLastName()
        {
            if (string.IsNullOrEmpty(this.EditedAuthor.LastName))
            {
                var lastNameEditor = this.View.FindByName<Editor>("LastNameEditor");
                lastNameEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                lastNameEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                this.AuthorLastNameValid = false;
            }
            else
            {
                var userAppTheme = Application.Current?.UserAppTheme == AppTheme.Unspecified ? Application.Current?.PlatformAppTheme : Application.Current?.UserAppTheme;

                var lastNameEditor = this.View.FindByName<Editor>("LastNameEditor");
                lastNameEditor.TextColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                lastNameEditor.PlaceholderColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                this.AuthorLastNameValid = true;
            }
        }

        public static async Task AddToStaticList(AuthorModel author)
        {
            if (AuthorsViewModel.fullAuthorList != null)
            {
                AuthorsViewModel.RefreshView = await AddAuthorToStaticList(author, AuthorsViewModel.fullAuthorList, AuthorsViewModel.filteredAuthorList);
            }
        }

        private static async Task<bool> AddAuthorToStaticList(AuthorModel author, ObservableCollection<AuthorModel> authorList, ObservableCollection<AuthorModel>? filteredAuthorList)
        {
            var refresh = false;

            await Task.WhenAll(new Task[]
            {
                author.SetTotalBooks(true),
                author.SetTotalCostOfBooks(true),
            });

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
