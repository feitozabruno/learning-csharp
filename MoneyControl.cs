using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

MoneyControl.Run();

public record MessageResponse(string Message);
public record ErrorResponse(string Error, int StatusCode);
public record Summary(decimal Incomes, decimal Outcomes, decimal Balance);

public class UpdateTransaction
{
    public string? Description { get; set; }
    public decimal? Value { get; set; }
    public TransactionType? Type { get; set; }
    public string? Category { get; set; }
}

class MoneyControl
{
    public static void Run()
    {
        HttpListener server = new HttpListener();
        server.Prefixes.Add("http://localhost:5000/");
        server.Prefixes.Add("http://127.0.0.1:5000/");
        server.Start();
        Console.WriteLine("Servidor iniciado em http://localhost:5000");

        Money money = new Money();

        while (true)
        {
            HttpListenerContext context = server.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            Console.WriteLine($"{request.HttpMethod} {request.Url}");

            string route = request.Url!.AbsolutePath;
            string[] segments = route.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (route == "/" && request.HttpMethod == "GET")
            {
                SendJson(
                    response,
                    new MessageResponse("Hello, World!"),
                    AppJsonContext.Default.MessageResponse
                );
            }
            else if (route == "/new-transaction" && request.HttpMethod == "POST")
            {
                try
                {
                    string requestBody = ReadJson(request.InputStream);
                    Transaction? transaction = JsonSerializer.Deserialize(requestBody, AppJsonContext.Default.Transaction);

                    if (transaction is null)
                    {
                        response.StatusCode = 400;
                        SendJson(
                            response,
                            new ErrorResponse("O corpo da requisição não é válido", 400),
                            AppJsonContext.Default.ErrorResponse
                        );
                        continue;
                    }

                    money.CreateTransaction(transaction);
                    SendJson(
                        response,
                        new MessageResponse("Dado lançado com sucesso!"),
                        AppJsonContext.Default.MessageResponse
                    );
                    continue;
                }
                catch
                {
                    response.StatusCode = 400;
                    SendJson(
                        response,
                        new ErrorResponse("Corpo da requisição inválido", 400),
                        AppJsonContext.Default.ErrorResponse
                    );
                    continue;
                }
            }
            else if (
                route == "/transactions"
                && request.QueryString["category"] != null
                && request.HttpMethod == "GET"
            )
            {
                string? queryString = request.QueryString["category"];

                if (queryString == null || queryString == "")
                {
                    response.StatusCode = 400;
                    SendJson(
                        response,
                        new ErrorResponse("O parâmetro enviado não é válido", 400),
                        AppJsonContext.Default.ErrorResponse
                    );
                    continue;
                }

                List<Transaction> categorized = money.CategorizedTransactions(queryString);
                SendJson(
                    response,
                    categorized,
                    AppJsonContext.Default.ListTransaction
                );
            }
            else if (route == "/transactions" && request.HttpMethod == "GET")
            {
                SendJson(
                    response,
                    money.ListTransactions(),
                    AppJsonContext.Default.ListTransaction
                );
            }
            else if (
                segments[0] == "transactions"
                && segments.Length == 2
                && request.HttpMethod == "GET"
            )
            {
                bool parsed = int.TryParse(segments[1], out int id);

                if (!parsed)
                {
                    response.StatusCode = 400;
                    SendJson(
                        response,
                        new ErrorResponse("O id do recurso informado não é válido", 400),
                        AppJsonContext.Default.ErrorResponse
                    );
                    continue;
                }

                Transaction? transaction = money.FindTransaction(id);

                if (transaction is null)
                {
                    response.StatusCode = 404;
                    SendJson(
                        response,
                        new ErrorResponse($"Nenhuma transacão com id: {id} foi encontrada.", 404),
                        AppJsonContext.Default.ErrorResponse
                    );
                    continue;
                }

                SendJson(
                    response,
                    transaction,
                    AppJsonContext.Default.Transaction
                );
                continue;
            }
            else if (
                segments[0] == "transactions"
                && segments.Length == 2
                && request.HttpMethod == "PATCH"
            )
            {
                bool parsed = int.TryParse(segments[1], out int id);

                if (!parsed)
                {
                    response.StatusCode = 400;
                    SendJson(
                        response,
                        new ErrorResponse("O id do recurso informado não é válido.", 400),
                        AppJsonContext.Default.ErrorResponse
                    );
                    continue;
                }

                string requestBody = ReadJson(request.InputStream);
                UpdateTransaction? parsedBody;

                try
                {
                    parsedBody = JsonSerializer.Deserialize(requestBody, AppJsonContext.Default.UpdateTransaction);
                }
                catch
                {
                    response.StatusCode = 400;
                    SendJson(
                        response,
                        new ErrorResponse("Corpo da requisição é inválido.", 400),
                        AppJsonContext.Default.ErrorResponse
                    );
                    continue;
                }

                if (parsedBody is null)
                {
                    response.StatusCode = 400;
                    SendJson(
                        response,
                        new ErrorResponse("Corpo da requisição é inválido.", 400),
                        AppJsonContext.Default.ErrorResponse
                    );
                    continue;
                }

                bool hasUpdates = parsedBody.Description is not null
                                  || parsedBody.Value is not null
                                  || parsedBody.Type is not null
                                  || parsedBody.Category is not null;

                if (!hasUpdates)
                {
                    response.StatusCode = 400;
                    SendJson(
                        response,
                        new ErrorResponse($"Nenhum dado para atualizar foi enviado.", 400),
                        AppJsonContext.Default.ErrorResponse
                    );
                    continue;
                }

                Transaction? transaction = money.UpdateTransaction(id, parsedBody);

                if (transaction is null)
                {
                    response.StatusCode = 404;
                    SendJson(
                        response,
                        new ErrorResponse($"Nenhuma transacão com id: {id} foi encontrada.", 404),
                        AppJsonContext.Default.ErrorResponse
                    );
                    continue;
                }

                SendJson(
                    response,
                    transaction,
                    AppJsonContext.Default.Transaction
                );
                continue;
            }
            else if (
                segments[0] == "transactions"
                && segments.Length == 2
                && request.HttpMethod == "DELETE"
            )
            {
                bool parsed = int.TryParse(segments[1], out int id);

                if (!parsed)
                {
                    response.StatusCode = 400;
                    SendJson(
                        response,
                        new ErrorResponse("O id do recurso informado não é válido", 400),
                        AppJsonContext.Default.ErrorResponse
                    );
                    continue;
                }

                bool transactionRemoved = money.RemoveTransaction(id);

                if (!transactionRemoved)
                {
                    response.StatusCode = 404;
                    SendJson(
                        response,
                        new ErrorResponse($"Nenhuma transacão com id: {id} foi encontrada.", 404),
                        AppJsonContext.Default.ErrorResponse
                    );
                    continue;
                }

                SendJson(
                    response,
                    new MessageResponse("Transação deletada com sucesso."),
                    AppJsonContext.Default.MessageResponse
                );
                continue;
            }
            else if (route == "/balance" && request.HttpMethod == "GET")
            {
                Summary summary = money.GetBalance();
                SendJson(
                    response,
                    summary,
                    AppJsonContext.Default.Summary
                );
            }
            else if (route == "/incomes" && request.HttpMethod == "GET")
            {
                SendJson(
                    response,
                    money.FindTransactionType(TransactionType.Income),
                    AppJsonContext.Default.ListTransaction
                );
            }
            else if (route == "/outcomes" && request.HttpMethod == "GET")
            {
                SendJson(
                    response,
                    money.FindTransactionType(TransactionType.Outcome),
                    AppJsonContext.Default.ListTransaction
                );
            }
            else
            {
                response.StatusCode = 404;
                SendJson(
                    response,
                    new ErrorResponse("Not Found", 404),
                    AppJsonContext.Default.ErrorResponse
                );
            }
        }
    }

