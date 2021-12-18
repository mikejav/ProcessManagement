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
    class UserRepository : IUserRepository
    {
        private readonly ProcessManagementDbContext _processManagementDbContext;

        public UserRepository(ProcessManagementDbContext pocessManagementDbContext)
        {
            _processManagementDbContext = pocessManagementDbContext;
        }

        public List<User> Get(ISpecification<User> specification)
        {
            IQueryable<User> query = _processManagementDbContext.Users;

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

        public User Add(User user)
        {
            var createdUser = _processManagementDbContext.Users.Add(user);

            return createdUser.Entity;
        }

        public void Remove(User user)
        {
            throw new NotImplementedException();
        }

        public User Update(User user)
        {
            var updatedUser = _processManagementDbContext.Users.Update(user);
            return updatedUser.Entity;
        }

        public void SaveChanges()
        {
            _processManagementDbContext.SaveChanges();
        }
    }
}
