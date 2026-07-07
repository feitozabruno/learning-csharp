using Blog.Models;

namespace Blog.Repositories.Interfaces;

public interface IPostRepository
{
    Task<Post> AddAsync(Post newPost);
}