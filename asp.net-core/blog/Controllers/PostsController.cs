using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Repositories.Interfaces;
using Blog.Services.Interfaces;
using Blog.DTOs.Post;
using Blog.Models;

namespace Blog.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUserService _currentUserService;

    public PostsController(IPostRepository postRepository, ICurrentUserService currentUserService)
    {
        _postRepository = postRepository;
        _currentUserService = currentUserService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostCreateDto dto)
    {
        var newPost = new Post
        {
            Title = dto.Title,
            Content = dto.Content,
            Author = _currentUserService.FullName,
            UserId = _currentUserService.UserId
        };

        await _postRepository.AddAsync(newPost);
        return Created("", newPost);
    }
}