using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DocumentViewModel
    {
        public int DocumentId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
