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
            var location = await _context.Locations.FindAsync(latitude, longitude);
            if(location == null) 
            {
                location = new Location(latitude, longitude);
                location.Users.Add(user);
                await _context.Locations.AddAsync(location);
            }
            else 
            {
                
                if(!location.Users.Contains(user))
                    location.Users.Add(user);
                _context.Locations.Update(location);
            }

            await _context.SaveChangesAsync();

            return location;
        }
    }
}