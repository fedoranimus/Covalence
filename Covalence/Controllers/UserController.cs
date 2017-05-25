using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Identity;
using Covalence.Authentication;
using Covalence.Contracts;
using Microsoft.EntityFrameworkCore;
using System;

namespace Covalence.Controllers
{
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

        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            var populatedUser = _context.Users.Where(x => x.Id == user.Id)
                .Include(x => x.StudyTags)
                    .ThenInclude(ut => ut.Tag)
                .Include(x => x.ExpertTags)
                    .ThenInclude(ut => ut.Tag)
                .FirstOrDefault();

            var userContract = new UserContract()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Location = user.Location,
                Email = user.Email,
                StudyTags = populatedUser.StudyTags
                    .Select(ut => new TagContract(){
                        Name = ut.Tag.Name,
                        Description = ut.Tag.Description
                    }),
                ExpertTags = populatedUser.ExpertTags
                    .Select(ut => new TagContract(){
                        Name = ut.Tag.Name,
                        Description = ut.Tag.Description
                    })
            };

            return Ok(userContract); 
        }

        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("add/tag/{tagType}/{tagName}")]
        public async Task<IActionResult> AddTagToUser(int tagType, string tagName) 
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            if(user == null) 
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            if(!Enum.IsDefined(typeof(TagType), tagType))
            {
                _logger.LogError("No tagType corresponding to {0}", tagType);
                return BadRequest();
            }

            TagType type = (TagType)tagType;

            var tag = _tagService.GetTagByName(tagName);
            if(tag == null)
            {
                _logger.LogError("No tag corresponding to tagName '{0}'", tagName);
                return BadRequest();
            }

            user = await _tagService.AddUserToTag(tag, type, user);
            var result = await _userManager.UpdateAsync(user);

            if(result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                _logger.LogError("Updating Tag failed");
                return BadRequest();
            } 
            
        }

        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("remove/tag/{tagType}/{tagName}")]
        public async Task<IActionResult> RemoveTagFromUser(int tagType, string tagName) 
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            var tag = _tagService.GetTagByName(tagName);

            _logger.LogInformation("Removing Tag: {0}", tag.ToString());
            return Ok();
        }

        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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

[       Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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

        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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

        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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
