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
    string handleInputOption = Console.ReadLine();
    Console.WriteLine();

    if (handleInputOption == "0") break;

    Console.Write("Digite o primeiro número da operação: ");
    string handleFirstNumber = Console.ReadLine();
    Console.WriteLine();

    Console.Write("Digite o segundo número da operação: ");
    string handleSecondNumber = Console.ReadLine();
    Console.WriteLine();

    double firstNumber = double.Parse(handleFirstNumber);
    double secondNumber = double.Parse(handleSecondNumber);

    switch (handleInputOption)
    {
        case "1":
            Console.WriteLine($"Resultado {firstNumber + secondNumber}");
            break;
        case "2":
            Console.WriteLine($"Resultado {firstNumber - secondNumber}");
            break;
        case "3":
            Console.WriteLine($"Resultado {firstNumber * secondNumber}");
            break;
        case "4":
            Console.WriteLine($"Resultado {firstNumber / secondNumber}");
            break;
    }
    Console.WriteLine();
}