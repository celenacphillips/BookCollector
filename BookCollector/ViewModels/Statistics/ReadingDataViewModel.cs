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

    /// <summary>
    /// ReadingDataViewModel class.
    /// </summary>
    public partial class ReadingDataViewModel : StatisticsBaseViewModel
    {
        /// <summary>
        /// Gets or sets the reading data list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<ReadingData> readingDataList;

        /// <summary>
        /// Gets or sets a value indicating whether the year count input is valid or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool yearCountNotValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadingDataViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public ReadingDataViewModel(ContentPage view)
        {
            this.View = view;
            this.YearCount = $"{5}";
            this.InfoText = AppStringResources.ReadingData_InfoText.Replace("number", this.YearCount);
        }

        /// <summary>
        /// Gets or sets the year count.
        /// </summary>
        public string YearCount { get; set; }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async new Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.GetPreferences();

                this.ReadingDataList = [];

                if (AllBooksViewModel.hiddenFilteredBookList == null || AllBooksViewModel.RefreshView)
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
                await this.ViewModelCatch(ex);
            }
        }

        /// <summary>
        /// Update the max number of years of data to show.
        /// </summary>
        /// <returns>A task.</returns>
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
