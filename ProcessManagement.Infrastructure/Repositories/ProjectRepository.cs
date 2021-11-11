using Microsoft.EntityFrameworkCore;
using ProcessManagement.Core;
using ProcessManagement.Core.Entities;
using ProcessManagement.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Infrastructure.Repositories
{
    class ProjectRepository : IProjectRepository
    {
        private readonly ProcessManagementDbContext _processManagementDbContext;

        public ProjectRepository(ProcessManagementDbContext pocessManagementDbContext)
        {
            _processManagementDbContext = pocessManagementDbContext;
        }

        public List<Project> Get(ISpecification<Project> specification)
        {
            IQueryable<Project> query = _processManagementDbContext.Projects;

            if (specification.Includes != null)
            {
                query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            return query.ToList();
        }

        public Project Add(Project project)
        {
            var createdProject = _processManagementDbContext.Projects.Add(project);

            return createdProject.Entity;
        }

        public void Remove(Project project)
        {
            throw new NotImplementedException();
        }

        public Project Update(Project project)
        {
            var updatedProejct = _processManagementDbContext.Projects.Update(project);
            return updatedProejct.Entity;
        }

        public void SaveChanges()
        {
            _processManagementDbContext.SaveChanges();
        }
    }
}
