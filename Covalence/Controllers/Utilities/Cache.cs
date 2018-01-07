
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Covalence {
    public interface ICache {
        Task<List<ApplicationUser>> CacheTryGetUsersAsync();
        Task<List<Connection>> CacheTryGetConnectionsAsync();
    }
    public class Cache : ICache {
        private IMemoryCache _cache;
        private ApplicationDbContext _context;
        public Cache(IMemoryCache cache, ApplicationDbContext context) {
            _cache = cache;
            _context = context;
        }
        public async Task<List<ApplicationUser>> CacheTryGetUsersAsync() {
            List<ApplicationUser> users;
            
            if(!_cache.TryGetValue(CacheKeys.Users, out users))
            {
                users = await _context.Users.Include(x => x.Tags).ThenInclude(ut => ut.Tag).ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(CacheKeys.Users, users, cacheEntryOptions);
            }

            return users;
        }

        public async Task<List<Connection>> CacheTryGetConnectionsAsync() {
            List<Connection> connections;

            if(!_cache.TryGetValue(CacheKeys.Connections, out connections))
            {
                connections = await _context.Connections.Include(x => x.RequestedUser).Include(x => x.RequestingUser).ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(CacheKeys.Connections, connections, cacheEntryOptions);
            }

            return connections;
        }
    }
}