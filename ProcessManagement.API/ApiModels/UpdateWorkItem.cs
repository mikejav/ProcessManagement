using ProcessManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessManagement.API.ApiModels
{
    public class UpdateWorkItem
    {
        public string Name { get; set; }
        public WorkItemStatus Status { get; set; }
        public string Description { get; set; }

        public WorkItem ToWorkItem(string projectId)
        {
            return new WorkItem()
            {
                Id = projectId,
                Name = this.Name,
                Status = this.Status,
                Description = this.Description,
            };
        }
    }
}
