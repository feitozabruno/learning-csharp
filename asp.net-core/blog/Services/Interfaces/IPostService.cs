using Blog.DTOs.Post;

namespace Blog.Services.Interfaces;

public interface IPostService
{
    Task<PostResponseDto> CreatePostAsync(PostCreateDto dto);
    Task<IEnumerable<PostResponseDto>> GetAllUserPostsAsync();
    Task<PostResponseDto> GetPostByIdAsync(int id);
    Task<IEnumerable<PostResponseDto>> GetAllPostsAsync();
    Task<PostResponseDto> UpdatePostAsync(int id, PostUpdateDto dto);
    Task<PostResponseDto> PatchPostAsync(int id, PostPatchDto dto);
    Task DeletePostAsync(int id);
}