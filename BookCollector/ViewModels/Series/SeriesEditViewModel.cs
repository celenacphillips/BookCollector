using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Series;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Series
{
    public partial class SeriesEditViewModel : SeriesBaseViewModel
    {
        [ObservableProperty]
        public SeriesModel editedSeries;

        [ObservableProperty]
        public bool seriesNameValid;

        public bool InsertMainViewBefore {  get; set; }

        public SeriesEditViewModel(SeriesModel series, ContentPage view)
        {
            View = view;

            EditedSeries = (SeriesModel)series.Clone();
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
        public async Task SaveSeries()
        {
            if (!SeriesNameValid)
            {
                await DisplayMessage(AppStringResources.SeriesNameNotValid, null);
            }
            else
            {
#if ANDROID
                if (Platform.CurrentActivity != null &&
                Platform.CurrentActivity.Window != null)
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (!string.IsNullOrEmpty(ViewTitle) && ViewTitle.Equals($"{AppStringResources.AddNewSeries}"))
                {
                    if (TestData.UseTestData)
                    {
                        TestData.InsertSeries(EditedSeries);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (TestData.UseTestData)
                    {
                        TestData.UpdateSeries(EditedSeries);
                    }
                    else
                    {

                    }
                }
                if (InsertMainViewBefore)
                {
                    var view = new SeriesMainView(EditedSeries, $"{EditedSeries.SeriesName}");
                    Shell.Current.Navigation.InsertPageBefore(view, View);
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

        [RelayCommand]
        public void ValidateSeriesName()
        {
            ValidateEntry();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(EditedSeries.SeriesName))
            {
                var seriesNameEditor = View.FindByName<Editor>("SeriesNameEditor");
                seriesNameEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                seriesNameEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                SeriesNameValid = false;
            }
            else
            {
                var seriesNameEditor = View.FindByName<Editor>("SeriesNameEditor");
                seriesNameEditor.TextColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                seriesNameEditor.PlaceholderColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                SeriesNameValid = true;
            }
        }
    }
}
