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

    private static decimal CaptureDepositValue()
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
                bank.CheckAccountBalance(accountNumber);
            }

            if (option == 4)
            {
                int accountNumber = CaptureAccountNumber();
                decimal value = CaptureDepositValue();
                bank.Deposit(accountNumber, value);
            }
        }
    }
}

class Bank
{
    List<Account> Accounts = new List<Account>();

    public Account CreateAccount(string name)
    {
        Account newAccount = new Account { Holder = name };
        Accounts.Add(newAccount);
        Console.WriteLine($"Conta número: {newAccount.Number} criada com sucesso.");
        Console.WriteLine();
        return newAccount;
    }

    public void ListAccounts()
    {
        if (Accounts.Count == 0)
        {
            Console.WriteLine("Nenhuma conta cadastrada.");
            return;
        }

        foreach (Account account in Accounts)
        {
            Console.WriteLine($"Conta: {account.Number}");
            Console.WriteLine($"Titular: {account.Holder}");
            Console.WriteLine($"Saldo: {account.Balance.ToString("C", new CultureInfo("pt-BR"))}");
            Console.WriteLine();
        }
    }

    private Account ValidateAccountNumber(int number)
    {
        foreach (Account account in Accounts)
        {
            if (account.Number == number)
            {
                return account;
            }
        }

        return null;
    }

    public void CheckAccountBalance(int number)
    {
        Account validAccount = ValidateAccountNumber(number);

        if (validAccount == null)
        {
            Console.WriteLine("Nenhuma conta com esse número foi encontrada");
            return;
        }

        Console.WriteLine($"Conta: {validAccount.Number}");
        Console.WriteLine($"Titular: {validAccount.Holder}");
        Console.WriteLine($"Saldo: {validAccount.Balance.ToString("C", new CultureInfo("pt-BR"))}");
        Console.WriteLine();
        return;
    }

    public void Deposit(int number, decimal value)
    {
        Account validAccount = ValidateAccountNumber(number);

        if (validAccount == null)
        {
            Console.WriteLine("Nenhuma conta com esse número foi encontrada");
            return;
        }

        validAccount.Balance += value;
        Console.WriteLine("Deposito realizado.");
        return;
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