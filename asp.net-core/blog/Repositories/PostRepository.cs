using Blog.Repositories.Interfaces;
using Blog.Models;
using Blog.Data;

namespace Blog.Repositories;

public class PostRepository(AppDbContext context) : IPostRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Post> AddAsync(Post newPost)
    {
        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();
        return newPost;
    }
}