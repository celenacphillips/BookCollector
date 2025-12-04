using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.Data.Models
{
    public class PriceModel
    {
        public Guid BookGuid { get; set; }
        public Guid? SeriesGuid { get; set; }
        public Guid? CollectionGuid { get; set; }
        public Guid? GenreGuid { get; set; }
        public Guid? LocationGuid { get; set; }
        public List<Guid>? AuthorGuids { get; set; }
        public double? Price { get; set; }
    }
}
