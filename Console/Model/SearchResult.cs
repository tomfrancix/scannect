using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scannect.Models;

namespace ScannectConsole.Model
{
    public class SearchResult
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
