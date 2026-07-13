using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Services.Interfaces;
using Blog.DTOs.Post;
using Blog.Models;

namespace Blog.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController(IPostService postService) : ControllerBase
{
    private readonly IPostService _postService = postService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostCreateDto dto)
    {
        PostResponseDto newPost = await _postService.CreatePostAsync(dto);
        return Created("", newPost);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        IEnumerable<PostResponseDto> posts = await _postService.GetAllPostsAsync();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        PostResponseDto? postFound = await _postService.GetPostById(id);

        if (postFound is null)
        {
            return NotFound("Post não encontrado ou não pertence a você.");
        }

        return Ok(postFound);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, PostUpdateDto dto)
    {
        PostResponseDto? updatedPost = await _postService.UpdatePostAsync(id, dto);

        if (updatedPost is null)
        {
            return NotFound("Post não encontrado ou não pertence a você.");
        }

        return Ok(updatedPost);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch([FromRoute] int id, PostPatchDto dto)
    {
        if (dto.Title is null && dto.Content is null)
        {
            return BadRequest("Nenhum dado para atualizar foi enviado.");
        }

        PostResponseDto? updatedPost = await _postService.PatchPostAsync(id, dto);

        if (updatedPost is null)
        {
            return NotFound("Post não encontrado ou não percentence a você");
        }

        return Ok(updatedPost);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        bool removed = await _postService.DeletePostAsync(id);

        if (!removed)
        {
            return NotFound("Post não encontrado ou não pertence a você.");
        }

        return NoContent();
    }
}