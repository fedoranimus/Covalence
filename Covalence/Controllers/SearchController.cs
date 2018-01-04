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

        public SearchController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ITagService tagService, ILogger<UserController> logger) {
            _userManager = userManager;
            _tagService = tagService;
            _logger = logger;
            _context = context;
        }

        [HttpPost("list/{page?}")]
        public async Task<IActionResult> GetAll(int? page) {
            var currentUser = await _userManager.GetUserAsync(User);

            if(currentUser == null)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            var users = await _context.Users.Where(u => u.Id != currentUser.Id).Include(x => x.Tags).ThenInclude(ut => ut.Tag).ToListAsync();
            var connections = await _context.Connections.Where(x => x.RequestedUserId == currentUser.Id || x.RequestingUserId == currentUser.Id).Include(x => x.RequestedUser).Include(x => x.RequestingUser).ToListAsync();

            var contract = Converters.ConvertRemoteUserListToContract(currentUser, users, connections);

            var pageSize = 3; // TODO
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

            var users = await _context.Users.Where(u => u.Id != currentUser.Id)
                                            .Include(x => x.Tags)
                                            .ThenInclude(ut => ut.Tag)
                                            .ToListAsync();

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

            var connections = await _context.Connections.Where(x => x.RequestedUserId == currentUser.Id || x.RequestingUserId == currentUser.Id).Include(x => x.RequestedUser).Include(x => x.RequestingUser).ToListAsync();

            var contract = Converters.ConvertRemoteUserListToContract(currentUser, sortedUsers, connections);
            return Ok(contract);
        }
    }
}