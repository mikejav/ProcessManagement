using ProcessManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Core.Services
{
    public interface IAuthService
    {
        public User GetLoggedInUser();
        public Task<User> SignInAsync(string email, string password);
        public Task SignOutAsync();
        public Task SignUpAsync(string name, string email, string password);
    }
}
