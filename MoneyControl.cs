using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

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

        while (true)
        {
            HttpListenerContext context = server.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            Console.WriteLine($"{request.HttpMethod} {request.Url}");

            string route = request.Url!.AbsolutePath;

            if (route == "/")
            {
                SendJson(
                    response,
                    new MessageResponse("Hello, World!"),
                    AppJsonContext.Default.MessageResponse
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
internal partial class AppJsonContext : JsonSerializerContext { }
