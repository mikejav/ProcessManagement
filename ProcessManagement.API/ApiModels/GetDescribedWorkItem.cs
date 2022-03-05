using ProcessManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessManagement.API.ApiModels
{
    public class GetDescribedWorkItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public WorkItemStatus Status { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public static GetDescribedWorkItem FromWorkItem(WorkItem workItem)
        {
            return new GetDescribedWorkItem
            {
                Id = workItem.Id,
                Name = workItem.Name,
                DueDate = workItem.DueDate,
                Status = workItem.Status,
                Description = workItem.Description,
                ProjectId = workItem.Project.Id,
                ProjectName = workItem.Project.Name,
                CreatedAt = workItem.CreatedAt,
                ModifiedAt = workItem.ModifiedAt,
                CreatedBy = workItem.CreatedBy,
                ModifiedBy = workItem.ModifiedBy,

            };
        }
    }
}
