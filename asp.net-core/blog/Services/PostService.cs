using Blog.DTOs.Post;
using Blog.Models;
using Blog.Repositories.Interfaces;
using Blog.Services.Interfaces;

namespace Blog.Services;

public class PostService(ICurrentUserService currentUserService, IPostRepository postRepository) : IPostService
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IPostRepository _postRepository = postRepository;

    public async Task<PostResponseDto> CreatePostAsync(PostCreateDto dto)
    {
        Post newPost = new Post
        {
            Title = dto.Title,
            Content = dto.Content,
            Author = _currentUserService.FullName,
            UserId = _currentUserService.UserId
        };

        await _postRepository.AddAsync(newPost);

        return new PostResponseDto
        {
            Id = newPost.Id,
            Title = newPost.Title,
            Content = newPost.Content,
            Author = newPost.Author,
            CreatedAt = newPost.CreatedAt,
            UpdatedAt = newPost.UpdatedAt
        };
    }

    public async Task<IEnumerable<PostResponseDto>> GetAllPostsAsync()
    {
        IEnumerable<Post> posts = await _postRepository.GetAllAsync();

        return posts
            .Where(post => post.UserId == _currentUserService.UserId)
            .Select(post => new PostResponseDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Author = post.Author,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            });
    }

    public async Task<PostResponseDto?> GetPostById(int id)
    {
        Post? post = await _postRepository.GetByIdAsync(id);
        if (post is null) return null;

        return new PostResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Author = post.Author,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt
        };
    }

    public async Task<PostResponseDto?> UpdatePostAsync(int id, PostUpdateDto dto)
    {
        Post? post = await _postRepository.GetByIdAsync(id);
        if (post is null) return null;

        post.Title = dto.Title;
        post.Content = dto.Content;
        post.UpdatedAt = DateTime.UtcNow;

        await _postRepository.UpdateAsync(post);

        return new PostResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Author = post.Author,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt
        };
    }
}