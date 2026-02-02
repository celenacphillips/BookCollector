// <copyright file="CreditsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Windows.Input;

namespace BookCollector.Views.Support;

public partial class CreditsView : ContentPage
{
    public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

    public CreditsView()
    {
        this.InitializeComponent();
        this.BindingContext = this;
    }
}