    private static string ReadJson(Stream inputStream)
    {
        StreamReader reader = new StreamReader(inputStream);
        return reader.ReadToEnd();
    }

    private static void SendJson<T>(HttpListenerResponse response, T data, JsonTypeInfo<T> typeInfo)
    {
        string json = JsonSerializer.Serialize(data, typeInfo);
        byte[] buffer = Encoding.UTF8.GetBytes(json);
        response.ContentType = "application/json";
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer);
        response.Close();
    }
}

class Money
{
    private readonly List<Transaction> transactions = new List<Transaction>();

    public Transaction CreateTransaction(Transaction transaction)
    {
        transactions.Add(transaction);
        SaveTransactionsFile(transactions);
        return transaction;
    }

    public List<Transaction> ListTransactions()
    {
        return transactions;
    }

    public Transaction? FindTransaction(int id)
    {
        return transactions.Find(transaction => transaction.Id == id);
    }

    public Transaction? UpdateTransaction(int id, UpdateTransaction? updateTransaction)
    {
        Transaction? transaction = FindTransaction(id);

        if (transaction is null)
        {
            return null;
        }

        if (updateTransaction?.Description is not null)
        {
            transaction.Description = updateTransaction.Description;
        }

        if (updateTransaction?.Value is not null)
        {
            transaction.Value = updateTransaction.Value.Value;
        }

        if (updateTransaction?.Type is not null)
        {
            transaction.Type = updateTransaction.Type.Value;
        }

        if (updateTransaction?.Category is not null)
        {
            transaction.Category = updateTransaction.Category;
        }

        SaveTransactionsFile(transactions);
        return transaction;
    }

