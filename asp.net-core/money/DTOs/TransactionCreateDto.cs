using System.ComponentModel.DataAnnotations;

public record TransactionCreateDto(
    [Required(ErrorMessage = "Descrição é obrigatória.")]
    string Description,

    [Required(ErrorMessage = "Valor é obrigatório.")]
    [Range(0.01, 99999.99, ErrorMessage = "Valor deve estar entre 0.01 e 99999.99")]
    decimal Value,

    [Required(ErrorMessage = "Tipo é obrigatório.")]
    TransactionType Type,

    [Required(ErrorMessage = "Categoria é obrigatória.")]
    string Category
);