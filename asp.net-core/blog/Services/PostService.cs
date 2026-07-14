using Blog.DTOs.Post;
using Blog.Exceptions;
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
        return PostResponseDto.From(newPost);
    }

    public async Task<IEnumerable<PostResponseDto>> GetAllUserPostsAsync()
    {
        IEnumerable<Post> posts = await _postRepository.GetAllByUserIdAsync(_currentUserService.UserId);

        return posts
            .Where(post => post.UserId == _currentUserService.UserId)
            .Select(post => PostResponseDto.From(post));
    }

    public async Task<PostResponseDto> GetUserPostByIdAsync(int id)
    {
        Post? post = await _postRepository
            .GetByIdForUserAsync(id, _currentUserService.UserId)
            ?? throw new NotFoundException("Post", id);

        return PostResponseDto.From(post);
    }

    public async Task<PostResponseDto> UpdatePostAsync(int id, PostUpdateDto dto)
    {
        Post? post = await _postRepository
            .GetByIdForUserAsync(id, _currentUserService.UserId)
            ?? throw new NotFoundException("Post", id);

        post.Title = dto.Title;
        post.Content = dto.Content;
        post.UpdatedAt = DateTime.UtcNow;

        await _postRepository.UpdateAsync(post);
        return PostResponseDto.From(post);
    }

    public async Task<PostResponseDto> PatchPostAsync(int id, PostPatchDto dto)
    {
        if (dto.Title is null && dto.Content is null)
        {
            throw new ValidationException("Nenhum dado para atualizar foi enviado.");
        }

        Post? post = await _postRepository
            .GetByIdForUserAsync(id, _currentUserService.UserId)
            ?? throw new NotFoundException("Post", id);

        if (dto.Title is not null) post.Title = dto.Title;
        if (dto.Content is not null) post.Content = dto.Content;
        post.UpdatedAt = DateTime.Now;

        await _postRepository.UpdateAsync(post);
        return PostResponseDto.From(post);
    }

    public async Task DeletePostAsync(int id)
    {
        Post? post = await _postRepository
            .GetByIdForUserAsync(id, _currentUserService.UserId)
            ?? throw new NotFoundException("Post", id);

        await _postRepository.DeleteAsync(post);
    }
}