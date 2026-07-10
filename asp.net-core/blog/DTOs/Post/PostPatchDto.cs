namespace Blog.DTOs.Post;

public record PostPatchDto(
    string? Title,
    string? Content
);