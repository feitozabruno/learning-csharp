using System.Globalization;
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

    private static decimal CaptureOperationValue()
    {
        decimal validValue;

        while (true)
        {
            Console.Write("Digite o valor: ");
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

    private static string CaptureHolderName()
    {
        while (true)
        {
            Console.Write("Nome do titular: ");
            string? holder = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(holder))
            {
                return holder;
            }
            Console.WriteLine("Nome inválido, tente novamente..");
        }
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

    public static void Run()
    {
        Bank bank = new Bank();
        while (true)
        {
            int option = SelectMenuOption();

            if (option == 0) break;

            if (option == 1)
            {
                string holder = CaptureHolderName();
                bank.CreateAccount(holder);
            }

            if (option == 2)
            {
                bank.ListAccounts();
            }

            if (option == 3)
            {
                int accountNumber = CaptureAccountNumber();
                Account? account = bank.FindAccount(accountNumber);

                if (account != null)
                {
                    Console.WriteLine($"Conta: {account.Number}");
                    Console.WriteLine($"Titular: {account.Holder}");
                    Console.WriteLine($"Saldo: {account.Balance.ToString("C", new CultureInfo("pt-BR"))}");
                    Console.WriteLine();
                }
            }

            if (option == 4)
            {
                int accountNumber = CaptureAccountNumber();
                decimal value = CaptureOperationValue();
                Account? account = bank.FindAccount(accountNumber);
                account?.Deposit(value);
            }

            if (option == 5)
            {
                int accountNumber = CaptureAccountNumber();
                decimal value = CaptureOperationValue();
                Account? account = bank.FindAccount(accountNumber);
                account?.Withdrawal(value);
            }

            if (option == 6)
            {
                Console.WriteLine("Conta de origem");
                int accountNumber1 = CaptureAccountNumber();
                Console.WriteLine("Conta de destino");
                int accountNumber2 = CaptureAccountNumber();
                decimal value = CaptureOperationValue();
                bank.MoneyTransfer(accountNumber1, accountNumber2, value);
            }
        }
    }
}

class Bank
{
    private readonly List<Account> accounts = new List<Account>();

    public Account CreateAccount(string name)
    {
        Account newAccount = new Account { Holder = name };
        accounts.Add(newAccount);
        Console.WriteLine($"Conta número: {newAccount.Number} criada com sucesso.");
        Console.WriteLine();
        return newAccount;
    }

    public void ListAccounts()
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

    public Account? FindAccount(int number)
    {
        foreach (Account account in accounts)
        {
            if (account.Number == number)
            {
                return account;
            }
        }

        Console.WriteLine("Nenhuma conta com esse número foi encontrada");
        return null;
    }

    public void MoneyTransfer(int origin, int destination, decimal value)
    {
        if (value <= 0)
        {
            Console.WriteLine("Valor de transferência inválido.");
            return;
        }

        Account? accountOrigin = FindAccount(origin);
        Account? accountDestination = FindAccount(destination);

        if (accountOrigin == null || accountDestination == null)
        {
            Console.WriteLine("A conta de origem ou de destino é inválida");
            return;
        }

        accountOrigin?.Withdrawal(value);
        accountDestination?.Deposit(value);
        Console.WriteLine("Transferência finalizada");
        return;
    }
}

class Account()
{
    private static int InitialNumber = 1;
    public int Number { get; private set; } = InitialNumber++;
    public required string Holder { get; init; }
    public decimal Balance { get; set; } = 0m;

    public void Deposit(decimal value)
    {
        if (value <= 0)
        {
            Console.WriteLine("Valor de depósito inválido.");
            return;
        }

        Balance += value;
        Console.WriteLine("Depósito realizado.");
    }

    public void Withdrawal(decimal value)
    {
        if (value <= 0)
        {
            Console.WriteLine("Valor de saque inválido.");
            return;
        }

        if (Balance >= value)
        {
            Balance -= value;
            Console.WriteLine("Saque realizado.");
            return;
        }

        Console.WriteLine("Saldo insuficiente.");
        return;
    }
}
