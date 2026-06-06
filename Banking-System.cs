using System.Globalization;
Banking banking = new Banking();
banking.Run();

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

    private static int SelectMenuOption()
    {
        ShowMenu();
        int option = 0;

        while (true)
        {
            Console.Write("Escolha uma opção: ");
            string? handleInputOption = Console.ReadLine();
            Console.WriteLine();
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

    private static void ListAccounts(List<Account> accounts)
    {
        if (accounts.Count == 0)
        {
            Console.WriteLine("Nenhuma conta cadastrada.");
            return;
        }

        foreach (Account account in accounts)
        {
            Console.WriteLine($"Conta: {account.Number}");
            Console.WriteLine($"Titular: {account.Holder}");
            Console.WriteLine($"Saldo: {account.Balance.ToString("C", new CultureInfo("pt-BR"))}");
            Console.WriteLine();
        }
    }

    public static Account CreateAccount(string name)
    {
        return new Account { Holder = name };
    }

    private static bool ValidateAccountNumber(int number, List<Account> accounts)
    {
        foreach (Account account in accounts)
        {
            if (account.Number == number)
            {
                return true;
            }
        }

        return false;
    }

    public static void Deposit(int number, decimal value)
    {
        // PRECISO ADICIONAR A LISTA DE CONTAS NO CONTEXTO DO PROGRAMA
        // TER QUE INJETAR EM TODOS OS METODOS ESTÁ FICANDO INVIAVEL!

        Console.WriteLine("Nenhuma conta com esse número foi encontrada");
    }

    private static decimal CaptureDepositValue(string instruction)
    {
        decimal validValue;

        while (true)
        {
            Console.Write(instruction);
            string? handleInputValue = Console.ReadLine();
            bool parsed = decimal.TryParse(handleInputValue, out decimal value);

            if (parsed)
            {
                validValue = value;
                break;
            }

            Console.WriteLine("Valor inválido, tente novamente..");
            Console.WriteLine();
        }

        return validValue;
    }

    private static int CaptureAccountNumber()
    {
        int validAccountNumber;

        while (true)
        {
            Console.Write("Digite o número da conta: ");
            string? handleInputValue = Console.ReadLine();
            bool parsed = int.TryParse(handleInputValue, out int number);

            if (parsed)
            {
                validAccountNumber = number;
                break;
            }

            Console.WriteLine("Número inválido, tente novamente..");
            Console.WriteLine();
        }

        return validAccountNumber;
    }

    public void Run()
    {
        List<Account> accounts = new List<Account>();

        while (true)
        {
            int option = SelectMenuOption();

            if (option == 0) break;

            if (option == 1)
            {
                while (true)
                {
                    Console.Write("Nome do titular: ");
                    string? holder = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(holder))
                    {
                        Account newAccount = new Account { Holder = holder };
                        accounts.Add(newAccount);
                        Console.WriteLine($"Conta número: {newAccount.Number} criada com sucesso.");
                        Console.WriteLine();
                        break;
                    }
                    Console.WriteLine("Nome inválido, tente novamente..");
                }
            }

            if (option == 2)
            {
                ListAccounts(accounts);
            }

            if (option == 4)
            {
                int accountNumber = CaptureAccountNumber();
                decimal value = CaptureDepositValue("Digite o valor: ");
                Deposit(accountNumber, value);
            }
        }
    }
}

class Account
{
    public int Number { get; private set; }
    public required string Holder { get; set; }
    public decimal Balance { get; set; } = 0m;

    static int InitialNumber = 1;

    public Account()
    {
        Number = InitialNumber++;
    }
}