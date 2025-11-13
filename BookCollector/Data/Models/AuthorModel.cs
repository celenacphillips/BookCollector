using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.Models
{
    public partial class AuthorModel : ObservableObject
    {
        [PrimaryKey]
        public Guid? AuthorGuid { get; set; }

        [ObservableProperty]
        public string firstName;
        [ObservableProperty]
        public string lastName;
        [ObservableProperty]
        public string fullName;
        [ObservableProperty]
        public string? totalBooksString;

        public int AuthorTotalBooks { get; set; }
        public double TotalCostOfBooks { get; set; }
    }
}
