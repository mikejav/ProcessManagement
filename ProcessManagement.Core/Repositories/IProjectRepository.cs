using ProcessManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Core.Repositories
{
    public interface IProjectRepository
    {
        List<Project> Get(ISpecification<Project> specification);
        Project Add(Project project);
        Project Update(Project project);
        void Remove(Project project);
        void SaveChanges();
    }
}
