using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Infrastructure.Auth
{
    class RedisSessionStore : ISessionStore
    {
        private readonly IDatabase _database;

        public RedisSessionStore(IDatabase database)
        {
            _database = database;
        }

        public Task RemoveAsync(string key)
        {
            return _database.KeyDeleteAsync(key);
        }

        public Task RenewAsync(string key, SessionModel sessionModel)
        {
            return Task.CompletedTask;
        }

        public Task<SessionModel> RetrieveAsync(string key)
        {
            var hash = _database.HashGetAll(key);
            if (hash.Length == 0) {
                return Task.FromResult<SessionModel>(null);
            }

            var dictionary = hash.ToStringDictionary();

            var sessionModel = new SessionModel
            {
                Id = key,
                UserNameIdentifier = dictionary["NameIdentifier"],
            };

            return Task.FromResult(sessionModel);
        }

        public Task<string> StoreAsync(SessionModel sessionModel)
        {
            var key = sessionModel.Id;
            _database.HashSet(key, new []
            {
                new HashEntry("NameIdentifier", sessionModel.UserNameIdentifier),
            });

            return Task.FromResult(key);
        }
    }
}
