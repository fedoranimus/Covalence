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
    //[Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Authorize(AuthenticationSchemes = "Bearer")]
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
                    
                user.FirstName = model.FirstName == null ? user.FirstName : model.FirstName;
                user.LastName = model.LastName == null ? user.LastName : model.LastName;
                user.IsMentor = (bool)(model.IsMentor == null ? user.IsMentor : model.IsMentor);
                user.Email = model.Email == null ? user.Email : model.Email;
                user.UserName = user.Email;
                user.ZipCode = model.ZipCode == null ? user.ZipCode : model.ZipCode;

                if(user.ZipCode == null)
                {
                    user.Location = model.Latitude == null || model.Longitude == null ? user.Location : new Location((double)model.Latitude, (double)model.Longitude);
                }
                else
                {
                    var zipLocation = await _context.ZipCodes.FindAsync(model.ZipCode);
                    if(zipLocation != null)
                        user.Location = new Location(zipLocation.Latitude, zipLocation.Longitude);
                }

                if(await _context.Locations.FindAsync(user.Location) != null) {
                    await _context.Locations.AddAsync(user.Location);
                }

                await _context.SaveChangesAsync();

                user = await UpdateUserTags(user, model.Tags);
                user.NeedsOnboarding = (bool)(model.NeedsOnboarding == null ? user.NeedsOnboarding : model.NeedsOnboarding);

                await _userManager.UpdateAsync(user);
                var contract = Converters.ConvertUserToContract(user);

                return Ok(contract);
            }
            return BadRequest(ModelState);
        }

        private async Task<ApplicationUser> UpdateUserTags(ApplicationUser user, List<string> tags) {
            if(tags != null)
            {
                //user = await _context.Users.Where(u => u.Id == user.Id).Include(x => x.Tags).ThenInclude(ut => ut.Tag).FirstOrDefaultAsync();
                var tagsToRemove = user.Tags.Select(t => t.Name.ToLowerInvariant()).Except(tags).ToList(); // Get list of current tags not in the new tag list
                var tagsToAdd = tags.Except(user.Tags.Select(t => t.Name.ToLowerInvariant())).ToList(); // Get list of new tags which aren't in the current tag list
                user = await _tagService.RemoveTags(tagsToRemove, user); //can I clear this maybe?
                user = await _tagService.AddTags(tagsToAdd, user);
            }

            return user;
        }

        // [HttpPut("tags/{userId}")]
        // public async Task<IActionResult> UpdateUserTags(string userId, [FromBody] List<string> tags)
        // {
        //     var user = await _userManager.GetUserAsync(User);

        //     if(user == null && user.Id == userId)
        //     {
        //         _logger.LogError("User not found");
        //         return BadRequest();
        //     }

        //     if(tags != null)
        //     {
        //         user = await _context.Users.Where(u => u.Id == user.Id).Include(x => x.Tags).ThenInclude(ut => ut.Tag).FirstOrDefaultAsync();
        //         var tagsToRemove = user.Tags.Select(t => t.Name.ToLowerInvariant()).Except(tags).ToList(); // Get list of current tags not in the new tag list
        //         var tagsToAdd = tags.Except(user.Tags.Select(t => t.Name.ToLowerInvariant())).ToList(); // Get list of new tags which aren't in the current tag list
        //         user = await _tagService.RemoveTags(tagsToRemove, user); //can I clear this maybe?
        //         user = await _tagService.AddTags(tagsToAdd, user);

        //         await _userManager.UpdateAsync(user);
        //         var contract = Converters.ConvertUserToContract(user);

        //         return Ok(contract);
        //     }

        //     return BadRequest("Invalid Tag List");
        // }
    }
}
