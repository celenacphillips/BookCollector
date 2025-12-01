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
            _view = view;

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
            if (SeriesNameValid)
            {
#if ANDROID
                if (Platform.CurrentActivity != null &&
                Platform.CurrentActivity.Window != null)
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (ViewTitle.Equals($"{AppStringResources.AddNewSeries}"))
                {
                    // Unit test data
                    TestData.InsertSeries(EditedSeries);
                }
                else
                {
                    // Unit test data
                    TestData.UpdateSeries(EditedSeries);
                }
                if (InsertMainViewBefore)
                {
                    SeriesMainView view = new SeriesMainView(EditedSeries, $"{EditedSeries.SeriesName}");
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
            if (string.IsNullOrEmpty(EditedSeries.SeriesName))
                SeriesNameValid = false;
            else
                SeriesNameValid = true;
        }
    }
}
