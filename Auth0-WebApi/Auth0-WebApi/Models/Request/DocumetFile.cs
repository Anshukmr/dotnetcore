using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth0.WebApi.Models.Request
{
    public class DocumetFile
    {      
        public IFormFile file { get; set; }
    }
}
