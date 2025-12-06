using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Collection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Collection
{
    public partial class CollectionEditViewModel : CollectionBaseViewModel
    {
        [ObservableProperty]
        public CollectionModel editedCollection;

        [ObservableProperty]
        public bool collectionNameValid;

        public bool InsertMainViewBefore { get; set; }

        public CollectionEditViewModel(CollectionModel collection, ContentPage view)
        {
            _view = view;

            EditedCollection = (CollectionModel)collection.Clone();
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                ValidateEntry();

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SaveCollection()
        {
            if (CollectionNameValid)
            {
#if ANDROID
                if (Platform.CurrentActivity != null &&
                Platform.CurrentActivity.Window != null)
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (ViewTitle.Equals($"{AppStringResources.AddNewCollection}"))
                {
                    if (TestData.UseTestData)
                    {
                        TestData.InsertCollection(EditedCollection);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (TestData.UseTestData)
                    {
                        TestData.UpdateCollection(EditedCollection);
                    }
                    else
                    {

                    }
                }

                if (InsertMainViewBefore)
                {
                    CollectionMainView view = new CollectionMainView(EditedCollection, $"{EditedCollection.CollectionName}");
                    Shell.Current.Navigation.InsertPageBefore(view, _view);
                }

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
            if (string.IsNullOrEmpty(EditedCollection.CollectionName))
                CollectionNameValid = false;
            else
                CollectionNameValid = true;
        }
    }
}
