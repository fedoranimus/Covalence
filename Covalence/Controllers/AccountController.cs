using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Covalence.Authentication;
using System;
using Covalence.ViewModels;

namespace Covalence.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;
        
        public AccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<AccountController> logger) 
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        // POST: /Account/Register
        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            _logger.LogInformation("Registering {0} {1} with email {2}", model.FirstName, model.LastName, model.Email);
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, Location = model.Location };
                var result = await _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    _logger.LogInformation("Registered {0}", model.Email);
                    return Ok();
                }

                AddErrors(result);
            }

            // If we got this far, something bad happened
            return BadRequest(ModelState);
        }

        // Need to create an email confirmation endpoint and send a confirmation email
        // http://benjii.me/2017/02/send-email-using-asp-net-core/

        #region Helpers

        private void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}
