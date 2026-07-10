using System.ComponentModel.DataAnnotations;

namespace Blog.DTOs.Post;

public record PostUpdateDto(
    [Required]
    string Title,

    [Required]
    string Content
);