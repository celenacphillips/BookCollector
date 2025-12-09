using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Author;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            View = view;

            EditedAuthor = (AuthorModel)author.Clone();
        }

        public void SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                AuthorFirstNameValid = !string.IsNullOrEmpty(EditedAuthor.FirstName);
                AuthorLastNameValid = !string.IsNullOrEmpty(EditedAuthor.LastName);

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SaveAuthor()
        {
            ValidateFirstName();
            ValidateLastName();

            if (!AuthorFirstNameValid || !AuthorLastNameValid)
            {
                await DisplayMessage(AppStringResources.AuthorNameNotValid, null);
            }
            else
            {
#if ANDROID
                if (Platform.CurrentActivity != null &&
                Platform.CurrentActivity.Window != null)
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (!string.IsNullOrEmpty(ViewTitle) && ViewTitle.Equals($"{AppStringResources.AddNewAuthor}"))
                {
                    if (TestData.UseTestData)
                    {
                        TestData.InsertAuthor(EditedAuthor);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (TestData.UseTestData)
                    {
                        TestData.UpdateAuthor(EditedAuthor);
                    }
                    else
                    {

                    }
                }

                var view = new AuthorMainView(EditedAuthor, $"{EditedAuthor.FullName}");
                Shell.Current.Navigation.InsertPageBefore(view, View);
                await Shell.Current.Navigation.PopAsync();
            }
        }

        [RelayCommand]
        public void Refresh()
        {
            SetRefreshTrue();
            SetViewModelData();
            SetRefreshFalse();
        }

        [RelayCommand]
        public void ValidateFirstName()
        {
            if (string.IsNullOrEmpty(EditedAuthor.FirstName))
            {
                var firstNameEditor = View.FindByName<Editor>("FirstNameEditor");
                firstNameEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                firstNameEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                AuthorFirstNameValid = false;
            }
            else
            {
                var firstNameEditor = View.FindByName<Editor>("FirstNameEditor");
                firstNameEditor.TextColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                firstNameEditor.PlaceholderColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                AuthorFirstNameValid = true;
            }
        }

        [RelayCommand]
        public void ValidateLastName()
        {
            if (string.IsNullOrEmpty(EditedAuthor.LastName))
            {
                var lastNameEditor = View.FindByName<Editor>("LastNameEditor");
                lastNameEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                lastNameEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                AuthorLastNameValid = false;
            }
            else
            {
                var lastNameEditor = View.FindByName<Editor>("LastNameEditor");
                lastNameEditor.TextColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                lastNameEditor.PlaceholderColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                AuthorLastNameValid = true;
            }
        }
    }
}
