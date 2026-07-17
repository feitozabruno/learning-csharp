using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.DTOs.Auth;
using Blog.Exceptions;
using Blog.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Services;

public class AuthService(UserManager<AppUser> userManager, IConfiguration config) : IAuthService
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IConfiguration _config = config;

    public string GenerateToken(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
          new Claim(ClaimTypes.NameIdentifier, user.Id),
          new Claim(ClaimTypes.Email, user.Email!),
          new Claim(ClaimTypes.Name, user.FullName ?? user.Email!)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task CreateUserAsync(RegisterDto dto)
    {
        AppUser? userExists = await _userManager.FindByEmailAsync(dto.Email);
        if (userExists is not null) throw new ValidationException("Esse email já está em uso.");

        var newUser = new AppUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(newUser, dto.Password);

        if (!result.Succeeded)
        {
            var messages = string.Join(" | ", result.Errors.Select(e => e.Description));
            throw new ValidationException(messages);
        }
    }

    public async Task<string> LoginUserAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        var validPassword = user is not null && await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!validPassword) throw new ValidationException("Email ou senha inválidos.");
        return GenerateToken(user!);
    }
}