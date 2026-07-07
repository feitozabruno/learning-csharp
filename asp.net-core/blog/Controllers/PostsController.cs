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
    private readonly IPostRepository _repo;
    private readonly ICurrentUserService _currentUser;

    public PostsController(IPostRepository repo, ICurrentUserService currentUser)
    {
        _repo = repo;
        _currentUser = currentUser;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostCreateDto dto)
    {
        var newPost = new Post
        {
            Title = dto.Title,
            Content = dto.Content,
            Author = _currentUser.FullName,
            UserId = _currentUser.UserId
        };

        await _repo.AddAsync(newPost);
        return Created("", newPost);
    }
}