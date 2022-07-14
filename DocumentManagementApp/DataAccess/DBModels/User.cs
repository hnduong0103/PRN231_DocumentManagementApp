using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.DBModels
{
    public partial class User
    {
        public User()
        {
            Documents = new HashSet<Document>();
            Projects = new HashSet<Project>();
        }

        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
