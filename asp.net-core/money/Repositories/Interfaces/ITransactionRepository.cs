public interface ITransactionRepository
{
    List<Transaction> GetAll();
    Transaction Create(Transaction transaction);
    Transaction? GetById(int id);
    bool Delete(int id);
    Transaction? UpdatePut(int id, Transaction updatedTransaction);
    Transaction? UpdatePatch(int id, TransactionPatch patch);
}