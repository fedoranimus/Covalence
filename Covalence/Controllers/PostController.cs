using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covalence.Authentication;
using Covalence.Contracts;
using Covalence.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNet.Security.OAuth.Validation;

namespace Covalence.Controllers {
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        readonly ILogger<PostController> _logger;
        private readonly IPostService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostController(UserManager<ApplicationUser> userManager, IPostService service, ILogger<PostController> logger) {
            _logger = logger;
            _service = service;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostViewModel model) {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                _logger.LogError("User not found");
                return BadRequest();
            }
            
            if(ModelState.IsValid) {
                try 
                {
                    var post = await _service.CreatePost(user, model);
                    var contract = Converters.ConvertPostToContract(post);
                    return Ok(contract);
                }
                catch(Exception e) 
                {
                    return BadRequest("Failed to create post");
                }
            }    
            return BadRequest("ViewModel is invalid");
        }

        [HttpGet("{startIndex?}/{pageSize?}")]
        public async Task<IActionResult> GetAllPosts(int startIndex = 0, int pageSize = 20) 
        {
            try
            {
                var posts = await _service.GetAllPosts(startIndex, pageSize);
                var contracts = posts.Select(x => Converters.ConvertPostToContract(x)).ToList();

                return Ok(contracts);
            }
            catch(Exception e) 
            {
                return BadRequest("Failed to get posts");
            }   
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(int postId) 
        {
            try
            {
                var post = await _service.GetPost(postId);
                var contract = Converters.ConvertPostToContract(post);
                
                return Ok(contract);
            }
            catch(Exception e)
            {
                return BadRequest($"Failed to get post: {postId}");
            }
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try 
            {
                await _service.DeletePost(postId);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest("Failed to delete post");
            }
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] PostViewModel model)
        {
            try
            {
                var post = await _service.UpdatePost(postId, model);
                var contract = Converters.ConvertPostToContract(post);
                return Ok(contract);
            }
            catch(Exception e)
            {
                return BadRequest($"Failed to update post: {postId}");
            }
        }

        [HttpGet("/search/{query}/{startIndex?}/{pageSize?}")]
        public async Task<IActionResult> SearchPosts(string query, int startIndex = 0, int pageSize = 20)
        {
            try
            {
                //TODO
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest($"Unable to find posts");
            }
        }
    }
}