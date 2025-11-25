using BookCollector.Data.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels
{
    public partial class SeriesBaseViewModel : BaseViewModel
    {
        [ObservableProperty]
        public static ObservableCollection<SeriesModel>? fullSeriesList;

        [ObservableProperty]
        public static ObservableCollection<SeriesModel>? filteredSeriesList;

        [ObservableProperty]
        public SeriesModel? selectedSeries;

        [RelayCommand]
        public async Task SeriesSelectionChanged()
        {

        }
    }
}
