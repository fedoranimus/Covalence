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

namespace Covalence.Controllers
{
   // [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class ConnectionController : Controller
    {
        private readonly ILogger<ConnectionController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITagService _tagService;
        private readonly ApplicationDbContext _context;
        private readonly ICache _cache;
        private readonly IConnectionService _connectionService;
        private readonly IEmailSender _emailSender;

        public ConnectionController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ITagService tagService, ILogger<ConnectionController> logger, ICache cache, IConnectionService connectionService, IEmailSender emailSender) {
            _userManager = userManager;
            _tagService = tagService;
            _logger = logger;
            _context = context;
            _cache = cache;
            _connectionService = connectionService;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> GetConnectionListForUser() {
            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                return BadRequest();
            }

            var connections = await _context.Connections.Where(x => x.RequestedUserId == user.Id || x.RequestingUserId == user.Id).Include(x => x.RequestedUser).Include(x => x.RequestingUser).ToListAsync();

            var connectionListContract = Converters.ConvertConnectionListToContract(connections, user.Id);

            return Ok(connectionListContract);
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestConnection([FromBody] UserIdWrapper requestedUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null || !user.EmailConfirmed)
            {
                return BadRequest();
            }

            try 
            {
                var connection = await _context.Connections.Where(x => x.RequestingUserId == user.Id && x.RequestedUserId == requestedUserId.Id).SingleOrDefaultAsync();
                if(connection == null) {
                    var requestingUser = await _context.Users.Where(x => x.Id == requestedUserId.Id).SingleOrDefaultAsync();
                    await _connectionService.RequestConnectionAsync(user, requestingUser);

                    var connections = await _connectionService.GetConnectionsForUserAsync(user.Id);
                    var connectionListContract = Converters.ConvertConnectionListToContract(connections, user.Id);

                    try {
                        await _emailSender.SendConnectionRequestedAsync(user.Email);
                    } catch (Exception e) {
                        _logger.LogDebug(e.Message);
                    }

                    return Ok(connectionListContract);
                } else {
                    return BadRequest("Connection already exists");
                }
            }
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
                return BadRequest("Failed to request new connection");
            }
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveConnection([FromBody] UserIdWrapper requestingUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            try 
            {
                await _connectionService.AcceptConnectionAsync(requestingUserId.Id, user.Id);

                var connections = await _connectionService.GetConnectionsForUserAsync(user.Id);
                var connectionListContract = Converters.ConvertConnectionListToContract(connections, user.Id);

                try {
                    await _emailSender.SendConnectionAcceptedAsync(user.Email);
                } catch (Exception e) {
                    _logger.LogDebug(e.Message);
                }
                

                return Ok(connectionListContract);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest("Connection Not Found");
            }
            
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectConnection([FromBody] UserIdWrapper requestingUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            try 
            {
                await _connectionService.RejectConnectionAsync(requestingUserId.Id, user.Id);

                var connections = await _connectionService.GetConnectionsForUserAsync(user.Id);
                var connectionListContract = Converters.ConvertConnectionListToContract(connections, user.Id);

                return Ok(connectionListContract);
            }
            catch (Exception e) {
                _logger.LogError(e.Message);
                return BadRequest("Connection Not Found");
            }
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelConnection([FromBody] UserIdWrapper requestedUserId) {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            try 
            {
                //var requestedUser = await _context.Users.Where(x => x.Id == requestedUserId.Id).SingleOrDefaultAsync();
                await _connectionService.RejectConnectionAsync(user.Id, requestedUserId.Id);

                var connections = await _connectionService.GetConnectionsForUserAsync(user.Id);
                var connectionListContract = Converters.ConvertConnectionListToContract(connections, user.Id);

                return Ok(connectionListContract);
            } 
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
                return BadRequest("Connection Not Found");
            }
        }

        // [HttpPost("disconnect")]
        // public async Task<IActionResult> DisconnectConnection([FromBody] UserIdWrapper requestedUserId) 
        // {
        //     var user = await _userManager.GetUserAsync(User);
        //     if(user == null)
        //     {
        //         return BadRequest();
        //     }    

        //     try 
        //     {
        //         await _connectionService.RejectConnectionAsync(user.Id, requestedUserId.Id);

        //         var connections = await _connectionService.GetConnectionsForUserAsync(user.Id);
        //         var connectionListContract = Converters.ConvertConnectionListToContract(connections, user.Id);

        //         return Ok(connectionListContract);
        //     }
        //     catch (Exception e)
        //     {
        //         _logger.LogError(e.Message);
        //         return BadRequest("Connection Not Found");
        //     }
        // }

        [HttpPost("block")]
        public async Task<IActionResult> BlockConnection([FromBody] UserIdWrapper requestingUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            // TODO - Implement

            return Ok();
        }

    }

    public class UserIdWrapper {
        public string Id { get; set; }
    }
}