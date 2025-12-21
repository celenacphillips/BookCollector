// <copyright file="CollectionEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Collection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Collection
{
    public partial class CollectionEditViewModel : CollectionBaseViewModel
    {
        [ObservableProperty]
        public CollectionModel editedCollection;

        [ObservableProperty]
        public bool collectionNameValid;

        public CollectionEditViewModel(CollectionModel collection, ContentPage view)
        {
            this.View = view;

            this.EditedCollection = (CollectionModel)collection.Clone();
        }

        public bool InsertMainViewBefore { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.ValidateEntry();

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SaveCollection()
        {
            if (!this.CollectionNameValid)
            {
                await DisplayMessage(AppStringResources.CollectionNameNotValid, null);
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
                    TestData.UpdateCollection(this.EditedCollection);
                }
                else
                {
                    this.EditedCollection = await Database.SaveCollectionAsync(ConvertTo<CollectionDatabaseModel>(this.EditedCollection));
                }

                if (this.InsertMainViewBefore)
                {
                    CollectionMainView view = new CollectionMainView(this.EditedCollection, $"{this.EditedCollection.CollectionName}");
                    Shell.Current.Navigation.InsertPageBefore(view, this.View);
                }

                await Shell.Current.Navigation.PopAsync();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public void ValidateCollectionName()
        {
            this.ValidateEntry();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(this.EditedCollection.CollectionName))
            {
                var collectionNameEditor = this.View.FindByName<Editor>("CollectionNameEditor");
                collectionNameEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                collectionNameEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                this.CollectionNameValid = false;
            }
            else
            {
                var userAppTheme = Application.Current?.UserAppTheme == AppTheme.Unspecified ? Application.Current?.PlatformAppTheme : Application.Current?.UserAppTheme;

                var collectionNameEditor = this.View.FindByName<Editor>("CollectionNameEditor");
                collectionNameEditor.TextColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                collectionNameEditor.PlaceholderColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                this.CollectionNameValid = true;
            }
        }
    }
}
