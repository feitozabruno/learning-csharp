GuessingGame.Run();

class GuessingGame
{
    private static void ShowMenu()
    {
        Console.WriteLine("=== JOGO DE ADIVINHAÇÃO ===");
        Console.WriteLine();
        Console.WriteLine("1 - Fácil");
        Console.WriteLine("2 - Médio");
        Console.WriteLine("3 - Díficil");
        Console.WriteLine("0 - Sair");
        Console.WriteLine();
        Console.Write("Escolha uma opção: ");
    }

    private static int SelectDifficulty()
    {
        int difficulty = 0;

        while (true)
        {
            string? handleInputOption = Console.ReadLine();
            bool parsed = int.TryParse(handleInputOption, out int option);

            if (parsed && option == 0) break;

            if (parsed && option >= 1 && option <= 3)
            {
                difficulty = option;
                break;
            }

            Console.WriteLine("Opção inválida, tente novamente..");
            Console.WriteLine();
            ShowMenu();
        }

        return difficulty;
    }

    private static int GenerateRandomNumber(int min, int max)
    {
        Random random = new Random();
        int randomNumber = random.Next(min, max + 1);
        return randomNumber;
    }

    private static int TakeGuess()
    {
        while (true)
        {
            Console.Write("Digite seu palpite: ");
            string? guess = Console.ReadLine();
            bool parsed = int.TryParse(guess, out int parsedGuess);

            if (parsed)
            {
                return parsedGuess;
            }

            Console.WriteLine("Número inválido, tente novamente..");
            Console.WriteLine();
        }
    }

    public static void Run()
    {
        ShowMenu();
        int difficulty = SelectDifficulty();

        if (difficulty == 0)
        {
            return;
        }

        int maxRandomNumber = difficulty == 1 ? 10 : difficulty == 2 ? 50 : 100;
        int randomNumber = GenerateRandomNumber(1, maxRandomNumber);

        int attempts = 0;

        while (true)
        {
            int guess = TakeGuess();
            attempts += 1;

            if (randomNumber == guess)
            {
                string personalMessage = attempts == 1 ? "de primeira!" : $"em {attempts} tentativas!";
                Console.WriteLine($"Parabéns! Você acertou {personalMessage}");
                Console.WriteLine();
                break;
            }

            if (randomNumber > guess)
            {
                Console.WriteLine("O número secreto é maior.");
                Console.WriteLine();
            }

            if (randomNumber < guess)
            {
                Console.WriteLine("O número secreto é menor.");
                Console.WriteLine();
            }
        }
    }
}
