using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blog.DTOs.Post;

namespace Blog.Tests.Fixtures;

public class PostHelper(HttpClient client)
{
    private readonly HttpClient _client = client;

    public async Task<PostResponseDto> CreateAsync(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var dto = new PostCreateDto($"Título-{Guid.NewGuid()}", $"Conteúdo-{Guid.NewGuid()}");
        var response = await _client.PostAsJsonAsync("/api/posts", dto);
        var body = await response.Content.ReadFromJsonAsync<PostResponseDto>();

        return body!;
    }
}