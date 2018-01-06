using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covalence.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Covalence.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Covalence.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITagService _tagService;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public SearchController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ITagService tagService, ILogger<UserController> logger, IMemoryCache memoryCache) {
            _userManager = userManager;
            _tagService = tagService;
            _logger = logger;
            _context = context;
            _cache = memoryCache;
        }

        [HttpPost("list/{page?}")]
        public async Task<IActionResult> GetAll(int? page) {
            var currentUser = await _userManager.GetUserAsync(User);

            if(currentUser == null)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            var users = await CacheTryGetUsersAsync();            
            users = users.Where(u => u.Id != currentUser.Id).ToList();
            
            var connections = await CacheTryGetConnectionsAsync();
            connections = connections.Where(x => x.RequestedUserId == currentUser.Id || x.RequestingUserId == currentUser.Id).ToList();

            var contract = Converters.ConvertRemoteUserListToContract(currentUser, users, connections);

            var pageSize = 3; // TODO - get parameter from UI
            var paginatedList = await PaginatedList<RemoteUserContract>.CreateAsync(contract, page ?? 1, pageSize);

            var pagedContract = Converters.ConvertPagingListToContract(paginatedList);

            return Ok(pagedContract);
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromBody] SearchViewModel model) {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if(currentUser == null)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            var users = await CacheTryGetUsersAsync();

            users = users.Where(u => u.Id != currentUser.Id).ToList();

            var tagCounts = new Dictionary<ApplicationUser, int>();

            foreach(var user in users) 
            {
                var tagCount = user.Tags.Select(tag => tag.Name)
                                        .Intersect(model.Tags)
                                        .Count();
                if(tagCount > 0)
                    tagCounts.Add(user, tagCount);
            }

            var sortedUsers = tagCounts.OrderBy(x => x.Value)
                                        .Select(x => x.Key)
                                        .ToList();

            var connections = await CacheTryGetConnectionsAsync();
            connections = connections.Where(x => x.RequestedUserId == currentUser.Id || x.RequestingUserId == currentUser.Id).ToList();

            var contract = Converters.ConvertRemoteUserListToContract(currentUser, sortedUsers, connections);

            var pageSize = 3; // TODO - get parameter from UI
            var paginatedList = await PaginatedList<RemoteUserContract>.CreateAsync(contract, model.Page ?? 1, pageSize);

            var pagedContract = Converters.ConvertPagingListToContract(paginatedList);

            return Ok(pagedContract);
        }

        private async Task<List<ApplicationUser>> CacheTryGetUsersAsync() {
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

        private async Task<List<Connection>> CacheTryGetConnectionsAsync() {
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