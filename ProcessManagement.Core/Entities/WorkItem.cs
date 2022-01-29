using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Core.Entities
{
    public class WorkItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public WorkItemStatus Status { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
