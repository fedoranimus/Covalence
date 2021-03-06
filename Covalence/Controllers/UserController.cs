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
using System;
using OpenIddict.Validation;

namespace Covalence.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITagService _tagService;
        private readonly ApplicationDbContext _context;
        private readonly ILocationService _locationService;
        private readonly IConnectionService _connectionService;
        public UserController(UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context, 
            ITagService tagService, 
            ILogger<UserController> logger, 
            ILocationService locationService, 
            IConnectionService connectionService) {
            _userManager = userManager;
            _tagService = tagService;
            _logger = logger;
            _context = context;
            _locationService = locationService;
            _connectionService = connectionService;
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

            await _context.Entry(user).Reference(x => x.Location).LoadAsync(); // TODO: Apply this other places we need to eagerly load navigation properties
            await _context.Entry(user).Collection(x => x.Tags).LoadAsync();

            // var populatedUser = _context.Users.Where(x => x.Id == user.Id)
            //     .Include(x => x.Tags)
            //         .ThenInclude(ut => ut.Tag)
            //     .FirstOrDefault();

            var userContract = Converters.ConvertUserToContract(user);

            return Ok(userContract); 
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var user = await _userManager.GetUserAsync(User);
            var remoteUser = await _context.Users.FindAsync(userId);
            var connections = await _connectionService.GetConnectionsForUserAsync(user.Id);

            if(remoteUser == null || user == null) {
                _logger.LogError("User or RemoteUser not found");
                return BadRequest();
            }

            var remoteUserContract = Converters.ConvertRemoteUserToContract(user, remoteUser, connections);
            return Ok(remoteUserContract);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserViewModel model) // TODO: Simplify this logic
        {
            var user = await _userManager.GetUserAsync(User);

            if(user == null && user.Id == userId)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            if(ModelState.IsValid)
            {
                user = await _context.Users
                                .Where(u => u.Id == user.Id)
                                .Include(u => u.Location)
                                .Include(u => u.Tags)
                                    .ThenInclude(ut => ut.Tag)
                                .FirstOrDefaultAsync();
                    
                user.FirstName = model.FirstName == null ? user.FirstName : model.FirstName;
                user.LastName = model.LastName == null ? user.LastName : model.LastName;
                user.Email = model.Email == null ? user.Email : model.Email;
                user.Bio = model.Bio == null ? user.Bio : model.Bio;
                user.UserName = user.Email;
                
                if(model.ShareLocation != null) {
                    if(model.ShareLocation == true)
                        user.Location = model.Latitude == null || model.Longitude == null ? user.Location : await _locationService.AddUpdateLocationAsync(user, (double)model.Latitude, (double)model.Longitude);
                    else
                        user = await _locationService.RemoveLocationAsync(user);
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
