using System.Collections.Generic;
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
        public IEnumerable<Tag> Get()
        {
            return _service.GetAllTags();
        }

        // GET api/tags/5
        [HttpGet("{id}")]
        public Tag Get(int id)
        {
            return _service.GetTagById(id);
        }

        // GET api/tags/olo
        [HttpGet("query/{query}")]
        public IEnumerable<Tag> Get(string query)
        {
            return _service.QueryTags(query);
        }
    }
}
