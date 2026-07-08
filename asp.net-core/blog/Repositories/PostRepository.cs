using Blog.Repositories.Interfaces;
using Blog.Models;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using Blog.Services.Interfaces;
using Blog.DTOs.Post;

namespace Blog.Repositories;

public class PostRepository(AppDbContext context, ICurrentUserService currentUserService) : IPostRepository
{
    private readonly AppDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<Post> AddAsync(PostCreateDto dto)
    {
        Post newPost = new Post
        {
            Title = dto.Title,
            Content = dto.Content,
            Author = _currentUserService.FullName,
            UserId = _currentUserService.UserId
        };

        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();
        return newPost;
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await _context.Posts
            .Where(post => post.UserId == _currentUserService.UserId)
            .ToListAsync();
    }
}