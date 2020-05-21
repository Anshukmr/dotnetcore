using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentalVerse.Web.Api.Models.Request
{
    public class DocumentInput
    {
        public Guid Id { get; set; }
        public string DocType { get; set; }
        public string Author { get; set; }
        public string Location { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? Published { get; set; }
        public int? Shared { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CustomPropertyX { get; set; }
        public int? IsAnimating { get; set; }
    }

}
