using SQLite;

namespace BookCollector.Data.Models
{
    public class BookAuthorModel : ICloneable
    {
        [PrimaryKey]
        public Guid? BookAuthorGuid { get; set; }

        public Guid AuthorGuid { get; set; }
        public Guid BookGuid { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
}
