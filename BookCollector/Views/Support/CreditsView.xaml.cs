// <copyright file="CreditsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Support;

using System.Windows.Input;

/// <summary>
/// CreditsView class.
/// </summary>
public partial class CreditsView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreditsView"/> class.
    /// </summary>
    public CreditsView()
    {
        this.InitializeComponent();
        this.BindingContext = this;
    }

    /// <summary>
    /// Gets the command to open the URL in the default browser when a link is tapped.
    /// </summary>
    public static ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
}