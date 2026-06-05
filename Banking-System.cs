Banking.Run();

class Banking
{
    private static void ShowMenu()
    {
        Console.WriteLine("=== BANCO FEITOZA ===");
        Console.WriteLine("1 - Criar conta");
        Console.WriteLine("2 - Listas contas");
        Console.WriteLine("3 - Consultar saldo");
        Console.WriteLine("4 - Depositar");
        Console.WriteLine("5 - Sacar");
        Console.WriteLine("6 - Transferir");
        Console.WriteLine("0 - Sair");
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
            if (parsed && parsedOption >= 1 && parsedOption <= 6)
            {
                option = parsedOption;
                break;
            }

            Console.WriteLine("Opção inválida, tente novamente..");
        }

        return option;
    }

    private static Account CreateAccount(string cpf, string name)
    {
        return new Account { Cpf = cpf, Holder = name };
    }

    public static void Run()
    {
        int option = SelectOption();

        if (option == 1)
        {
            Console.Write("Digite o seu cpf: ");
            string? cpf = Console.ReadLine();

            Console.WriteLine();

            Console.Write("Digite o seu nome completo: ");
            string? fullName = Console.ReadLine();

            Console.WriteLine();

            if (!string.IsNullOrWhiteSpace(cpf) && !string.IsNullOrWhiteSpace(fullName))
            {
                Account newAccount = CreateAccount(cpf, fullName);
                Console.WriteLine("Conta criada com sucesso!");
                Console.WriteLine($"Número: {newAccount.Cpf}");
                Console.WriteLine($"Titular: {newAccount.Holder}");
            }
            else
            {
                Console.WriteLine("Nome ou CPF Inválidos");
            }
        }
    }
}

class Account
{
    public required string Cpf { get; set; }
    public required string Holder { get; set; }
    public decimal Balance { get; set; } = 0;
}