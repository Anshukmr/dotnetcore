using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auth0.WebApi.Models.Request
{
    public class UserInput
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = "Please enter valid RoleId. 1. Editor, 2. Viewer, 3. Admin")]
        public int Role { get; set; }
    }
}
