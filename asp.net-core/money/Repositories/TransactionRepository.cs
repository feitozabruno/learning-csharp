using Money.Models;
using Money.Dtos;
using Money.Repositories.Interfaces;
using Money.Data;
using Microsoft.EntityFrameworkCore;

namespace Money.Repositories;

class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _context.Transactions.ToListAsync();
    }

    public async Task<Transaction> Create(TransactionCreateDto dto)
    {
        Transaction newTransaction = new Transaction
        {
            Description = dto.Description,
            Value = dto.Value,
            Type = dto.Type,
            Category = dto.Category
        };
        await _context.Transactions.AddAsync(newTransaction);
        await _context.SaveChangesAsync();
        return newTransaction;
    }

    public async Task<Transaction?> GetByIdAsync(int id)
    {
        return await _context.Transactions.FindAsync(id);
    }

    public async Task<bool> Delete(int id)
    {
        Transaction? transactionFound = await GetByIdAsync(id);
        if (transactionFound is null) return false;
        _context.Transactions.Remove(transactionFound);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Transaction?> UpdatePut(int id, TransactionUpdateDto dto)
    {
        Transaction? transactionFound = await GetByIdAsync(id);
        if (transactionFound is null) return null;

        transactionFound.Description = dto.Description;
        transactionFound.Value = dto.Value;
        transactionFound.Type = dto.Type;
        transactionFound.Category = dto.Category;

        await _context.SaveChangesAsync();
        return transactionFound;
    }

    public async Task<Transaction?> UpdatePatch(int id, TransactionPatchDto dto)
    {
        Transaction? transactionFound = await GetByIdAsync(id);
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
        await _context.SaveChangesAsync();
        return transactionFound;
    }
}