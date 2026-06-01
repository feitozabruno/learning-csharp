Calculator.Run();

class Calculator
{
    private static void ShowMenu()
    {
        Console.WriteLine("=== CALCULADORA ===");
        Console.WriteLine("1 - Somar");
        Console.WriteLine("2 - Subtrair");
        Console.WriteLine("3 - Multiplicar");
        Console.WriteLine("4 - Dividir");
        Console.WriteLine("5 - Histórico");
        Console.WriteLine("0 - Sair");
        Console.WriteLine();
        Console.Write("Escolha uma opção: ");
    }

    private static int SelectOption()
    {
        int parsedOption = 0;

        while (true)
        {
            string? handleInputOption = Console.ReadLine();
            bool parsed = int.TryParse(handleInputOption, out int option);

            if (parsed && option == 0) break;

            if (parsed && option >= 1 && option <= 5)
            {
                parsedOption = option;
                break;
            }

            Console.WriteLine("Opção inválida, tente novamente..");
            Console.WriteLine();
            ShowMenu();
        }

        return parsedOption;
    }

    private static double CaptureNumber(string instruction)
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

    private static void ShowHistory(List<string> operations)
    {
        if (operations.Count > 0)
        {
            Console.WriteLine("Histórico de Operações:");
            foreach (string operation in operations)
            {
                Console.WriteLine(operation);
            }
            Console.WriteLine();
            return;
        }

        Console.WriteLine("Sem histórico de operações");
        Console.WriteLine();
        return;
    }

    public static void Run()
    {
        double firstNumber;
        double secondNumber;
        List<string> operations = new List<string>();

        while (true)
        {
            ShowMenu();
            int option = SelectOption();

            if (option == 0) break;

            if (option == 5)
            {
                ShowHistory(operations);
            }

            if (option >= 1 && option <= 4)
            {
                firstNumber = CaptureNumber("Digite o primeiro número: ");
                secondNumber = CaptureNumber("Digite o segundo número: ");

                switch (option)
                {
                    case 1:
                        double result1 = firstNumber + secondNumber;
                        Console.WriteLine($"Resultado: {result1}");
                        operations.Add($"{firstNumber} + {secondNumber} = {result1}");
                        break;
                    case 2:
                        double result2 = firstNumber - secondNumber;
                        Console.WriteLine($"Resultado: {result2}");
                        operations.Add($"{firstNumber} - {secondNumber} = {result2}");
                        break;
                    case 3:
                        double result3 = firstNumber * secondNumber;
                        Console.WriteLine($"Resultado: {result3}");
                        operations.Add($"{firstNumber} * {secondNumber} = {result3}");
                        break;
                    case 4:
                        double result4 = firstNumber / secondNumber;
                        Console.WriteLine($"Resultado: {result4}");
                        operations.Add($"{firstNumber} / {secondNumber} = {result4}");
                        break;
                }

                Console.WriteLine();
            }
        }

    }
}