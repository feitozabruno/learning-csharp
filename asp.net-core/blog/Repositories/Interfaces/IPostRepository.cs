using Blog.Models;

namespace Blog.Repositories.Interfaces;

public interface IPostRepository
{
    Task AddAsync(Post post);
    Task<IEnumerable<Post>> GetAllAsync();
    Task<IEnumerable<Post>> GetAllByUserIdAsync(string userId);
    Task<Post?> GetByIdAsync(int id);
    Task<Post?> GetByIdForUserAsync(int id, string userId);
    Task UpdateAsync(Post updatedPost);
    Task DeleteAsync(Post postRemoved);
}