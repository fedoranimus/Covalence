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

        [HttpGet("list")]
        public async Task<IActionResult> GetAll() {
            var currentUser = await _userManager.GetUserAsync(User);

            if(currentUser == null)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            var users = await _context.Users.Where(u => u.Id != currentUser.Id).Include(x => x.Tags).ThenInclude(ut => ut.Tag).ToListAsync();

            var contract = Converters.ConvertRemoteUserListToContract(users);
            return Ok(contract);
        }

        [HttpPost("search")]
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

            var contract = Converters.ConvertRemoteUserListToContract(sortedUsers);
            return Ok(contract);
        }
    }
}