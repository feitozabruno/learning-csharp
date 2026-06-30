public record TransactionPatchDto(
    string? Description,
    decimal? Value,
    string? Type,
    string? Category
);