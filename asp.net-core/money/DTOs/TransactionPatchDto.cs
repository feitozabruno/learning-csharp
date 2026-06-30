using Money.Models;

namespace Money.Dtos;

public record TransactionPatchDto(
    string? Description,
    decimal? Value,
    TransactionType? Type,
    string? Category
);