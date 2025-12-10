using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Author;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        public void SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.AuthorFirstNameValid = !string.IsNullOrEmpty(this.EditedAuthor.FirstName);
                this.AuthorLastNameValid = !string.IsNullOrEmpty(this.EditedAuthor.LastName);

                this.SetIsBusyFalse();
            }
            catch (Exception)
            {
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SaveAuthor()
        {
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

                if (!string.IsNullOrEmpty(this.ViewTitle) && this.ViewTitle.Equals($"{AppStringResources.AddNewAuthor}"))
                {
                    if (TestData.UseTestData)
                    {
                        TestData.InsertAuthor(this.EditedAuthor);
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (TestData.UseTestData)
                    {
                        TestData.UpdateAuthor(this.EditedAuthor);
                    }
                    else
                    {
                    }
                }

                var view = new AuthorMainView(this.EditedAuthor, $"{this.EditedAuthor.FullName}");
                Shell.Current.Navigation.InsertPageBefore(view, this.View);
                await Shell.Current.Navigation.PopAsync();
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
                var firstNameEditor = this.View.FindByName<Editor>("FirstNameEditor");
                firstNameEditor.TextColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                firstNameEditor.PlaceholderColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
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
                var lastNameEditor = this.View.FindByName<Editor>("LastNameEditor");
                lastNameEditor.TextColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                lastNameEditor.PlaceholderColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                this.AuthorLastNameValid = true;
            }
        }
    }
}
