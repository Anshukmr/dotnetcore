using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Auth0.WebApi.Models
{
    public partial class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string DocType { get; set; }
        public int? CompanyId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string URI { get; set; }
        public string SASToken { get; set; }
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
