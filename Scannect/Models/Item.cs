using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scannect.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string WebsiteUrl { get; set; }
        public string Title { get; set; }
        public string Snippet { get; set; }
        public DateTime DateCreated { get; set; }
        public int Hits { get; set; }
        public int Ranking { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public List<ItemImage> Images { get; set; }


        public List<Tag> Tags { get; set; }

    }
}
