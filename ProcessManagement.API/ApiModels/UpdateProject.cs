using ProcessManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessManagement.API.ApiModels
{
    public class UpdateProject
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Project ToProject(string projectId)
        {
            return new Project()
            {
                Id = projectId,
                Name = this.Name,
                Description = this.Description,
            };
        }
    }
}
