using BookCollector.Data;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;

namespace BookCollector.Views.Popups;

public partial class FilterPopup : Popup
{
    public FilterPopup()
	{
        InitializeComponent();
    }
}