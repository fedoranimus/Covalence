using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;

namespace Covalence
{
    public interface ILocationService {
        Task<Location> AddLocationAsync(ApplicationUser user, double latitude, double longitude);
    }

    public class LocationService : ILocationService {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LocationService> _logger;
        public LocationService(ApplicationDbContext context, ILoggerFactory loggerFactory) 
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<LocationService>();
        }

        public async Task<Location> AddLocationAsync(ApplicationUser user, double latitude, double longitude) {
            Location location = new Location(latitude, longitude);
            if(!location.Users.Contains(user))
                location.Users.Add(user);
            
            if(await _context.Locations.FindAsync(latitude, longitude) == null) 
            {
                await _context.Locations.AddAsync(location);
            }
            else 
            {
                _context.Locations.Update(location);
            }

            await _context.SaveChangesAsync();

            return location;
        }
    }
}