using System.ComponentModel.DataAnnotations;

namespace Blog.DTOs.Auth;

public record RegisterDto(
    [Required]
    string FullName,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [MinLength(6)]
    string Password
);
