using System.Security.Claims;
using Blog.Services.Interfaces;

namespace Blog.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("Usuário não autenticado.");

    public string Email =>
            _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)
            ?? throw new InvalidOperationException("Email não encontrado no token.");

    public string FullName =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)
        ?? throw new InvalidOperationException("Nome não encontrado no token.");
}