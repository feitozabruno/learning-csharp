using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

MoneyControl.Run();

public record MessageResponse(string Message);
public record ErrorNotFoundResponse(string Error, int StatusCode);
public record Summary(decimal Incomes, decimal Outcomes, string Balance);

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

            if (route == "/new-transaction" && request.HttpMethod == "POST")
            {
                try
                {
                    string requestBody = ReadJson(request.InputStream);
                    Transaction transaction = JsonSerializer.Deserialize(requestBody, AppJsonContext.Default.Transaction);
                    transactions.Add(transaction);
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
                        new ErrorNotFoundResponse("Corpo da requisição inválido", 400),
                        AppJsonContext.Default.ErrorNotFoundResponse
                    );
                }
            }
            else if (route == "/transactions" && request.HttpMethod == "GET")
            {
                SendJson(
                    response,
                    transactions,
                    AppJsonContext.Default.ListTransaction
                );
            }
            else if (route == "/summary" && request.HttpMethod == "GET")
            {
                decimal incomes = 0m;
                decimal outcomes = 0m;
                decimal balance;
                foreach (Transaction transaction in transactions)
                {
                    if (transaction.Type == "Income")
                    {
                        incomes += transaction.Value;
                    }
                    if (transaction.Type == "Outcome")
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
                    if (transaction.Type == "Income")
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
                    if (transaction.Type == "Outcome")
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
                    new ErrorNotFoundResponse("Not Found", 404),
                    AppJsonContext.Default.ErrorNotFoundResponse
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

[JsonSerializable(typeof(MessageResponse))]
[JsonSerializable(typeof(ErrorNotFoundResponse))]
[JsonSerializable(typeof(Transaction))]
[JsonSerializable(typeof(List<Transaction>))]
[JsonSerializable(typeof(Summary))]
internal partial class AppJsonContext : JsonSerializerContext { }

class Transaction
{
    private static int initialId = 1;
    public int Id { get; set; } = initialId++;
    public required string Description { get; set; }
    public required decimal Value { get; set; }
    public required string Type { get; set; }
}
