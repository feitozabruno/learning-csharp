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
        List<Transaction> transactions = new List<Transaction>();

        while (true)
        {
            HttpListenerContext context = server.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            Console.WriteLine($"{request.HttpMethod} {request.Url}");

            string route = request.Url!.AbsolutePath;
            string[] segments = route.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (route == "/new-transaction" && request.HttpMethod == "POST")
            {
                try
                {
                    string requestBody = ReadJson(request.InputStream);
                    Transaction transaction = JsonSerializer.Deserialize(requestBody, AppJsonContext.Default.Transaction);
                    transactions.Add(transaction);
                    SaveTransactionsFile(transactions);
                    SendJson(
                        response,
                        new MessageResponse("Dado lançado com sucesso!"),
                        AppJsonContext.Default.MessageResponse
                    );
                }
                catch
                {
                    response.StatusCode = 400;
                    SendJson(
                        response,
                        new ErrorResponse("Corpo da requisição inválido", 400),
                        AppJsonContext.Default.ErrorResponse
                    );
                }
            }
            else if (
                route == "/transactions"
                && request.QueryString["category"] != null
                && request.HttpMethod == "GET"
            )
            {
                List<Transaction> categorized = transactions.FindAll(transaction => transaction.Category.ToLower() == request.QueryString["category"].ToLower());
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
                    transactions,
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
                Transaction? transactionFounded = transactions.Find(transaction => transaction.Id == id);

                if (transactionFounded is null)
                {
                    SendJson(
                        response,
                        new MessageResponse($"Nenhuma transacão com id: {id} foi encontrada."),
                        AppJsonContext.Default.MessageResponse
                    );
                }
                else
                {
                    SendJson(
                        response,
                        transactionFounded,
                        AppJsonContext.Default.Transaction
                    );
                }
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
                        new ErrorResponse("O ID do recurso não é válido.", 400),
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

                Transaction? transactionFounded = transactions.Find(transaction => transaction.Id == id);

                if (transactionFounded is null)
                {
                    SendJson(
                        response,
                        new MessageResponse($"Nenhuma transacão com id: {id} foi encontrada."),
                        AppJsonContext.Default.MessageResponse
                    );
                    continue;
                }

                if (parsedBody.Description is not null)
                {
                    transactionFounded.Description = parsedBody.Description;
                }

                if (parsedBody.Value is not null)
                {
                    transactionFounded.Value = parsedBody.Value.Value;
                }

                if (parsedBody.Type is not null)
                {
                    transactionFounded.Type = parsedBody.Type.Value;
                }

                if (parsedBody.Category is not null)
                {
                    transactionFounded.Category = parsedBody.Category;
                }

                if (
                    transactionFounded is not null
                    && parsedBody.Description is null
                    && parsedBody.Value is null
                    && parsedBody.Type is null
                    && parsedBody.Category is null
                )
                {
                    SendJson(
                        response,
                        new MessageResponse($"Nenhum dado para atualizar foi enviado."),
                        AppJsonContext.Default.MessageResponse
                    );
                    continue;
                }

                if (
                    transactionFounded is not null
                    && parsedBody.Description is not null
                    || parsedBody.Value is not null
                    || parsedBody.Type is not null
                    || parsedBody.Category is not null
                )
                {
                    SaveTransactionsFile(transactions);
                    SendJson(
                        response,
                        transactionFounded,
                        AppJsonContext.Default.Transaction
                    );
                    continue;
                }
            }
            else if (
                segments[0] == "transactions"
                && segments.Length == 2
                && request.HttpMethod == "DELETE"
            )
            {
                bool parsed = int.TryParse(segments[1], out int id);
                Transaction? transactionFounded = transactions.Find(transaction => transaction.Id == id);

                if (transactionFounded is null)
                {
                    SendJson(
                        response,
                        new MessageResponse($"Nenhuma transacão com id: {id} foi encontrada."),
                        AppJsonContext.Default.MessageResponse
                    );
                }
                else
                {
                    transactions.Remove(transactionFounded);
                    SaveTransactionsFile(transactions);
                    SendJson(
                        response,
                        new MessageResponse("Transação deletada com sucesso."),
                        AppJsonContext.Default.MessageResponse
                    );
                }
            }
            else if (route == "/balance" && request.HttpMethod == "GET")
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
                SendJson(
                    response,
                    new Summary(incomes, outcomes, balance),
                    AppJsonContext.Default.Summary
                );
            }
            else if (route == "/incomes" && request.HttpMethod == "GET")
            {
                List<Transaction> incomes = new List<Transaction>();

                foreach (Transaction transaction in transactions)
                {
                    if (transaction.Type == TransactionType.Income)
                    {
                        incomes.Add(transaction);
                    }
                }

                SendJson(
                    response,
                    incomes,
                    AppJsonContext.Default.ListTransaction
                );
            }
            else if (route == "/outcomes" && request.HttpMethod == "GET")
            {
                List<Transaction> incomes = new List<Transaction>();

                foreach (Transaction transaction in transactions)
                {
                    if (transaction.Type == TransactionType.Outcome)
                    {
                        incomes.Add(transaction);
                    }
                }

                SendJson(
                    response,
                    incomes,
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
    private static int initialId = 1;
    public int Id { get; set; } = initialId++;
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