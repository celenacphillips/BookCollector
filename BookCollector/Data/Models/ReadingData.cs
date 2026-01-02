// <copyright file="ReadingData.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;

namespace BookCollector.Data.Models
{
    public partial class ReadingData : ObservableObject
    {
        [ObservableProperty]
        public int year;

        [ObservableProperty]
        public int booksReadCount;

        [ObservableProperty]
        public int pagesReadCount;
    }
}
