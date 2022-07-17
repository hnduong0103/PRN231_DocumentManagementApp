using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserCreateModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "User Name must have atleast 6 characters")]
        public string UserName { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password must have atleast 6 characters")]
        public string Password { get; set; }
        [Required]
        public string UserRole { get; set; }
    }
}
