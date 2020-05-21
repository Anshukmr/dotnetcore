using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth0.WebApi.Models.Response
{
    public class DocumentOutput
    {
        public Guid Id { get; set; }
        public string DocType { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
    }
}
