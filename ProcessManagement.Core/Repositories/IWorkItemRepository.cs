using ProcessManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Core.Repositories
{
    public interface IWorkItemRepository
    {
        List<WorkItem> Get(ISpecification<WorkItem> specification);
        WorkItem Add(WorkItem workItem);
        WorkItem Update(WorkItem workItem);
        void Remove(WorkItem workItem);
        void SaveChanges();
    }
}
