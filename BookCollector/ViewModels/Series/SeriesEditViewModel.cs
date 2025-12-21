// <copyright file="SeriesEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Series;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Series
{
    public partial class SeriesEditViewModel : SeriesBaseViewModel
    {
        [ObservableProperty]
        public SeriesModel editedSeries;

        [ObservableProperty]
        public bool seriesNameValid;

        public SeriesEditViewModel(SeriesModel series, ContentPage view)
        {
            this.View = view;

            this.EditedSeries = (SeriesModel)series.Clone();
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
        public async Task SaveSeries()
        {
            if (!this.SeriesNameValid)
            {
                await DisplayMessage(AppStringResources.SeriesNameNotValid, null);
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
                    TestData.UpdateSeries(this.EditedSeries);
                }
                else
                {
                    this.EditedSeries = await Database.SaveSeriesAsync(ConvertTo<SeriesDatabaseModel>(this.EditedSeries));
                }

                if (this.InsertMainViewBefore)
                {
                    var view = new SeriesMainView(this.EditedSeries, $"{this.EditedSeries.SeriesName}");
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
        public void ValidateSeriesName()
        {
            this.ValidateEntry();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(this.EditedSeries.SeriesName))
            {
                var seriesNameEditor = this.View.FindByName<Editor>("SeriesNameEditor");
                seriesNameEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                seriesNameEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                this.SeriesNameValid = false;
            }
            else
            {
                var userAppTheme = Application.Current?.UserAppTheme == AppTheme.Unspecified ? Application.Current?.PlatformAppTheme : Application.Current?.UserAppTheme;

                var seriesNameEditor = this.View.FindByName<Editor>("SeriesNameEditor");
                seriesNameEditor.TextColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                seriesNameEditor.PlaceholderColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                this.SeriesNameValid = true;
            }
        }
    }
}
