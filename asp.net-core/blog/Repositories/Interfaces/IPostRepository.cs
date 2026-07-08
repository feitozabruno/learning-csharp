using Blog.Models;
using Blog.DTOs.Post;

namespace Blog.Repositories.Interfaces;

public interface IPostRepository
{
    Task<Post> AddAsync(PostCreateDto dto);
    Task<IEnumerable<Post>> GetAllAsync();
}