using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookCollector.ViewModels.Statistics
{
    public partial class ReadingDataViewModel : StatisticsBaseViewModel
    {
        [ObservableProperty]
        public List<ReadingData> readingDataList;

        public ReadingDataViewModel(ContentPage view)
        {
            this.View = view;
            this.MaxListNumber = 5;
            this.InfoText = AppStringResources.ReadingData_InfoText.Replace("number", this.MaxListNumber.ToString());
        }

        private int MaxListNumber { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.GetPreferences();

                this.ReadingDataList = new List<ReadingData>();

                if (AllBooksViewModel.fullBookList == null)
                {
                    await AllBooksViewModel.SetList(this.ShowHiddenBooks);
                }

                for (int i = 0; i < this.MaxListNumber; i++)
                {
                    var year = DateTime.Now.Year - i;

                    var bookReadCount = GetCounts.GetBookCountReadInYear(year, this.ShowHiddenBooks);
                    var pageReadCount = GetCounts.GetBookPageCountReadInYear(year, this.ShowHiddenBooks);

                    await Task.WhenAll(bookReadCount, pageReadCount);

                    this.ReadingDataList.Add(
                        new ReadingData
                        {
                            Year = year,
                            BooksReadCount = bookReadCount.Result,
                            PagesReadCount = pageReadCount.Result,
                        });
                }

                this.SetIsBusyFalse();
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
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }
    }
}
