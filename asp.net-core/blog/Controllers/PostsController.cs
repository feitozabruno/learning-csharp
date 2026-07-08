using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Repositories.Interfaces;
using Blog.DTOs.Post;
using Blog.Models;

namespace Blog.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController(IPostRepository postRepository) : ControllerBase
{
    private readonly IPostRepository _postRepository = postRepository;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostCreateDto dto)
    {
        Post newPost = await _postRepository.AddAsync(dto);
        return Created("", newPost);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        IEnumerable<Post> posts = await _postRepository.GetAllAsync();
        return Ok(posts);
    }
}