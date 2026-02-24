// <copyright file="BookCoverMatchingPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups
{
    using CommunityToolkit.Maui.Views;

    public partial class BookCoverMatchingPopup : Popup<bool>
    {
        public BookCoverMatchingPopup(ImageSource bookCover, string bookTitle)
        {
            this.BookCover = bookCover;
            this.BookTitle = bookTitle;

            this.BindingContext = this;

            this.InitializeComponent();
        }

        public ImageSource BookCover { get; set; }

        public string BookTitle { get; set; }

        private async void OnNoButton_Clicked(object sender, EventArgs e)
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            await this.CloseAsync(false, token: cts.Token);
        }

        private async void OnYesButton_Clicked(object sender, EventArgs e)
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            await this.CloseAsync(true, token: cts.Token);
        }
    }
}