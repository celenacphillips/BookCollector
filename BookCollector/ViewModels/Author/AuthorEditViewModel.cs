using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
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
        public bool authorNameValid;

        public AuthorEditViewModel(AuthorModel author, ContentPage view)
        {
            _view = view;

            EditedAuthor = (AuthorModel)author.Clone();
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            ValidateEntry();

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SaveAuthor()
        {
            if (AuthorNameValid)
            {
#if ANDROID
                if (Platform.CurrentActivity != null &&
                Platform.CurrentActivity.Window != null)
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (ViewTitle.Equals($"{AppStringResources.AddNewAuthor}"))
                {
                    // Unit test data
                    TestData.InsertAuthor(EditedAuthor);
                }
                else
                {
                    // Unit test data
                    TestData.UpdateAuthor(EditedAuthor);
                }

                AuthorMainView view = new AuthorMainView(EditedAuthor, $"{EditedAuthor.FullName}");
                Shell.Current.Navigation.InsertPageBefore(view, _view);
                await Shell.Current.Navigation.PopAsync();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(EditedAuthor.FirstName) ||
                string.IsNullOrEmpty(EditedAuthor.LastName))
                AuthorNameValid = false;
            else
                AuthorNameValid = true;
        }
    }
}
