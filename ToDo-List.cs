using System.Text.Json;
using System.Text.Json.Serialization;

ToDoList.Run();

class ToDoList
{
    private static void ShowMenu()
    {
        Console.WriteLine("=== TO-DO LIST ===");
        Console.WriteLine();
        Console.WriteLine("1. Adicionar tarefa");
        Console.WriteLine("2. Listar tarefas");
        Console.WriteLine("3. Concluir tarefa");
        Console.WriteLine("4. Remover tarefa");
        Console.WriteLine("5. Mostrar Estatísticas");
        Console.WriteLine("0. Sair");
        Console.WriteLine();
    }

    private static int SelectOption()
    {
        ShowMenu();
        int option = 0;
        while (true)
        {
            Console.Write("Escolha uma opção: ");
            string? handleInputOption = Console.ReadLine();
            bool parsed = int.TryParse(handleInputOption, out int parsedOption);

            if (parsed && parsedOption == 0) break;
            if (parsed && parsedOption >= 1 && parsedOption <= 5)
            {
                option = parsedOption;
                break;
            }

            Console.WriteLine("Opção inválida, tente novamente..");
        }
        return option;
    }

    private static Priority SelectPriority()
    {
        int priority;

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Selecione a prioridade:");
            Console.WriteLine("1 - Critica");
            Console.WriteLine("2 - Alta");
            Console.WriteLine("3 - Normal");
            Console.WriteLine("4 - Baixa");
            Console.Write("Escolha uma opção: ");

            string? handleInputPriority = Console.ReadLine();
            bool parsed = int.TryParse(handleInputPriority, out int parsedPriority);

            if (parsed && parsedPriority >= 1 && parsedPriority <= 4)
            {
                priority = parsedPriority;
                break;
            }

            Console.WriteLine("Prioridade inválida, tente novamente..");
        }

        if (priority == 1) return Priority.Critica;
        if (priority == 2) return Priority.Alta;
        if (priority == 3) return Priority.Normal;
        if (priority == 4) return Priority.Baixa;
        return Priority.Normal;
    }

    private static Task CreateTask()
    {
        string validTitle;

        while (true)
        {
            Console.Write("Digite o título da tarefa: ");
            string? title = Console.ReadLine();
            bool validateTitle = string.IsNullOrWhiteSpace(title);

            if (!validateTitle)
            {
                validTitle = title!;
                break;
            }

            Console.WriteLine("Título inválido, tente novamente..");
        }

        Priority priority = SelectPriority();
        return new Task { Title = validTitle, Priority = priority };
    }

    private static void ListTasks(List<Task> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Você não possui tarefas, adicione algumas!");
            return;
        }

        for (int i = 0; i < tasks.Count; i++)
        {
            string personalDone = tasks[i].Done ? "X" : " ";
            Console.WriteLine($"{i + 1} - [{personalDone}] {tasks[i].Title} [{tasks[i].Priority}]");
        }

        Console.WriteLine();
        Console.WriteLine("[ ] = pendente");
        Console.WriteLine("[X] = concluída");
        Console.WriteLine();
    }

    private static int CaptureTaskNumber(string message, List<Task> tasks)
    {
        int task = 0;

        while (true)
        {
            Console.Write(message);
            string? handleInput = Console.ReadLine();
            bool parsed = int.TryParse(handleInput, out int parsedTask);

            if (parsed && parsedTask >= 1 && parsedTask <= tasks.Count)
            {
                task = parsedTask - 1;
                break;
            }

            Console.WriteLine("Essa tarefa não existe, tente novamente..");
        }

        return task;
    }

    private static void DoneTask(List<Task> tasks)
    {
        ListTasks(tasks);
        string message = "Digite o número da tarefa que deseja concluir: ";
        int task = CaptureTaskNumber(message, tasks);
        tasks[task].Done = true;
        Console.WriteLine("Tarefa concluída.");
        Console.WriteLine();
        return;
    }

    private static void RemoveTask(List<Task> tasks)
    {
        ListTasks(tasks);
        string message = "Digite o número da tarefa que deseja remover: ";
        int task = CaptureTaskNumber(message, tasks);
        tasks.Remove(tasks[task]);
        Console.WriteLine("Tarefa removida.");
        Console.WriteLine();
        return;
    }

    private static void ShowStatistics(List<Task> tasks)
    {
        int countTaskDone = 0;
        int countTaskPending = 0;

        foreach (Task task in tasks)
        {
            if (task.Done)
            {
                countTaskDone += 1;
            }
            else
            {
                countTaskPending += 1;
            }
        }

        Console.WriteLine("=== Relatório de Tarefas ===");
        Console.WriteLine($"Total: {tasks.Count}");
        Console.WriteLine($"Conclúidas: {countTaskDone}");
        Console.WriteLine($"Pendentes: {countTaskPending}");
        Console.WriteLine();
    }

    private static void SaveTasksFile(List<Task> tasks)
    {
        string json = JsonSerializer.Serialize(tasks, AppJsonContext.Default.ListTask);
        File.WriteAllText("tasks.json", json);
    }

    private static List<Task> ReadTasksFile()
    {
        if (!File.Exists("tasks.json"))
        {
            return new List<Task>();
        }

        string json = File.ReadAllText("tasks.json");

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Task>();
        }

        return JsonSerializer.Deserialize(json, AppJsonContext.Default.ListTask) ?? new List<Task>();
    }

    public static void Run()
    {
        List<Task> taskList = ReadTasksFile();
        if (taskList.Count > 0) Console.WriteLine("Suas tarefas foram carregadas.");

        while (true)
        {
            int option = SelectOption();
            if (option == 0) break;

            if (option == 1)
            {
                Task newTask = CreateTask();
                taskList.Add(newTask);
                SaveTasksFile(taskList);
                Console.WriteLine($"Tarefa adicionada com sucesso.");
                Console.WriteLine();
            }

            if (option == 2)
            {
                ListTasks(taskList);
            }

            if (option == 3)
            {
                DoneTask(taskList);
                SaveTasksFile(taskList);
            }

            if (option == 4)
            {
                RemoveTask(taskList);
                SaveTasksFile(taskList);
            }

            if (option == 5)
            {
                ShowStatistics(taskList);
            }
        }
    }
}

class Task
{
    public required string Title { get; set; }
    public bool Done { get; set; } = false;
    public required Priority Priority { get; set; }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(List<Task>))]
internal partial class AppJsonContext : JsonSerializerContext
{
}

enum Priority
{
    Critica,
    Alta,
    Normal,
    Baixa
}