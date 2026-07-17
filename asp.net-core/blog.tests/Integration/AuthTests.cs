using System.Net.Http.Json;
using Blog.DTOs.Auth;
using Blog.Tests.Fixtures;
using FluentAssertions;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Tests.Integration;

public class AuthTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly AuthHelper _auth;

    public AuthTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _auth = new AuthHelper(_client);
    }

    [Fact]
    public async Task Register_WithValidData_ReturnOk()
    {
        // Arrange - prepara os dados
        var dto = new RegisterDto(
            "John Doe",
            $"test_{Guid.NewGuid()}@email.com",
            "JohnPassword"
        );

        // Act - faz a requisição
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);
        var body = await response.Content.ReadAsStringAsync();

        // Assert - verifica o resultado
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().Be("Usuário criado com sucesso.");
    }

    [Fact]
    public async Task Register_WithDuplicatedEmail_ReturnBadRequest()
    {
        var email = $"test_{Guid.NewGuid()}@email.com";
        var dto = new RegisterDto("John Doe", email, "JohnPassword");

        await _client.PostAsJsonAsync("/api/auth/register", dto);
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);
        var body = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        body!.Title.Should().Be("Erro de validação");
        body!.Status.Should().Be(400);
        body!.Detail.Should().Be("Esse email já está em uso.");
        body!.Instance.Should().Be("/api/auth/register");
    }

    [Fact]
    public async Task Register_WithWeakPassword_ReturnBadRequest()
    {
        var dto = new RegisterDto("John Doe", $"test_{Guid.NewGuid()}@email.com", "Senha");

        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);
        var body = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        body!.Title.Should().Be("Erro de validação");
        body!.Status.Should().Be(400);
        body!.Detail.Should().Be("Passwords must be at least 6 characters.");
        body!.Instance.Should().Be("/api/auth/register");
    }

    [Fact]
    public async Task Login_WithValidData_ReturnToken()
    {
        var user = await _auth.CreateUserAsync();
        var dto = new LoginDto(user.Email, user.Password);

        var response = await _client.PostAsJsonAsync("/api/auth/login", dto);
        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body!.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnBadRequest()
    {
        var user = await _auth.CreateUserAsync();
        var dto = new LoginDto(user.Email, "Password");

        var response = await _client.PostAsJsonAsync("/api/auth/login", dto);
        var body = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        body!.Title.Should().Be("Erro de validação");
        body!.Status.Should().Be(400);
        body!.Detail.Should().Be("Email ou senha inválidos.");
        body!.Instance.Should().Be("/api/auth/login");
    }

    [Fact]
    public async Task Login_WithInvalidEmail_ReturnBadRequest()
    {
        var dto = new LoginDto("invalid@email.com", "password");

        var response = await _client.PostAsJsonAsync("/api/auth/login", dto);
        var body = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        body!.Title.Should().Be("Erro de validação");
        body!.Status.Should().Be(400);
        body!.Detail.Should().Be("Email ou senha inválidos.");
        body!.Instance.Should().Be("/api/auth/login");
    }
}