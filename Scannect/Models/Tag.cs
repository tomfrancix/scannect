using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scannect.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Hits { get; set; }
        public string Ranking { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
