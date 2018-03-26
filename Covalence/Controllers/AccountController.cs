using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using Covalence.ViewModels;


// https://github.com/aspnet/Docs/blob/master/aspnetcore/migration/1x-to-2x/samples/AspNetCoreDotNetFx2.0App/AspNetCoreDotNetFx2.0App/Controllers/AccountController.cs
namespace Covalence.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        
        public AccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<AccountController> logger, IEmailSender emailSender) 
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
        }

        // POST: /Account/Register
        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            _logger.LogInformation("Registering {0}", model.Email);
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    _logger.LogInformation("Registered {0}", model.Email);

                    try {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                        await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                    } catch (Exception e) {
                        _logger.LogError(e.Message);
                    }

                    return Ok();
                }

                AddErrors(result);
            }

            // If we got this far, something bad happened
            return BadRequest(ModelState);
        }

        [Route("forgotpassword")]
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Ok();
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendForgotPasswordAsync(model.Email, callbackUrl);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if(userId == null || code == null)
            {
                return BadRequest("Code not included");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                // Don't reveal the user does not exist
                return BadRequest("User does not exist");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return Redirect("https://becovalent.com/");
        }

        [HttpPost]
        [Route("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                // Don't reveal that the user does not exist
                return Ok();
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if(result.Succeeded)
            {
                return Ok();
            }

            AddErrors(result);
            return BadRequest("Something went wrong");
        }

        
        [HttpPost]
        [Route("resendVerification")]
        public async Task<IActionResult> ResendVerificationEmail() {
            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }

            try {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);
            } catch (Exception e) {
                _logger.LogError(e.Message);
            }

            return BadRequest();
        }

        #region Helpers

        private void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}
