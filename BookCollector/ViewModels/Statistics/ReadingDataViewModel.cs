// <copyright file="ReadingDataViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Statistics
{
    using BookCollector.Data;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Library;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class ReadingDataViewModel : StatisticsBaseViewModel
    {
        [ObservableProperty]
        public List<ReadingData> readingDataList;

        [ObservableProperty]
        public bool yearCountNotValid;

        public ReadingDataViewModel(ContentPage view)
        {
            this.View = view;
            this.YearCount = $"{5}";
            this.InfoText = AppStringResources.ReadingData_InfoText.Replace("number", this.YearCount);
        }

        public string YearCount { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.GetPreferences();

                this.ReadingDataList = [];

                if (AllBooksViewModel.filteredBookList1 == null || AllBooksViewModel.RefreshView)
                {
                    await AllBooksViewModel.SetList(this.ShowHiddenBooks);
                }

                for (int i = 0; i < int.Parse(this.YearCount); i++)
                {
                    var year = DateTime.Now.Year - i;

                    var bookReadCount = GetCounts.GetBookCountReadInYear(year);
                    var pageReadCount = GetCounts.GetBookPageCountReadInYear(year);
                    var listenedTimeCount = GetCounts.GetBookTimeCountReadInYear(year);

                    await Task.WhenAll(bookReadCount, pageReadCount);

                    this.ReadingDataList.Add(
                        new ReadingData
                        {
                            Year = year,
                            BooksReadCount = bookReadCount.Result,
                            PagesReadCount = pageReadCount.Result,
                            AudiobookTime = listenedTimeCount.Result,
                        });
                }

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
#if DEBUG
                await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                await DisplayMessage(AppStringResources.AnErrorOccurred, null);
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

        [RelayCommand]
        public async Task UpdateMaxNumber()
        {
            if (!string.IsNullOrEmpty(this.YearCount) && int.Parse(this.YearCount) < 21 && int.Parse(this.YearCount) > 0)
            {
                this.YearCountNotValid = false;
                await this.SetViewModelData();
                this.InfoText = AppStringResources.ReadingData_InfoText.Replace("number", this.YearCount.ToString());
            }
            else
            {
                this.YearCountNotValid = true;
            }
        }
    }
}
