using Money.Models;
using Money.Dtos;

namespace Money.Repositories.Interfaces;

public interface ITransactionRepository
{
    List<Transaction> GetAll();
    Transaction Create(TransactionCreateDto dto);
    Transaction? GetById(int id);
    bool Delete(int id);
    Transaction? UpdatePut(int id, TransactionUpdateDto dto);
    Transaction? UpdatePatch(int id, TransactionPatchDto dto);
}