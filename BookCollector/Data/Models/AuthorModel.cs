using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.Models
{
    public partial class AuthorModel : ObservableObject, ICloneable
    {
        [PrimaryKey]
        public Guid? AuthorGuid { get; set; }

        [ObservableProperty]
        public string firstName;
        [ObservableProperty]
        public string lastName;
        [ObservableProperty]
        public string? totalBooksString;
        [ObservableProperty]
        public bool hideAuthor;

        public string FullName
        {
            get => $"{this.FirstName} {this.LastName}";
        }
        public string ReverseFullName
        {
            get => $"{this.LastName}, {this.FirstName}";
        }

        public int AuthorTotalBooks { get; set; }
        public double TotalCostOfBooks { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
