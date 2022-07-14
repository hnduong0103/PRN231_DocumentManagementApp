using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.DBModels
{
    public partial class Project
    {
        public Project()
        {
            Documents = new HashSet<Document>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? UsersUserId { get; set; }

        public virtual User UsersUser { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
