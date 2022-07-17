using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProjectCreateModel
    {
        [Required]
        [MinLength(6,ErrorMessage ="Project Name must have atleast 6 characters")]
        public string ProjectName { get; set; }
    }
}
