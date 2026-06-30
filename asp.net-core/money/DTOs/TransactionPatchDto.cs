public record TransactionPatchDto(
    string? Description,
    decimal? Value,
    TransactionType? Type,
    string? Category
);