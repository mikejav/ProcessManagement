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
        public DateTime? DueDate { get; set; }
        public WorkItemStatus Status { get; set; }
        public string Description { get; set; }

        public WorkItem ToWorkItem(string projectId, string userId)
        {
            return new WorkItem()
            {
                Id = projectId,
                Name = this.Name,
                DueDate = this.DueDate,
                Status = this.Status,
                Description = this.Description,
                CreatedBy = userId,
            };
        }
    }
}
