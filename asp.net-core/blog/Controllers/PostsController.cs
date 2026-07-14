using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Services.Interfaces;
using Blog.DTOs.Post;

namespace Blog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(IPostService postService) : ControllerBase
{
    private readonly IPostService _postService = postService;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] PostCreateDto dto)
    {
        PostResponseDto newPost = await _postService.CreatePostAsync(dto);
        return Created("", newPost);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetAllUserPosts()
    {
        IEnumerable<PostResponseDto> posts = await _postService.GetAllUserPostsAsync();
        return Ok(posts);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<PostResponseDto> posts = await _postService.GetAllPostsAsync();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        PostResponseDto postFound = await _postService.GetPostByIdAsync(id);
        return Ok(postFound);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] int id, PostUpdateDto dto)
    {
        PostResponseDto updatedPost = await _postService.UpdatePostAsync(id, dto);
        return Ok(updatedPost);
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> Patch([FromRoute] int id, PostPatchDto dto)
    {
        PostResponseDto updatedPost = await _postService.PatchPostAsync(id, dto);
        return Ok(updatedPost);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _postService.DeletePostAsync(id);
        return NoContent();
    }
}