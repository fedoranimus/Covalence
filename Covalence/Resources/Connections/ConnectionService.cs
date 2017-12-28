using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;

namespace Covalence
{
    public interface IConnectionService {
        Task RequestConnection(ApplicationUser RequestingUser, ApplicationUser RequestedUser);
        Task AcceptConnection(ApplicationUser RequestingUser, ApplicationUser RequestedUser);
    }

    public class ConnectionService : IConnectionService {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ConnectionService> _logger;
        public ConnectionService(ApplicationDbContext context, ILoggerFactory loggerFactory) 
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<ConnectionService>();
        }

        public async Task RequestConnection(ApplicationUser requestingUser, ApplicationUser requestedUser) {
            var connection = new Connection() {
                RequestedUser = requestedUser,
                RequestedUserId = requestedUser.Id,
                RequestingUser = requestingUser,
                RequestingUserId = requestingUser.Id
            };

            var connections = await _context.Connections.AddAsync(connection);

            await _context.SaveChangesAsync();
        }

        public async Task AcceptConnection(ApplicationUser requestingUser, ApplicationUser requestedUser) {
            var connection = await _context.Connections.FindAsync(requestingUser.Id, requestedUser.Id);
            connection.State = ConnectionState.Connected;

            await _context.SaveChangesAsync();
        }

        public async Task RejectConnection() {
            await _context.SaveChangesAsync();
        }
    }
}