using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covalence.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Covalence.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class TagController : Controller
    {

        private readonly ITagService _service;
        private readonly ILogger<TagController> _logger;

        public TagController(ITagService service, ILogger<TagController> logger) {
            _service = service;
            _logger = logger;
        }
        // GET api/tag
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _service.GetAllTags();
            var contracts = tags.Select(x => Converters.ConvertTagToContract(x)).ToList();
            return Ok(contracts);
        }

        [HttpGet("{tagName}")]
        public async Task<IActionResult> GetTag(string tagName)
        {
            if(tagName == null)
            {
                return BadRequest("tagName is null");
            }
            var existingTag = await _service.GetTag(tagName);
            var contract = Converters.ConvertTagToContract(existingTag);
            
            return Ok(contract);
        }

        // POST api/tag
        [HttpPost("{tagName}")]
        public async Task<IActionResult> CreateTag(string tagName)
        {
            if(tagName == null) 
            {
                return BadRequest("tagName is null");
            }
            var existingTag = await _service.GetTag(tagName);
            if(existingTag != null) 
            {
                var error = "Tag already exists";
                _logger.LogError(error);
                return BadRequest(error);
            } 
            else 
            {
                var tag = await _service.CreateTag(tagName);
                var contract = Converters.ConvertTagToContract(tag);
                return Ok(contract);
            }
        }

        // GET api/tag/olo
        [HttpGet("query/{query}")]
        public async Task<IActionResult> Get(string query)
        {
            if(query == null) 
            {
                return BadRequest("query is null");
            }
            var tags = await _service.QueryTags(query);
            var contracts = tags.Select(x => Converters.ConvertTagToContract(x)).ToList();
            return Ok(contracts);
        }
    }
}
