using Money.Models;
using Money.Dtos;
using Money.Repositories.Interfaces;

namespace Money.Repositories;

class TransactionRepository : ITransactionRepository
{
    private static readonly List<Transaction> transactions = new List<Transaction>();

    public List<Transaction> GetAll()
    {
        return transactions;
    }

    public Transaction Create(TransactionCreateDto dto)
    {
        Transaction newTransaction = new Transaction
        {
            Description = dto.Description,
            Value = dto.Value,
            Type = dto.Type,
            Category = dto.Category
        };

        transactions.Add(newTransaction);
        return newTransaction;
    }

    public Transaction? GetById(int id)
    {
        return transactions.Find(transaction => transaction.Id == id);
    }

    public bool Delete(int id)
    {
        Transaction? transactionFound = GetById(id);
        if (transactionFound is null) return false;
        return transactions.Remove(transactionFound);
    }

    public Transaction? UpdatePut(int id, TransactionUpdateDto dto)
    {
        Transaction? transactionFound = GetById(id);
        if (transactionFound is null) return null;

        transactionFound.Description = dto.Description;
        transactionFound.Value = dto.Value;
        transactionFound.Type = dto.Type;
        transactionFound.Category = dto.Category;

        return transactionFound;
    }

    public Transaction? UpdatePatch(int id, TransactionPatchDto dto)
    {
        Transaction? transactionFound = GetById(id);
        if (transactionFound is null) return null;

        if (
            dto.Description is null
            && dto.Value is null
            && dto.Type is null
            && dto.Category is null
        )
        {
            return transactionFound;
        }

        if (dto.Description is not null)
        {
            transactionFound.Description = dto.Description;
        }

        if (dto.Value is not null)
        {
            transactionFound.Value = dto.Value.Value;
        }

        if (dto.Type is not null)
        {
            transactionFound.Type = (TransactionType)dto.Type;
        }

        if (dto.Category is not null)
        {
            transactionFound.Category = dto.Category;
        }

        return transactionFound;
    }
}