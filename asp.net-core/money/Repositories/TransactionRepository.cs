class TransactionRepository : ITransactionRepository
{
    private static readonly List<Transaction> transactions = new List<Transaction>();

    public List<Transaction> GetAll()
    {
        return transactions;
    }

    public Transaction Create(Transaction newTransaction)
    {
        transactions.Add(newTransaction);
        return newTransaction;
    }

    public Transaction? GetById(int id)
    {
        Transaction? transactionFound = transactions.Find(transaction => transaction.Id == id);
        return transactionFound;
    }

    public bool Delete(int id)
    {
        Transaction? transactionFound = GetById(id);
        if (transactionFound is null) return false;
        return transactions.Remove(transactionFound);
    }

    public Transaction? UpdatePut(int id, Transaction updatedTransaction)
    {
        Transaction? transactionFound = GetById(id);
        transactionFound?.Description = updatedTransaction.Description;
        transactionFound?.Value = updatedTransaction.Value;
        transactionFound?.Type = updatedTransaction.Type;
        transactionFound?.Category = updatedTransaction.Category;
        return transactionFound;
    }

    public Transaction? UpdatePatch(int id, TransactionPatch patch)
    {
        Transaction? transactionFound = GetById(id);
        if (transactionFound is null) return null;

        if (
            patch.Description is null
            && patch.Value is null
            && patch.Type is null
            && patch.Category is null
        )
        {
            return transactionFound;
        }

        if (patch.Description is not null)
        {
            transactionFound.Description = patch.Description;
        }

        if (patch.Value is not null)
        {
            transactionFound.Value = patch.Value.Value;
        }

        if (patch.Type is not null)
        {
            transactionFound.Type = patch.Type;
        }

        if (patch.Category is not null)
        {
            transactionFound.Category = patch.Category;
        }

        return transactionFound;
    }
}