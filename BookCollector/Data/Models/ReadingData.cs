// <copyright file="ReadingData.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class ReadingData : ObservableObject
    {
        [ObservableProperty]
        public int year;

        [ObservableProperty]
        public int booksReadCount;

        [ObservableProperty]
        public int pagesReadCount;

        [ObservableProperty]
        public double audiobookTime;
    }
}
