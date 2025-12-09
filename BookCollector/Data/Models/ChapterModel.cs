using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.Models
{
    public partial class ChapterModel : ObservableObject, ICloneable
    {
        [PrimaryKey]
        public Guid? ChapterGuid { get; set; }

        [ObservableProperty]
        public string? chapterName;
        [ObservableProperty]
        public string? pageRange;

        public int ChapterOrder { get; set; }
        public Guid BookGuid { get; set; }

        public ChapterModel()
        {
            ChapterGuid = Guid.NewGuid();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
