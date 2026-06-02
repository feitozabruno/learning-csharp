GuessingGame.Run();

class GuessingGame
{
    private static void ShowMenu()
    {
        Console.WriteLine("=== JOGO DE ADIVINHAÇÃO ===");
        Console.WriteLine();
        Console.WriteLine("1 - Fácil (1 - 10)");
        Console.WriteLine("2 - Médio (1 - 50)");
        Console.WriteLine("3 - Díficil (1 - 100)");
        Console.WriteLine("0 - Sair");
        Console.WriteLine();
        Console.Write("Escolha uma opção: ");
    }

    private static int SelectDifficulty()
    {
        ShowMenu();

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

    private static bool TryAgain()
    {
        while (true)
        {
            Console.WriteLine("Deseja jogar novamente?");
            Console.WriteLine("1 - Sim");
            Console.WriteLine("2 - Não");
            Console.Write("Escolha uma opção: ");

            string? handleInputOption = Console.ReadLine();
            Console.WriteLine();

            bool parsed = int.TryParse(handleInputOption, out int option);

            if (parsed && option == 2) return false;

            if (parsed && option == 1) return true;

            Console.WriteLine("Opção inválida, tente novamente..");
            Console.WriteLine();
        }
    }

    private static int GenerateRandomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max + 1);
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
        while (true)
        {
            int difficulty = SelectDifficulty();
            if (difficulty == 0) return;

            int maxRandomNumber = difficulty == 1 ? 10 : difficulty == 2 ? 50 : 100;
            int randomNumber = GenerateRandomNumber(1, maxRandomNumber);

            int maxAttempts = difficulty == 1 ? 5 : difficulty == 2 ? 8 : 10;
            int attempts = 0;

            while (true)
            {
                if (attempts == maxAttempts)
                {
                    Console.WriteLine("Game Over!");
                    Console.WriteLine($"O número era: {randomNumber}");
                    Console.WriteLine();
                    break;
                }

                int guess = TakeGuess();
                attempts += 1;

                if (randomNumber == guess)
                {
                    string personalMessage = attempts == 1 ? "de primeira!" : $"em {attempts} tentativas!";
                    int standAttempts = difficulty == 1 ? 4 : difficulty == 2 ? 6 : 7;
                    Console.WriteLine($"Parabéns! Você acertou {personalMessage}");
                    Console.WriteLine($"A estratégia ideal precisaria de no máximo {standAttempts}.");
                    Console.WriteLine();
                    break;
                }

                if (randomNumber > guess)
                {
                    Console.WriteLine("O número secreto é maior.");
                    Console.WriteLine($"Faltam {maxAttempts - attempts} tentativas.");
                    Console.WriteLine();
                }

                if (randomNumber < guess)
                {
                    Console.WriteLine("O número secreto é menor.");
                    Console.WriteLine($"Faltam {maxAttempts - attempts} tentativas.");
                    Console.WriteLine();
                }
            }

            bool tryAgain = TryAgain();
            if (!tryAgain) break;
        }
    }
}
