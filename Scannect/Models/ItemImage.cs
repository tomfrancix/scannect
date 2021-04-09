using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scannect.Models
{
    public class ItemImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public string Title { get; set; }
        public string Annotation { get; set; }
        public string Placeholder { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
