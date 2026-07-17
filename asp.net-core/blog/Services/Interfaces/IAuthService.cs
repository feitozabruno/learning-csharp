using Blog.DTOs.Auth;
using Blog.Models;

namespace Blog.Services.Interfaces;

public interface IAuthService
{
    string GenerateToken(AppUser user);
    Task CreateUserAsync(RegisterDto dto);
    Task<string> LoginUserAsync(LoginDto dto);
}