using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.Models
{
    public partial class ChapterModel : ObservableObject, ICloneable
    {
        [ObservableProperty]
        public string? chapterName;
        [ObservableProperty]
        public string? pageRange;

        public ChapterModel()
        {
            this.ChapterGuid = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid? ChapterGuid { get; set; }

        public int ChapterOrder { get; set; }

        public Guid BookGuid { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
