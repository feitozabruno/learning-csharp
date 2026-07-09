using Blog.DTOs.Post;

namespace Blog.Services.Interfaces;

public interface IPostService
{
    Task<PostResponseDto> CreatePostAsync(PostCreateDto dto);
    Task<IEnumerable<PostResponseDto>> GetAllPostsAsync();
    Task<PostResponseDto?> GetPostById(int id);
}