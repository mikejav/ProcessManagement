using ProcessManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessManagement.API.ApiModels
{
    public class CreateProject
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Project ToProject(string userId)
        {
            return new Project()
            {
                Name = this.Name,
                Description = this.Description,
                CreatedBy = userId,
            };
        }
    }
}
