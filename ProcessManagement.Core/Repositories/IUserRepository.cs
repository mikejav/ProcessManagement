using ProcessManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Core.Repositories
{
    public interface IUserRepository
    {
        List<User> Get(ISpecification<User> specification);
        User Add(User user);
        User Update(User user);
        void Remove(User user);
        void SaveChanges();
    }
}
