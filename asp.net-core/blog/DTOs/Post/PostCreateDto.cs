using System.ComponentModel.DataAnnotations;

namespace Blog.DTOs.Post;

public record PostCreateDto(
    [Required]
    string Title,

    [Required]
    string Content
);