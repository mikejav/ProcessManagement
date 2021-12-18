using ProcessManagement.Core;
using ProcessManagement.Core.Repositories;
using ProcessManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Infrastructure
{
    class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ProcessManagementDbContext _processManagementDbContext;

        private Lazy<ProjectRepository> _projectRepository;

        public IProjectRepository ProjectRepository
        {
            get
            {
                return _projectRepository.Value;
            }
        }

        private Lazy<WorkItemRepository> _workItemRepository;

        public IWorkItemRepository WorkItemRepository
        {
            get
            {
                return _workItemRepository.Value;
            }
        }

        private Lazy<UserRepository> _userRepository;

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository.Value;
            }
        }

        public UnitOfWork(ProcessManagementDbContext pocessManagementDbContext)
        {
            _processManagementDbContext = pocessManagementDbContext;

            _projectRepository = new Lazy<ProjectRepository>(() => new ProjectRepository(_processManagementDbContext));
            _workItemRepository = new Lazy<WorkItemRepository>(() => new WorkItemRepository(_processManagementDbContext));
            _userRepository = new Lazy<UserRepository>(() => new UserRepository(_processManagementDbContext));
        }

        public Task CompleteAsync()
        {
            return _processManagementDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _processManagementDbContext.Dispose();
        }
    }
}
