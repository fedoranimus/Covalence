using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Covalence.Authentication;
using Covalence.Contracts;
using Covalence.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Collections.Generic;

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

            if(user == null)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            var populatedUser = _context.Users.Where(x => x.Id == user.Id)
                .Include(x => x.Tags)
                    .ThenInclude(ut => ut.Tag)
                .FirstOrDefault();

            var userContract = Converters.ConvertUserToContract(populatedUser);

            return Ok(userContract); 
        }

        //TODO: Get UserById

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserViewModel model) 
        {
            var user = await _userManager.GetUserAsync(User);

            if(user == null && user.Id == userId)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            if(ModelState.IsValid)
            {
                user = await _context.Users.Where(u => u.Id == user.Id).Include(x => x.Tags).ThenInclude(ut => ut.Tag).FirstOrDefaultAsync();
                var userTags = model.Tags
                    .Select(async t => 
                        new UserTag() { User = user, UserId = user.Id, Tag = await _tagService.GetTag(t), Name = t.ToUpperInvariant()
                    }).ToList();
                    
                user.FirstName = model.FirstName == null ? user.FirstName : model.FirstName;
                user.LastName = model.LastName == null ? user.LastName : model.LastName;
                user.IsMentor = (bool)(model.IsMentor == null ? user.IsMentor : model.IsMentor);
                user.Email = model.Email == null ? user.Email : model.Email;
                user.UserName = user.Email;
                user.Tags = model.Tags == null ? user.Tags : await Task.WhenAll(userTags); //TODO: This is not efficient

                await _userManager.UpdateAsync(user);
                var contract = Converters.ConvertUserToContract(user);

                return Ok(contract);
            }
            return BadRequest("Invalid ViewModel");
        }

        [HttpPut("tags/{userId}")]
        public async Task<IActionResult> UpdateUserTags(string userId, [FromBody] List<string> tags)
        {
            var user = await _userManager.GetUserAsync(User);

            if(user == null && user.Id == userId)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            if(tags != null)
            {
                user = await _context.Users.Where(u => u.Id == user.Id).Include(x => x.Tags).ThenInclude(ut => ut.Tag).FirstOrDefaultAsync();
                var tagsToRemove = user.Tags.Select(t => t.Name.ToLowerInvariant()).Except(tags).ToList(); // Get list of current tags not in the new tag list
                var tagsToAdd = tags.Except(user.Tags.Select(t => t.Name.ToLowerInvariant())).ToList(); // Get list of new tags which aren't in the current tag list
                user = await _tagService.RemoveTags(tagsToRemove, user); //can I clear this maybe?
                user = await _tagService.AddTags(tagsToAdd, user);

                await _userManager.UpdateAsync(user);
                var contract = Converters.ConvertUserToContract(user);

                return Ok(contract);
            }

            return BadRequest("Invalid Tag List");
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
