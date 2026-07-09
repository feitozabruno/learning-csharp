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
            Title = newPost.Title,
            Content = newPost.Content,
            Author = newPost.Author,
            CreatedAt = newPost.CreatedAt
        };
    }

    public async Task<IEnumerable<PostResponseDto>> GetAllPostsAsync()
    {
        IEnumerable<Post> posts = await _postRepository.GetAllAsync();

        return posts
            .Where(post => post.UserId == _currentUserService.UserId)
            .Select(post => new PostResponseDto
            {
                Title = post.Title,
                Content = post.Content,
                Author = post.Author,
                CreatedAt = post.CreatedAt
            });
    }

    public async Task<PostResponseDto?> GetPostById(int id)
    {
        Post? post = await _postRepository.GetByIdAsync(id);
        if (post is null) return null;

        return new PostResponseDto
        {
            Title = post.Title,
            Content = post.Content,
            Author = post.Author,
            CreatedAt = post.CreatedAt
        };
    }
}