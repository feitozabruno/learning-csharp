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
        Console.WriteLine("0. Sair");
        Console.WriteLine();
        Console.Write("Escolha uma opção: ");
    }

    private static int SelectOption()
    {
        ShowMenu();
        int option = 0;
        while (true)
        {
            string? handleInputOption = Console.ReadLine();
            bool parsed = int.TryParse(handleInputOption, out int parsedOption);

            if (parsed && parsedOption == 0) break;
            if (parsed && parsedOption >= 1 && parsedOption <= 4)
            {
                option = parsedOption;
                break;
            }

            Console.WriteLine("Opção inválida, tente novamente..");
        }
        return option;
    }

    private static Task CreateTask(string title)
    {
        Task newTask = new Task { Title = title };
        return newTask;
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
            Console.WriteLine($"{i + 1} - [{personalDone}] {tasks[i].Title}");
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

    public static void Run()
    {
        List<Task> taskList = new List<Task>();

        while (true)
        {
            int option = SelectOption();
            if (option == 0) break;

            if (option == 1)
            {
                string validTask;

                while (true)
                {
                    Console.Write("Digite o título da tarefa: ");
                    string? task = Console.ReadLine();
                    bool validateTask = string.IsNullOrWhiteSpace(task);

                    if (!validateTask)
                    {
                        validTask = task!;
                        break;
                    }

                    Console.WriteLine("Título inválido, tente novamente..");
                }

                Task newTask = CreateTask(validTask);
                taskList.Add(newTask);
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
            }

            if (option == 4)
            {
                RemoveTask(taskList);
            }
        }
    }
}

class Task
{
    public required string Title { get; set; }
    public bool Done { get; set; } = false;
}