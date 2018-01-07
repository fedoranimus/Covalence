using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;

namespace Covalence
{
    public interface IConnectionService {
        Task RequestConnectionAsync(ApplicationUser RequestingUser, ApplicationUser RequestedUser);
        Task AcceptConnectionAsync(string RequestingUserId, string RequestedUserId);
        Task RejectConnectionAsync(string requestingUserId, string requestedUserId);
        Task<List<Connection>> GetConnectionsForUserAsync(string userId);
    }

    public class ConnectionService : IConnectionService {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ConnectionService> _logger;
        public ConnectionService(ApplicationDbContext context, ILoggerFactory loggerFactory) 
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<ConnectionService>();
        }

        public async Task RequestConnectionAsync(ApplicationUser requestingUser, ApplicationUser requestedUser) { //TODO - Ensure the connection doesn't exist yet
            var connection = new Connection() {
                RequestedUser = requestedUser,
                RequestedUserId = requestedUser.Id,
                RequestingUser = requestingUser,
                RequestingUserId = requestingUser.Id
            };

            var connections = await _context.Connections.AddAsync(connection);

            await _context.SaveChangesAsync();
        }

        public async Task AcceptConnectionAsync(string requestingUserId, string requestedUserId) { // TODO - Ensure connection exists
            var connection = await _context.Connections.FindAsync(requestingUserId, requestedUserId);
            connection.State = ConnectionState.Connected;

            await _context.SaveChangesAsync();
        }

        public async Task RejectConnectionAsync(string requestingUserId, string requestedUserId) { // TODO - Ensure connection exists
            var connection = await _context.Connections.FindAsync(requestingUserId, requestedUserId);
            _context.Connections.Remove(connection);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Connection>> GetConnectionsForUserAsync(string userId) {
            var connections = await _context.Connections.Where(x => x.RequestedUserId == userId || x.RequestingUserId == userId).Include(x => x.RequestedUser).Include(x => x.RequestingUser).ToListAsync();
            return connections;
        }
    }
}