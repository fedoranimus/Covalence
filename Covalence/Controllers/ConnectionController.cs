using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Covalence.Contracts;
using Covalence.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AspNet.Security.OAuth.Validation;

namespace Covalence.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ConnectionController : Controller
    {
        private readonly ILogger<ConnectionController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITagService _tagService;
        private readonly ApplicationDbContext _context;

        public ConnectionController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ITagService tagService, ILogger<ConnectionController> logger) {
            _userManager = userManager;
            _tagService = tagService;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetConnectionListForUser() {
            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                return BadRequest();
            }

            var connections = await _context.Connections.Where(x => x.RequestedUserId == user.Id || x.RequestingUserId == user.Id).Include(x => x.RequestedUser).Include(x => x.RequestingUser).ToListAsync();
            var connectionListContract = Converters.ConvertConnectionListToContract(connections);

            return Ok(connectionListContract);
        }

        [HttpPost("connection/request")]
        public async Task<IActionResult> RequestConnection([FromBody] string requestedUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("connection/approve")]
        public async Task<IActionResult> ApproveConnection([FromBody] string requestingUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("connection/reject")]
        public async Task<IActionResult> RejectConnection([FromBody] string requestingUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("connection/block")]
        public async Task<IActionResult> BlockConnection([FromBody] string requestingUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            return Ok();
        }

    }

}