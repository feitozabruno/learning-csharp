using System.ComponentModel.DataAnnotations;

namespace Blog.DTOs;

public record LoginDto(
    [Required]
    [EmailAddress]
    string Email,

    [Required]
    string Password
);