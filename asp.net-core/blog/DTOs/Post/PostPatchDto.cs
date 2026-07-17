namespace Blog.DTOs.Post;

public record PostPatchDto(
    string? Title = null,
    string? Content = null
);