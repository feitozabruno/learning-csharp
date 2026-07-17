using System.Net.Http.Json;
using Blog.DTOs.Auth;

namespace Blog.Tests.Fixtures;

public class AuthHelper(HttpClient client)
{
    private readonly HttpClient _client = client;

    private static string UniqueEmail() => $"test_{Guid.NewGuid()}@email.com";

    public async Task<CreatedUser> CreateUserAsync(
        string name = "John Doe",
        string email = "",
        string password = "JohnPassword")
    {
        if (string.IsNullOrEmpty(email)) email = UniqueEmail();
        var dto = new RegisterDto(name, email, password);
        await _client.PostAsJsonAsync("/api/auth/register", dto);
        return new CreatedUser(email, password);
    }

    public async Task<string> GetTokenAsync(string email, string password)
    {
        var dto = new LoginDto(email, password);
        var response = await _client.PostAsJsonAsync("/api/auth/login", dto);
        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return body!.Token;
    }

    public async Task<string> CreateUserAndGetTokenAsync()
    {
        var newUser = await CreateUserAsync();
        return await GetTokenAsync(newUser.Email, newUser.Password);
    }
}