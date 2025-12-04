using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Main
{
    public partial class ExportImportViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string startOutput;

        [ObservableProperty]
        public string booksOutput;

        [ObservableProperty]
        public string wishListOutput;

        [ObservableProperty]
        public string collectionsOutput;

        [ObservableProperty]
        public string genresOutput;

        [ObservableProperty]
        public string seriesOutput;

        [ObservableProperty]
        public string authorsOutput;

        [ObservableProperty]
        public string locationsOutput;

        [ObservableProperty]
        public string finalOutput;

        [ObservableProperty]
        public bool booksChecked;

        [ObservableProperty]
        public bool wishListChecked;

        [ObservableProperty]
        public bool collectionsChecked;

        [ObservableProperty]
        public bool genresChecked;

        [ObservableProperty]
        public bool seriesChecked;

        [ObservableProperty]
        public bool authorsChecked;

        [ObservableProperty]
        public bool locationsChecked;

        [ObservableProperty]
        public bool checkboxesVisible;

        [ObservableProperty]
        public bool outputVisible;

        [ObservableProperty]
        public bool exportEnabled;

        [ObservableProperty]
        public bool importEnabled;


        public ExportImportViewModel(ContentPage view)
        {
            _view = view;
            InfoText = AppStringResources.ExportImportView_InfoText;
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            ExportEnabled = IsBusy;
            ImportEnabled = IsBusy;
            CheckboxesVisible = IsBusy;
            OutputVisible = !IsBusy;
            BooksChecked = true;
            WishListChecked = true;
            CollectionsChecked = true;
            GenresChecked = true;
            SeriesChecked = true;
            AuthorsChecked = true;
            LocationsChecked = true;

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task Export()
        {
            var action = await DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureExport_Question, null, null);

            if (action)
            {
                try
                {
                    SetIsBusyTrue();

                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;

                    var exportLocation = Preferences.Get("ExportLocation", AppStringResources.DefaultExportLocation  /* Default */);

                    ResetOutput();
                    OutputVisible = IsBusy;
                    CheckboxesVisible = !IsBusy;



                    SetIsBusyFalse();
                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;
                }
                catch (Exception ex)
                {
                    SetIsBusyFalse();
                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;
                }
            }
            else
            {
                await CanceledAction();
            }
        }

        [RelayCommand]
        public async Task Import()
        {
            var action = await DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureImport_Question, null, null);

            if (action)
            {
                try
                {
                    SetIsBusyTrue();

                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;

                    ResetOutput();
                    OutputVisible = IsBusy;
                    CheckboxesVisible = !IsBusy;



                    SetIsBusyFalse();
                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;
                }
                catch (Exception ex)
                {
                    SetIsBusyFalse();
                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;
                }
            }
            else
            {
                await CanceledAction();
            }
        }

        private void ResetOutput()
        {
            BooksOutput = string.Empty;
            WishListOutput = string.Empty;
            CollectionsOutput = string.Empty;
            GenresOutput = string.Empty;
            SeriesOutput = string.Empty;
            AuthorsOutput = string.Empty;
            LocationsOutput = string.Empty;
            FinalOutput = string.Empty;
        }
    }
}
