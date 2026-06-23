using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

// CRUD
// CREATE => OK!
// READ ALL => OK!
// READ ONE => OK!
// UPDATE (PUT) => OK!
// UPDATE (PATCH) => ?
// DELETE => OK!

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
List<Task> tasks = new List<Task>();
app.MapGet("/", () => tasks);

app.MapPost("/tasks", (Task task) =>
{
    tasks.Add(task);
    Results.Created();
    return task;
});

app.MapGet("/tasks/{id}", (int id) =>
{
    Task task = tasks.Find(task => task.Id == id);
    if (task is null) Results.NotFound();
    Results.Ok();
    return task;
});

app.MapPut("/tasks/{id}", (int id, Task task) =>
{
    Task findTask = tasks.Find(task => task.Id == id);
    if (findTask is null) Results.NotFound();
    findTask.Title = task.Title;
    findTask.Done = task.Done;
    Results.Ok();
    return findTask;
});

app.MapDelete("/tasks/{id}", (int id) =>
{
    Task task = tasks.Find(task => task.Id == id);
    if (!tasks.Remove(task)) Results.NotFound();
    return Results.NoContent();
});

app.Run();

class Task
{
    private static int InitialId = 1;
    public int Id { get; private set; } = InitialId++;
    public required string Title { get; set; }
    public bool Done { get; set; } = false;
}
