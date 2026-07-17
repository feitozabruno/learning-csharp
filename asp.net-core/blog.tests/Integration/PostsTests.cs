using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blog.DTOs.Post;
using Blog.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Blog.Tests.Integration;

public class PostsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly AuthHelper _auth;
    private readonly PostHelper _post;

    public PostsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _auth = new AuthHelper(_client);
        _post = new PostHelper(_client);
    }

    [Fact]
    public async Task CreatePost_WithValidData_ReturnOk()
    {
        string token = await _auth.CreateUserAndGetTokenAsync();
        var dto = new PostCreateDto("Title First Post", "Content first post");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJsonAsync("/api/posts", dto);
        var body = await response.Content.ReadFromJsonAsync<PostResponseDto>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        body!.Title.Should().Be(dto.Title);
        body!.Content.Should().Be(dto.Content);
    }

    [Fact]
    public async Task CreatePost_WithInvalidData_ReturnBadRequest()
    {
        string token = await _auth.CreateUserAndGetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJsonAsync("/api/posts", new { Title = "Título" });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetPostById_ReturnPost()
    {
        string token = await _auth.CreateUserAndGetTokenAsync();
        PostResponseDto createdPost = await _post.CreateAsync(token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var post = await _client.GetFromJsonAsync<PostResponseDto>($"/api/posts/{createdPost.Id}");

        post!.Title.Should().Be(createdPost.Title);
        post!.Content.Should().Be(createdPost.Content);
    }

    [Fact]
    public async Task UpdatePost_WithValidData_ReturnOk()
    {
        string token = await _auth.CreateUserAndGetTokenAsync();
        PostResponseDto createdPost = await _post.CreateAsync(token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var postUpdated = new PostUpdateDto("Título123", "Conteúdo321");

        var response = await _client.PutAsJsonAsync($"/api/posts/{createdPost.Id}", postUpdated);
        var body = await response.Content.ReadFromJsonAsync<PostResponseDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Title.Should().Be(postUpdated.Title);
        body!.Content.Should().Be(postUpdated.Content);
    }

    [Fact]
    public async Task PatchPost_WithValidData_ReturnOk()
    {
        string token = await _auth.CreateUserAndGetTokenAsync();
        PostResponseDto createdPost = await _post.CreateAsync(token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var postUpdated = new PostPatchDto(Content: "Conteúdo alterado via patch.");

        var response = await _client.PatchAsJsonAsync($"/api/posts/{createdPost.Id}", postUpdated);
        var body = await response.Content.ReadFromJsonAsync<PostResponseDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Title.Should().Be(createdPost.Title);
        body!.Content.Should().Be(postUpdated.Content);
    }

    [Fact]
    public async Task DeletePost_ReturnNoContent()
    {
        string token = await _auth.CreateUserAndGetTokenAsync();
        PostResponseDto createdPost = await _post.CreateAsync(token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync($"/api/posts/{createdPost.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}