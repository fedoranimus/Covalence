using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using AspNet.Security.OpenIdConnect.Server;
using Covalence.Authentication;
using AspNet.Security.OpenIdConnect.Primitives;
using OpenIddict.Core;
using OpenIddict.Models;

namespace Covalence.Controllers
{
    //[Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly OpenIddictApplicationManager<OpenIddictApplication> _applicationManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(
            OpenIddictApplicationManager<OpenIddictApplication> applicationManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<AuthorizationController> logger)
        {
            _applicationManager = applicationManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPostAttribute("connect/token"), ProducesAttribute("application/json")]
        public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
        {
            if(request.IsPasswordGrantType())
            {
                var user = await _userManager.FindByNameAsync(request.Username); //should this be email instead?
                if(user == null) 
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username or password is invalid."
                    });
                }

                // Ensure the user is allowed to log in
                if(!await _signInManager.CanSignInAsync(user))
                {
                    return BadRequest( new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user is not allowed to sign in."
                    });
                }

                // TODO two-factor auth test

                // TODO user lockout test

                // Ensure the password is valid
                if(!await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    if(_userManager.SupportsUserLockout)
                    {
                        await _userManager.AccessFailedAsync(user);
                    }

                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username or password is invalid."
                    });
                }

                if(_userManager.SupportsUserLockout)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                }

                // Create a new authentication ticket
                var ticket = await CreateTicketAsync(request, user);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified Grant Type is not supported."
            });
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, ApplicationUser user)
        {
            var principal  = await _signInManager.CreateUserPrincipalAsync(user);

            foreach(var claim in principal.Claims)
            {
                claim.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken,
                                        OpenIdConnectConstants.Destinations.IdentityToken);
            }

            var ticket = new AuthenticationTicket(
                principal, 
                new AuthenticationProperties(),
                OpenIdConnectServerDefaults.AuthenticationScheme
            );

            ticket.SetScopes(new[] 
            {
                OpenIdConnectConstants.Scopes.OpenId,
                OpenIdConnectConstants.Scopes.Email,
                OpenIdConnectConstants.Scopes.Profile,
                OpenIdConnectConstants.Scopes.OfflineAccess,
                OpenIddictConstants.Scopes.Roles
            }.Intersect(request.GetScopes()));

            return ticket;
        }

    }
}
