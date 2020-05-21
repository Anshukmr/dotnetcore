using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentalVerse.Web.Api.Models.Request
{
    public class DocumetFile
    {      
        public IFormFile file { get; set; }
    }
}
