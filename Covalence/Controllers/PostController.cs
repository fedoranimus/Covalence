using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Covalence.Controllers {
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        readonly ILogger<PostController> _logger;
        private readonly IPostService _service;
        public PostController(IPostService service, ILogger<PostController> logger) {
            _logger = logger;
            _service = service;
        }
    }
}