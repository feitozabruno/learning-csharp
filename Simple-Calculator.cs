int Menu()
{
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
        string? handleInputOption = Console.ReadLine();
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

    return parsedOption;
}

double CaptureNumber(string instruction)
{
    double parsedNumber;

    while (true)
    {
        Console.Write(instruction);
        string? handleInputNumber = Console.ReadLine();
        bool parsed = double.TryParse(handleInputNumber, out double number);

        if (parsed)
        {
            parsedNumber = number;
            break;
        }

        Console.WriteLine("Número inválido, tente novamente..");
        Console.WriteLine();
    }

    return parsedNumber;
}

while (true)
{
    int parsedOption = Menu();
    if (parsedOption == 0) break;
    double parsedFirstNumber = CaptureNumber("Digite o primeiro número: ");
    double parsedSecondNumber = CaptureNumber("Digite o segundo número: ");

    switch (parsedOption)
    {
        case 1:
            Console.WriteLine($"Resultado: {parsedFirstNumber + parsedSecondNumber}");
            break;
        case 2:
            Console.WriteLine($"Resultado: {parsedFirstNumber - parsedSecondNumber}");
            break;
        case 3:
            Console.WriteLine($"Resultado: {parsedFirstNumber * parsedSecondNumber}");
            break;
        case 4:
            Console.WriteLine($"Resultado: {parsedFirstNumber / parsedSecondNumber}");
            break;
    }

    Console.WriteLine();
}
