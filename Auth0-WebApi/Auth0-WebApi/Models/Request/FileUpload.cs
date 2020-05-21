using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auth0.WebApi.Models
{
    public class FileUpload
    {
        [Required]
        public int companyId { get; set; }
        [Required]
        public Guid Id { get; set; }
        //[Required]
        //public string DocType { get; set; }
        [Required]
        public IFormFile file { get; set; }
    }
}
