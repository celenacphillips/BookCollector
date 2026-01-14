// <copyright file="ReadingDataViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Library;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Statistics
{
    public partial class ReadingDataViewModel : StatisticsBaseViewModel
    {
        [ObservableProperty]
        public List<ReadingData> readingDataList;

        public ReadingDataViewModel(ContentPage view)
        {
            this.View = view;
            this.YearCount = $"{5}";
            this.InfoText = AppStringResources.ReadingData_InfoText.Replace("number", this.YearCount);
        }

        public string YearCount { get; set; }

        [ObservableProperty]
        public bool yearCountNotValid;

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

                for (int i = 0; i < int.Parse(this.YearCount); i++)
                {
                    var year = DateTime.Now.Year - i;

                    var bookReadCount = GetCounts.GetBookCountReadInYear(year, this.ShowHiddenBooks);
                    var pageReadCount = GetCounts.GetBookPageCountReadInYear(year, this.ShowHiddenBooks);
                    var listenedTimeCount = GetCounts.GetBookTimeCountReadInYear(year, this.ShowHiddenBooks);

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
