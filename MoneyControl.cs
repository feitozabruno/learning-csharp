using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

// JÁ TEMOS O SERVIDOR HTTP FUNCIONANDO
// AGORA VAMOS CRIAR A ROTA PARA RECEBER OS LANÇAMENTOS DE DADOS DO USUÁRIO

MoneyControl.Run();

public record MessageResponse(string Message);
public record ErrorNotFoundResponse(string Error, int StatusCode);

class MoneyControl
{
    public static void Run()
    {
        HttpListener server = new HttpListener();
        server.Prefixes.Add("http://localhost:5000/");
        server.Prefixes.Add("http://127.0.0.1:5000/");
        server.Start();
        Console.WriteLine("Servidor iniciado em http://localhost:5000");
        List<DataEntry> entries = new List<DataEntry>();

        while (true)
        {
            HttpListenerContext context = server.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            Console.WriteLine($"{request.HttpMethod} {request.Url}");

            string route = request.Url!.AbsolutePath;

            if (route == "/input" && request.HttpMethod == "POST")
            {
                string requestBody = ReadJson(request.InputStream);
                DataEntry parsedBody = JsonSerializer.Deserialize(requestBody, AppJsonContext.Default.DataEntry);

                DataEntry newEntry = new DataEntry
                {
                    Description = parsedBody.Description,
                    Value = parsedBody.Value,
                    Type = parsedBody.Type,
                };

                entries.Add(newEntry);

                // NÃO ESTOU GOSTANDO DO NOME DESSA CLASSE (DataEntry)
                // PRECISO ACHAR UM NOME MELHOR

                SendJson(
                    response,
                    new MessageResponse("Dado lançado com sucesso!"),
                    AppJsonContext.Default.MessageResponse
                );
            }
            else if (route == "/entries" && request.HttpMethod == "GET")
            {
                SendJson(
                    response,
                    entries,
                    AppJsonContext.Default.ListDataEntry
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
        string body = reader.ReadToEnd();
        return body;
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
[JsonSerializable(typeof(DataEntry))]
[JsonSerializable(typeof(List<DataEntry>))]
internal partial class AppJsonContext : JsonSerializerContext { }

class DataEntry
{
    private static int initialId = 1;
    public int Id { get; set; } = initialId++;
    public string Description { get; set; }
    public decimal Value { get; set; }
    public string Type { get; set; }
}
