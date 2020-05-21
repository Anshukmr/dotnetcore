using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MentalVerse.Web.Api.Models
{
    public partial class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Auth0Domain { get; set; }
        public string Auth0ApiIdentifier { get; set; }
        public string Auth0ClientId { get; set; }
        public string Auth0ClientSecret { get; set; }
        public string AzureSQLCompanyDB { get; set; }
        public string AzureStorageAccount { get; set; }
        public string BlobContainer{ get; set; }
    }
}
