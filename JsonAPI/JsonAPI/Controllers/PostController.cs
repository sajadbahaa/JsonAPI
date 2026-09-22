using Application.Services;
using JsonAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JsonAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, [FromServices] IAuthorizationService authorizationService, CancellationToken cancellationToken)
        {
            var post = await _postService.GetAsync(id,cancellationToken);
            var authorizationResult = await authorizationService.AuthorizeAsync(User,post?.UserId,"UserOwnerOrAdmin");

            return Ok(post);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<List<Post>> GetAll(CancellationToken cancellationToken)
        {
            return await _postService.GetAllAsync(cancellationToken);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreatePost(CreatePost request, CancellationToken cancellationToken)
        //{
        //    var post = await _postService.CreateAsync(request);
        //    return Ok(post);
        //}
        ////UpdateAsync

        //[HttpPut("{id:int}")]
        //public async Task<IActionResult> UpdatePost(int id,UpdatePost request, CancellationToken cancellationToken)
        //{
        //    var post = await _postService.UpdateAsync(id,request,cancellationToken);
        //    return Ok(post);
        //}

        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> DeletePost(int id,CancellationToken cancellationToken)
        //{
        //    await _postService.DeleteAsync(id, cancellationToken);
        //    return NoContent();
        //}
    }
}
