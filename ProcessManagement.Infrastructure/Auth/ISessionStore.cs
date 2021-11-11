using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Infrastructure.Auth
{
    interface ISessionStore
    {
        Task RemoveAsync(string key);
        Task RenewAsync(string key, SessionModel sessionModel);
        Task<SessionModel> RetrieveAsync(string key);
        Task<string> StoreAsync(SessionModel sessionModel);
    }
}
