using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Task> tasks = new List<Task>();

app.MapGet("/", () => Results.Ok("Hello, World!"));

app.MapGet("/tasks", () => Results.Ok(tasks));

app.MapPost("/tasks", (Task task) =>
{
    tasks.Add(task);
    return Results.Created($"/tasks/{task.Id}", task);
});

app.MapGet("/tasks/{id}", (int id) =>
{
    Task? taskFound = FindTask(id);
    if (taskFound is null) return Results.NotFound(TaskError.NotFound);
    return Results.Ok(taskFound);
});

app.MapPut("/tasks/{id}", (int id, Task task) =>
{
    Task? taskFound = FindTask(id);
    if (taskFound is null) return Results.NotFound(TaskError.NotFound);
    taskFound.Title = task.Title;
    taskFound.Done = task.Done;
    return Results.Ok(taskFound);
});

app.MapPatch("/tasks/{id}", (int id, TaskPatch patch) =>
{
    Task? taskFound = FindTask(id);
    if (taskFound is null) return Results.NotFound(TaskError.NotFound);

    if (patch.Title is null && patch.Done is null)
    {
        return Results.BadRequest(TaskError.BadRequest);
    }

    if (patch.Title is not null) taskFound.Title = patch.Title;
    if (patch.Done is not null) taskFound.Done = patch.Done.Value;
    return Results.Ok(taskFound);
});

app.MapDelete("/tasks/{id}", (int id) =>
{
    Task? taskFound = FindTask(id);
    if (taskFound is null) return Results.NotFound(TaskError.NotFound);
    tasks.Remove(taskFound);
    return Results.NoContent();
});

app.Run();

Task? FindTask(int id) => tasks.Find(task => task.Id == id);

record TaskPatch(string? Title, bool? Done);

record TaskError
{
    public required string Title { get; init; }
    public required string Detail { get; init; }
    public required string Action { get; init; }
    public required int StatusCode { get; init; }

    public static readonly TaskError NotFound = new()
    {
        Title = "Not Found Error",
        Detail = "Tarefa não encontrada.",
        Action = "Verifique o id da tarefa.",
        StatusCode = StatusCodes.Status404NotFound
    };

    public static readonly TaskError BadRequest = new()
    {
        Title = "Bad Request Error",
        Detail = "Nenhum dado para alteração foi informado.",
        Action = "Informe ao menos um dado para realizar a alteração.",
        StatusCode = StatusCodes.Status400BadRequest
    };
};

class Task
{
    private static int InitialId = 1;
    public int Id { get; private set; } = InitialId++;
    public required string Title { get; set; }
    public bool Done { get; set; } = false;
}
