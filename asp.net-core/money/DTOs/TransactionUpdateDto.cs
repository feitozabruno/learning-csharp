public record TransactionPatch(
    string? Description,
    decimal? Value,
    string? Type,
    string? Category
);