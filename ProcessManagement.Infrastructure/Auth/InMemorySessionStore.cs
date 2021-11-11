using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Infrastructure.Auth
{
    class InMemorySessionStore : ISessionStore
    {
        private readonly IMemoryCache _cache;

        public InMemorySessionStore()
        {
            _cache = new MemoryCache(new MemoryCacheOptions
            {
            });
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public Task RenewAsync(string key, SessionModel sessionModel)
        {
            _cache.Set(key, sessionModel);
            return Task.CompletedTask;
        }

        public Task<SessionModel> RetrieveAsync(string key)
        {
            var sessionModel = _cache.Get<SessionModel>(key);
            return Task.FromResult(sessionModel);
        }

        public Task<string> StoreAsync(SessionModel sessionModel)
        {
            var key = sessionModel.Id;

            _cache.Set(key, sessionModel);

            return Task.FromResult(key);
        }
    }
}
