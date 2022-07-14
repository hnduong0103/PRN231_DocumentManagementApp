using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.DBModels
{
    public partial class Document
    {
        public int DocumentId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }
        public string DocumentTags { get; set; }
        public int? ProjectId { get; set; }
        public int? UsersUserId { get; set; }

        public virtual Project Project { get; set; }
        public virtual User UsersUser { get; set; }
    }
}
