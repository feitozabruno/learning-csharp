using Blog.Models;

namespace Blog.Repositories.Interfaces;

public interface IPostRepository
{
    Task AddAsync(Post post);
    Task<IEnumerable<Post>> GetAllAsync();
    Task<Post?> GetByIdAsync(int id);
    Task UpdateAsync(Post updatedPost);
    Task DeleteAsync(Post postRemoved);
}