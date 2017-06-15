using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Covalence.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : Controller
    {

        private readonly ITagService _service;
        private readonly ILogger<TagsController> _logger;

        public TagsController(ITagService service, ILogger<TagsController> logger) {
            _service = service;
            _logger = logger;
        }
        // GET api/tags
        [HttpGet]
        public async Task<IEnumerable<Tag>> Get()
        {
            return await _service.GetAllTags();
        }

        // POST api/tags
        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] string tagName)
        {
            var existingTag = await _service.GetTag(tagName);
            if(existingTag != null) 
            {
                var error = "Tag already exists";
                _logger.LogError(error);
                return BadRequest(error);
            } 
            else 
            {
                //TODO Return the tag that actually will be created?
                await _service.CreateTag(tagName);
                return Ok();
            }
        }

        // GET api/tags/olo
        [HttpGet("query/{query}")]
        public async Task<IEnumerable<Tag>> Get(string query)
        {
            return await _service.QueryTags(query);
        }
    }
}
