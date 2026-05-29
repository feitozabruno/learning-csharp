string? handleInputOption;
int parsedOption = 0;

while (true)
{
    Console.WriteLine("=== CALCULADORA ===");
    Console.WriteLine("1 - Somar");
    Console.WriteLine("2 - Subtrair");
    Console.WriteLine("3 - Multiplicar");
    Console.WriteLine("4 - Dividir");
    Console.WriteLine("0 - Sair");
    Console.WriteLine();

    Console.Write("Escolha uma opção: ");
    handleInputOption = Console.ReadLine();
    bool parsed = int.TryParse(handleInputOption, out int option);

    if (parsed && option == 0) break;

    if (parsed && option >= 1 && option <= 4)
    {
        parsedOption = option;
        break;
    }

    Console.WriteLine("Opção inválida, tente novamente..");
    Console.WriteLine();
}

string? handleInputFirstNumber;
double parsedFirstNumber;

while (true)
{
    Console.Write("Digite o primeiro número: ");
    handleInputFirstNumber = Console.ReadLine();
    bool parsed = double.TryParse(handleInputFirstNumber, out double number);

    if (parsed)
    {
        parsedFirstNumber = number;
        break;
    }

    Console.WriteLine("Número inválido, tente novamente..");
    Console.WriteLine();
}

string? handleInputSecondNumber;
double parsedSecondNumber;

while (true)
{
    Console.Write("Digite o segundo número: ");
    handleInputSecondNumber = Console.ReadLine();
    bool parsed = double.TryParse(handleInputSecondNumber, out double number);

    if (parsed)
    {
        parsedSecondNumber = number;
        break;
    }

    Console.WriteLine("Número inválido, tente novamente..");
    Console.WriteLine();
}


switch (parsedOption)
{
    case 1:
        Console.WriteLine($"Resultado {parsedFirstNumber + parsedSecondNumber}");
        break;
    case 2:
        Console.WriteLine($"Resultado {parsedFirstNumber - parsedSecondNumber}");
        break;
    case 3:
        Console.WriteLine($"Resultado {parsedFirstNumber * parsedSecondNumber}");
        break;
    case 4:
        Console.WriteLine($"Resultado {parsedFirstNumber / parsedSecondNumber}");
        break;
}

Console.WriteLine();
