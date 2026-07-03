using Money.Models;
using Money.Dtos;

namespace Money.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetAllAsync();
    Task<Transaction> Create(TransactionCreateDto dto);
    Task<Transaction?> GetByIdAsync(int id);
    Task<bool> Delete(int id);
    Task<Transaction?> UpdatePut(int id, TransactionUpdateDto dto);
    Task<Transaction?> UpdatePatch(int id, TransactionPatchDto dto);
}