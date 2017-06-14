using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covalence.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Covalence {
    public interface IPostService {

    }

    public class PostService : IPostService {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TagService> _logger;
        public PostService(ApplicationDbContext context, ILoggerFactory loggerFactory) 
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<TagService>();
        }


    }
}