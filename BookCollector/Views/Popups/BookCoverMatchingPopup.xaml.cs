// <copyright file="BookCoverMatchingPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups
{
    using CommunityToolkit.Maui.Views;

    /// <summary>
    /// BookCoverMatchingPopup class.
    /// </summary>
    public partial class BookCoverMatchingPopup : Popup<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookCoverMatchingPopup"/> class.
        /// </summary>
        /// <param name="bookCover">Book cover to view.</param>
        /// <param name="bookTitle">Book title.</param>
        public BookCoverMatchingPopup(ImageSource bookCover, string bookTitle)
        {
            this.BookCover = bookCover;
            this.BookTitle = bookTitle;

            this.BindingContext = this;

            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the book cover image source.
        /// </summary>
        public ImageSource BookCover { get; set; }

        /// <summary>
        /// Gets or sets the book title.
        /// </summary>
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