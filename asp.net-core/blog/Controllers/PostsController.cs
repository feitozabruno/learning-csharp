using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Services.Interfaces;
using Blog.DTOs.Post;

namespace Blog.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController(IPostService postService) : ControllerBase
{
    private readonly IPostService _postService = postService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostCreateDto dto)
    {
        PostResponseDto newPost = await _postService.CreatePostAsync(dto);
        return Created("", newPost);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        IEnumerable<PostResponseDto> posts = await _postService.GetAllPostsAsync();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        PostResponseDto? postFound = await _postService.GetPostById(id);
        return postFound is not null ? Ok(postFound) : NotFound();
    }
}