using Blog.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Blog.Services.Interfaces;

namespace Blog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        await _authService.CreateUserAsync(dto);
        return Ok("Usuário criado com sucesso.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        string token = await _authService.LoginUserAsync(dto);
        return Ok(new { token });
    }
}