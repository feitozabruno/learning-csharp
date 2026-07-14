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

    public async Task<IEnumerable<Post>> GetAllByUserIdAsync(string userId)
    {
        return await _context.Posts
            .Where(post => post.UserId == userId)
            .ToListAsync();
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        return await _context.Posts.FindAsync(id);
    }

    public async Task<Post?> GetByIdForUserAsync(int id, string userId)
    {
        return await _context.Posts
            .FirstOrDefaultAsync(post => post.Id == id && post.UserId == userId);
    }

    public async Task UpdateAsync(Post updatedPost)
    {
        _context.Posts.Update(updatedPost);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Post postRemoved)
    {
        _context.Posts.Remove(postRemoved);
        await _context.SaveChangesAsync();
    }
}