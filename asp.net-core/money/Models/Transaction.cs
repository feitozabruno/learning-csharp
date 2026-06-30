public class Transaction
{
    private static int InitialId = 1;
    public int Id { get; private set; } = InitialId++;
    public required string Description { get; set; }
    public required decimal Value { get; set; }
    public required TransactionType Type { get; set; }
    public required string Category { get; set; }
}

public enum TransactionType
{
    Income,
    Outcome
}