    public bool RemoveTransaction(int id)
    {
        Transaction? transaction = FindTransaction(id);

        if (transaction is not null)
        {
            transactions.Remove(transaction);
            SaveTransactionsFile(transactions);
            return true;
        }

        return false;
    }

    public List<Transaction> CategorizedTransactions(string category)
    {
        List<Transaction> categorized = new List<Transaction>();

        foreach (Transaction transaction in transactions)
        {
            if (transaction.Category.Equals(category, StringComparison.CurrentCultureIgnoreCase))
            {
                categorized.Add(transaction);
            }
        }

        return categorized;
    }

    public Summary GetBalance()
    {
        decimal incomes = 0m;
        decimal outcomes = 0m;
        decimal balance;

        foreach (Transaction transaction in transactions)
        {
            if (transaction.Type == TransactionType.Income)
            {
                incomes += transaction.Value;
            }
            if (transaction.Type == TransactionType.Outcome)
            {
                outcomes += transaction.Value;
            }
        }
        balance = incomes - outcomes;
        return new Summary(incomes, outcomes, balance);
    }

    public List<Transaction> FindTransactionType(TransactionType type)
    {
        return transactions.FindAll(transaction => transaction.Type == type);
    }

    private static void SaveTransactionsFile(List<Transaction> transactions)
    {
        string json = JsonSerializer.Serialize(transactions, AppJsonContext.Default.ListTransaction);
        File.WriteAllText("transactions.json", json);
    }
}

[JsonSerializable(typeof(MessageResponse))]
[JsonSerializable(typeof(ErrorResponse))]
[JsonSerializable(typeof(Transaction))]
[JsonSerializable(typeof(List<Transaction>))]
[JsonSerializable(typeof(Summary))]
[JsonSerializable(typeof(UpdateTransaction))]
internal partial class AppJsonContext : JsonSerializerContext { }

class Transaction
{
    private static int InitialId = 1;
    public int Id { get; private set; } = InitialId++;
    public required string Description { get; set; }
    public required decimal Value { get; set; }
    public required TransactionType Type { get; set; }
    public required string Category { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<TransactionType>))]
public enum TransactionType
{
    Income,
    Outcome
}