using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;

namespace Covalence
{
    public interface IConnectionService {
        Task<ApplicationUser> RequestConnection(ApplicationUser RequestingUser, ApplicationUser RequestedUser);
        Task<ApplicationUser> AcceptConnection(ApplicationUser RequestingUser, ApplicationUser RequestedUser);
    }

    public class ConnectionService : IConnectionService {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ConnectionService> _logger;
        public ConnectionService(ApplicationDbContext context, ILoggerFactory loggerFactory) 
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<ConnectionService>();
        }

        public async Task<ApplicationUser> RequestConnection(ApplicationUser requestingUser, ApplicationUser requestedUser) {
            requestingUser = await _context.Users.Where(u => u.Id == requestingUser.Id).Include(x => x.Connections).FirstOrDefaultAsync();
            requestedUser = await _context.Users.Where(u => u.Id == requestedUser.Id).Include(x => x.Connections).FirstOrDefaultAsync();

            var connection = new Connection() {
                RequestedUser = requestedUser,
                RequestedUserId = requestedUser.Id,
                RequestingUser = requestingUser,
                RequestingUserId = requestingUser.Id
            };

            try 
            {
                requestingUser.Connections.Add(connection);
                requestedUser.Connections.Add(connection);                
            }
            catch (Exception e) {
                _logger.LogError("Couldn't request connection");
                return requestingUser;
            }

            await _context.SaveChangesAsync();

            return requestingUser;
        }

        public async Task<ApplicationUser> AcceptConnection(ApplicationUser requestingUser, ApplicationUser requestedUser) {
            requestingUser = await _context.Users.Where(u => u.Id == requestingUser.Id).Include(x => x.Connections).FirstOrDefaultAsync();
            requestedUser = await _context.Users.Where(u => u.Id == requestedUser.Id).Include(x => x.Connections).FirstOrDefaultAsync();

            var requestedConnection = requestedUser.Connections.FirstOrDefault(c => c.RequestedUser == requestedUser && c.RequestingUser == requestingUser);
            requestedConnection.State = ConnectionState.Connected;

            var requestingConnection = requestingUser.Connections.FirstOrDefault(c => c.RequestedUser == requestedUser && c.RequestingUser == requestingUser);
            requestingConnection.State = ConnectionState.Connected;

            await _context.SaveChangesAsync();

            return requestedUser;
        }

        public async void RejectConnection() {

        }
    }
}