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
        Console.WriteLine("7 - Histórico");
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
            if (parsed && parsedOption >= 1 && parsedOption <= 7)
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

    private enum Menu
    {
        CreateAccount = 1,
        ListAccounts = 2,
        CheckAccountBalance = 3,
        Deposit = 4,
        Withdrawal = 5,
        Transfer = 6,
        Transactions = 7,
        Exit = 0
    }

    public static void Run()
    {
        Bank bank = new Bank();
        while (true)
        {
            int option = SelectMenuOption();

            if (option == (int)Menu.Exit) break;

            if (option == (int)Menu.CreateAccount)
            {
                string holder = CaptureHolderName();
                bank.CreateAccount(holder);
            }

            if (option == (int)Menu.ListAccounts)
            {
                bank.ListAccounts();
            }

            if (option == (int)Menu.CheckAccountBalance)
            {
                int accountNumber = CaptureAccountNumber();
                Account? account = bank.FindAccount(accountNumber);
                account?.CheckBalance();
            }

            if (option == (int)Menu.Deposit)
            {
                int accountNumber = CaptureAccountNumber();
                decimal value = CaptureOperationValue();
                Account? account = bank.FindAccount(accountNumber);
                account?.Deposit(value);
            }

            if (option == (int)Menu.Withdrawal)
            {
                int accountNumber = CaptureAccountNumber();
                decimal value = CaptureOperationValue();
                Account? account = bank.FindAccount(accountNumber);
                account?.Withdrawal(value);
            }

            if (option == (int)Menu.Transfer)
            {
                Console.WriteLine("Conta de origem:");
                int accountOriginNumber = CaptureAccountNumber();

                Console.WriteLine("Conta de destino:");
                int accountDestinationNumber = CaptureAccountNumber();

                decimal value = CaptureOperationValue();
                Account? accountOrigin = bank.FindAccount(accountOriginNumber);
                Account? accountDestination = bank.FindAccount(accountDestinationNumber);
                accountOrigin?.Transfer(accountDestination, value);
            }

            if (option == (int)Menu.Transactions)
            {
                int accountNumber = CaptureAccountNumber();
                Account? account = bank.FindAccount(accountNumber);
                account?.Transactions();
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
        Console.WriteLine($"Conta número: {newAccount.Number} foi criada com sucesso.");
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
}

class Account()
{
    private readonly List<Transaction> transactions = new List<Transaction>();
    private static int InitialNumber = 1;
    public int Number { get; private set; } = InitialNumber++;
    public required string Holder { get; init; }
    public decimal Balance { get; set; } = 0m;

    public void CheckBalance()
    {
        Console.WriteLine($"Conta: {Number}");
        Console.WriteLine($"Titular: {Holder}");
        Console.WriteLine($"Saldo: {Balance.ToString("C", new CultureInfo("pt-BR"))}");
        Console.WriteLine();
    }

    public void Deposit(decimal value)
    {
        if (value <= 0)
        {
            Console.WriteLine("Valor de depósito inválido.");
            return;
        }

        Transaction transaction = new Transaction
        {
            AccountNumber = Number,
            Type = TransactionType.Deposit,
            Value = value
        };

        Balance += value;
        transactions.Add(transaction);
        Console.WriteLine("Depósito realizado.");
    }

    public void Withdrawal(decimal value)
    {
        if (value <= 0)
        {
            Console.WriteLine("Valor de saque inválido.");
            return;
        }

        if (Balance < value)
        {
            Console.WriteLine("Saldo insuficiente.");
            return;
        }

        Transaction transaction = new Transaction
        {
            AccountNumber = Number,
            Type = TransactionType.Withdrawal,
            Value = value
        };

        Balance -= value;
        transactions.Add(transaction);
        Console.WriteLine("Saque realizado.");
        return;
    }

    public void Transfer(Account? accountDestination, decimal value)
    {
        if (value <= 0)
        {
            Console.WriteLine("Valor de transferência inválido.");
            return;
        }

        Transaction transactionOrigin = new Transaction
        {
            AccountNumber = Number,
            Type = TransactionType.TransferSent,
            Value = value
        };

        Transaction transactionDestination = new Transaction
        {
            AccountNumber = accountDestination!.Number,
            Type = TransactionType.TransferReceived,
            Value = value
        };

        Balance -= value;
        transactions.Add(transactionOrigin);

        accountDestination.Balance += value;
        accountDestination.transactions.Add(transactionDestination);

        Console.WriteLine("Transferência realizada.");
        return;
    }

    private static string FormatCurrency(decimal money)
    {
        return money.ToString("C", new CultureInfo("pt-BR"));
    }

    public void Transactions()
    {
        if (transactions.Count == 0)
        {
            Console.WriteLine("Nenhuma transação encontrada.");
            return;
        }

        foreach (Transaction transaction in transactions)
        {
            if (transaction.Type == TransactionType.Deposit)
            {
                Console.WriteLine($"+ {FormatCurrency(transaction.Value)} Depósito");
            }
            if (transaction.Type == TransactionType.Withdrawal)
            {
                Console.WriteLine($"- {FormatCurrency(transaction.Value)} Saque");
            }
            if (transaction.Type == TransactionType.TransferSent)
            {
                Console.WriteLine($"- {FormatCurrency(transaction.Value)} Transferência enviada");
            }
            if (transaction.Type == TransactionType.TransferReceived)
            {
                Console.WriteLine($"+ {FormatCurrency(transaction.Value)} Transferência recebida");
            }
        }
    }
}

class Transaction()
{
    public int AccountNumber { get; set; }
    public required TransactionType Type { get; set; }
    public decimal Value { get; set; }
}

enum TransactionType
{
    Deposit,
    Withdrawal,
    TransferSent,
    TransferReceived
}
