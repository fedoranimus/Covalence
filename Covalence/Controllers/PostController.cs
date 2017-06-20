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

namespace Covalence.Controllers {
    [Authorize]
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
                    var contract = ConvertPostToContract(post);
                    return Ok(contract);
                }
                catch(Exception e) 
                {
                    return BadRequest($"Failed to create post: {e}");
                }
            }    
            return BadRequest("ViewModel is invalid");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts() 
        {
            try
            {
                var posts = await _service.GetAllPosts();
                var contracts = new List<PostContract>();
                foreach(var post in posts)
                {
                    contracts.Add(ConvertPostToContract(post));
                }

                return Ok(contracts);
            }
            catch(Exception e) 
            {
                return BadRequest(e);
            }   
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(int postId) 
        {
            try
            {
                var post = await _service.GetPost(postId);
                var contract = ConvertPostToContract(post);
                
                return Ok(contract);
            }
            catch(Exception e)
            {
                return BadRequest(e);
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
                return BadRequest(e);
            }
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] PostViewModel model)
        {
            try
            {
                var post = await _service.UpdatePost(postId, model);
                var contract = ConvertPostToContract(post);
                return Ok(contract);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        private PostContract ConvertPostToContract(Post post) 
        {
            var remoteUserContract = new RemoteUserContract(){
                Id = post.Author.Id,
                FirstName = post.Author.FirstName,
                LastName = post.Author.LastName
            };

            return new PostContract()
                {
                    PostId = post.PostId,
                    Author = remoteUserContract,
                    Category = post.Category,
                    Content = post.Content,
                    DateModified = post.DateModified,
                    DateCreated = post.DateCreated,
                    Tags = post.Tags
                        .Select(ut => new TagContract(){
                            Name = ut.Tag.Name
                        }),
                    Title = post.Title
                };
        }
    }
}