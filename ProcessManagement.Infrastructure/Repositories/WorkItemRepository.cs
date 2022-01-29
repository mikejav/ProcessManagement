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
    class WorkItemRepository : IWorkItemRepository
    {
        private readonly ProcessManagementDbContext _processManagementDbContext;

        public WorkItemRepository(ProcessManagementDbContext pocessManagementDbContext)
        {
            _processManagementDbContext = pocessManagementDbContext;
        }

        public WorkItem Add(WorkItem workItem)
        {
            var project = _processManagementDbContext.Projects.FirstOrDefault(p => p.Id == workItem.Project.Id);
            workItem.Project = project;
            var createdWorkItem = _processManagementDbContext.WorkItems.Add(workItem);

            return createdWorkItem.Entity;
        }

        public List<WorkItem> Get(ISpecification<WorkItem> specification)
        {
            IQueryable<WorkItem> query = _processManagementDbContext.WorkItems;

            if (specification.Includes != null) {
                query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (specification.Criteria != null) {
                query = query.Where(specification.Criteria);
            }

            return query.ToList();
        }

        public void Remove(WorkItem workItem)
        {
            _processManagementDbContext.WorkItems.Remove(workItem);
        }
        
        public void RemoveRange(IEnumerable<WorkItem> workItems)
        {
            _processManagementDbContext.WorkItems.RemoveRange(workItems);
        }

        public WorkItem Update(WorkItem workItem)
        {
            var updatedWorkItem = _processManagementDbContext.WorkItems.Update(workItem);
            return updatedWorkItem.Entity;
        }

        public void SaveChanges()
        {
            _processManagementDbContext.SaveChanges();
        }
    }
}
