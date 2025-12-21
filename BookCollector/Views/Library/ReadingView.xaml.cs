// <copyright file="ReadingView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class ReadingView : ContentPage
{
    public ReadingView()
    {
        // Put on first view to set the status bar to whatever color the user wants the app to be.
        var savedColor = Preferences.Get("AppColor", "#336699" /* Default */);

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            try
            {
                CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb(savedColor));
            }
            catch
            {
            }
        }

        this.ViewModel = new ReadingViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private ReadingViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.ViewModel.SetViewModelData();
    }
}