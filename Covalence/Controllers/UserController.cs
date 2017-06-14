using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Covalence.Authentication;
using Covalence.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Covalence.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize(ActiveAuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITagService _tagService;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ITagService tagService, ILogger<UserController> logger) {
            _userManager = userManager;
            _tagService = tagService;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(User);

            var claims = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();
            if(user == null)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            var populatedUser = _context.Users.Where(x => x.Id == user.Id)
                .Include(x => x.Tags)
                    .ThenInclude(ut => ut.Tag)
                .FirstOrDefault();

            var userContract = new UserContract()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Location = user.Location,
                Email = user.Email,
                Tags = populatedUser.Tags
                    .Select(ut => new TagContract(){
                        Name = ut.Tag.Name,
                        Description = ut.Tag.Description
                    })//,
                //AuthoredPosts = populatedUser.AuthoredPosts
            };

            return Ok(userContract); 
        }

        [HttpPost("tag/{tagName}")]
        public async Task<IActionResult> AddTagToUser(string tagName) 
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null) 
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            var tag = _tagService.GetTag(tagName);
            if(tag == null)
            {
                var error = $"No tag corresponding to '{tagName}'";
                _logger.LogError(error);
                return BadRequest(error);
            }

            user = await _tagService.AddTag(tag, user);
            var result = await _userManager.UpdateAsync(user);

            if(result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                var error = "Updating tag failed";
                _logger.LogError(error);
                return BadRequest(error);
            } 
        }

        [HttpDelete("tag/{tagName}")]
        public async Task<IActionResult> RemoveTagFromUser(string tagName) 
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            var tag = _tagService.GetTag(tagName);
            if(tag == null)
            {
                var error = $"No tag corresponding to '{tagName}'";
                _logger.LogError(error);
                return BadRequest(error);
            }

            user = await _tagService.RemoveTag(tag, user);
            var result = await _userManager.UpdateAsync(user);

            _logger.LogInformation("Removing Tag: {0}", tag.ToString());

            if(result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                var error = "Updating tag failed";
                _logger.LogError(error);
                return BadRequest(error);
            }
        }

        [HttpPost("request/connection/{requestedUserId}")]
        public async Task<IActionResult> RequestConnection(string requestedUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("approve/connection/{requestedUserId}")]
        public async Task<IActionResult> ApproveConnection(string requestedUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("reject/connection/{requestedUserId}")]
        public async Task<IActionResult> RejectConnection(string requestedUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("block/connection/{requestedUserId}")]
        public async Task<IActionResult> BlockConnection(string requestedUserId)
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
