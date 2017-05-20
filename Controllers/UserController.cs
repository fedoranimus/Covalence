using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Identity;
using Covalence.Authentication;
using Covalence.API.Tags;
using Covalence.API.Contracts;

namespace Covalence.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITagService _tagService;

        public UserController(UserManager<ApplicationUser> userManager, ITagService tagService, ILogger<UserController> logger) {
            _userManager = userManager;
            _tagService = tagService;
            _logger = logger;
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

            // var studyTags = _tagService.PopulateTags(user.StudyTags);
            // var expertTags = _tagService.PopulateTags(user.ExpertTags);

            var userContract = new UserContract()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Location = user.Location,
                Email = user.Email,
                StudyTags = user.StudyTags.Select(ut => ut.Tag).ToList(),
                ExpertTags = user.ExpertTags.Select(ut => ut.Tag).ToList()
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

            TagType type = (TagType)tagType;

            var tag = _tagService.GetTagByName(tagName);
            if(tag == null)
            {
                _logger.LogError("No tag corresponding to tagName '{0}'", tagName);
                return BadRequest();
            }

            if(type == TagType.Study)
            {
                if(user.StudyTags.Select(ut => ut.Tag).Contains(tag))
                {
                    _logger.LogInformation("{0} already assigned to {1}", tag.ToString(), user.ToString());
                }
                else
                {
                    _logger.LogInformation("{0} added to {1}", tag.ToString(), user.ToString());
                    
                    user = await _tagService.AddUserToTag(tag, type, user);
                }
            }
            else if(type == TagType.Expert)
            {
                if(user.ExpertTags.Select(ut => ut.Tag).Contains(tag))
                {
                    _logger.LogInformation("{0} already assigned to {1}", tag.ToString(), user.ToString());
                }
                else 
                {
                    _logger.LogInformation("{0} added to {1}", tag.ToString(), user.ToString());
                    
                    user = await _tagService.AddUserToTag(tag, type, user);
                }
            }
            else
            {
                _logger.LogError("No tagType corresponding to {0}", tagType);
                return BadRequest();
            }
            
            
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
