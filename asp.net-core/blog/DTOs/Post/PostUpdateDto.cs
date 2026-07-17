using System.ComponentModel.DataAnnotations;

namespace Blog.DTOs.Post;

public record PostUpdateDto(
    [Required(ErrorMessage = "O título é obrigatório.")]
    string Title,

    [Required(ErrorMessage = "O conteúdo é obrigatório.")]
    string Content
);