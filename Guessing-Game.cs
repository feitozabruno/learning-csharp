GuessingGame.Run();

class GuessingGame
{
    private static void ShowMenu()
    {
        Console.WriteLine("=== JOGO DE ADIVINHAÇÃO ===");
        Console.WriteLine("");
        Console.WriteLine("Estou pensando em um número entre 1 e 100");
        Console.WriteLine("");
    }

    private static int GenerateRandomNumber()
    {
        Random random = new Random();
        int randomNumber = random.Next(1, 101);
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
        int randomNumber = GenerateRandomNumber();

        while (true)
        {
            int guess = TakeGuess();

            if (randomNumber == guess)
            {
                Console.WriteLine("Parabéns! Você acertou!");
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
