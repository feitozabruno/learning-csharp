using Blog.Repositories.Interfaces;
using Blog.Models;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories;

public class PostRepository(AppDbContext context) : IPostRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        return await _context.Posts.FindAsync(id);
    }
}