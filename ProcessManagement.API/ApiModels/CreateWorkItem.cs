using ProcessManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessManagement.API.ApiModels
{
    public class CreateWorkItem
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public string Description { get; set; }

        public WorkItem ToWorkItem(string userId)
        {
            return new WorkItem()
            {
                Name = this.Name,
                DueDate = this.DueDate,
                Description = this.Description,
                Project = new Project {
                    Id = this.ProjectId,
                },
                CreatedBy = userId,
            };
        }
    }
}
