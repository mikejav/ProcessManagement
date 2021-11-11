using ProcessManagement.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Core
{
    public interface IUnitOfWork
    {
        IProjectRepository ProjectRepository { get; }
        IWorkItemRepository WorkItemRepository { get; }

        Task CompleteAsync();
    }
}
