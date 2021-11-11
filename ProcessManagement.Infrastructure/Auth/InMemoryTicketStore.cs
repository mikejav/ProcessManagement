using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Infrastructure.Auth
{
    public class InMemoryTicketStore : ITicketStore
    {
        private readonly IMemoryCache _cache;

        public InMemoryTicketStore()
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

        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var ticket = _cache.Get<AuthenticationTicket>(key);

            return Task.FromResult(ticket);
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            _cache.Set(key, ticket);

            return Task.CompletedTask;
        }

        public Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = ticket.Principal.Claims
              .First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            _cache.Set(key, ticket);

            return Task.FromResult(key);
        }
    }
}
