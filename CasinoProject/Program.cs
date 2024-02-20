using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class Player
{
    public string Username { get; private set; }
    public decimal Balance { get; private set; }
    public List<string> GameHistory { get; private set; }
    public List<string> TransactionHistory { get; private set; }

    public Player(string username)
    {
        Username = username;
        Balance = 0;
        GameHistory = new List<string>();
        TransactionHistory = new List<string>();
    }

    // баланс
    public void UpdateBalance(decimal amount)
    {
        Balance += amount;
    }
    public void Deposit(decimal amount)
    {
        UpdateBalance(amount);
        AddToTransactionHistory($"Пополнение на {amount} виртуальных денег.");
    }


    public void AddToGameHistory(string gameResult)
    {
        GameHistory.Add(gameResult);
    }

    public void AddToTransactionHistory(string transactionDetails)
    {
        TransactionHistory.Add(transactionDetails);
    }

    public void DisplayGameHistory()
    {
        Console.WriteLine($"История игр для игрока {Username}:");
        if (GameHistory.Any())
        {
            foreach (var gameResult in GameHistory)
            {
                Console.WriteLine(gameResult);
            }
        }
        else
        {
            Console.WriteLine("История игр пуста.");
        }
    }

    public void Withdraw(decimal amount)
    {
        if (Balance >= amount)
        {
            UpdateBalance(-amount);
            AddToTransactionHistory($"Вывод {amount} виртуальных денег.");
        }
        else
        {
            Console.WriteLine($"Недостаточно средств на балансе.");
        }
    }
    public void DisplayBalance()
    {
        Console.WriteLine($"Текущий баланс игрока {Username}: {Balance}");
    }
}

public class AccountManager
{
    private List<Player> players;

    public AccountManager()
    {
        players = new List<Player>();
    }

    public void RegisterPlayer()
    {
        Console.Write("Введите ваше имя: ");
        string username = Console.ReadLine();

        if (IsUsernameUnique(username))
        {
            Player player = new Player(username);
            players.Add(player);
            Console.WriteLine($"Игрок {username} успешно зарегистрирован.");
            
        }
        else
        {
            Console.WriteLine("Имя пользователя уже занято. Пожалуйста, выберите другое имя.");
        }
    }

    public void DepositMoney()
    {
        Console.Write("Введите ваше имя: ");
        string username = Console.ReadLine();

        Player player = FindPlayerByUsername(username);

        if (player != null)
        {
            decimal amount = GetValidAmount("Введите сумму для пополнения баланса: ");
            player.UpdateBalance(amount);
            player.AddToTransactionHistory($"Пополнение на {amount} виртуальных денег.");
            player.DisplayBalance();
        }
        else
        {
            Console.WriteLine("Игрок не найден.");
        }
    }

    public void WithdrawMoney()
    {
        Console.Write("Введите ваше имя: ");
        string username = Console.ReadLine();

        Player player = FindPlayerByUsername(username);

        if (player != null)
        {
            decimal amount = GetValidAmount("Введите сумму для вывода средств: ");
            if (player.Balance >= amount)
            {
                player.UpdateBalance(-amount); // Снятие 
                player.AddToTransactionHistory($"Вывод {amount} виртуальных денег.");
                player.DisplayBalance();
            }
            else
            {
                Console.WriteLine($"Недостаточно средств на балансе игрока {username}.");
            }
        }
        else
        {
            Console.WriteLine("Игрок не найден.");
        }
    }

    private decimal GetValidAmount(string prompt)
    {
        decimal amount;
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0)
            {
                return amount;
            }
            else
            {
                Console.WriteLine("Некорректная сумма. Пожалуйста, введите положительное число.");
            }
        }
    }

    private Player FindPlayerByUsername(string username)
    {
        return players.FirstOrDefault(player => player.Username == username);
    }

    private bool IsUsernameUnique(string username)
    {
        return players.All(player => player.Username != username);
    }


    public void PlayGame()
    {
        Console.Write("Введите ваше имя: ");
        string username = Console.ReadLine();

        Player player = FindPlayerByUsername(username);

        if (player != null)
        {
            Console.WriteLine("Выберите игру:");
            Console.WriteLine("1. Блэкджек");
            Console.WriteLine("2. Рулетка");
            Console.WriteLine("3. Покер");
            Console.WriteLine("4. Слот-машина");
            Console.Write("Введите номер игры: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    new Blackjack().Play(player);
                    break;
                case "2":
                    new RouletteGame().Play(player);
                    break;
                case "3":
                    new Poker().Play(player);
                    break;
                case "4":
                    new SlotMachine().Play(player);
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, введите корректный номер.");
                    break;
                
            }
        }
        else
        {
            Console.WriteLine("Игрок не найден.");
        }
    }
}

public abstract class CasinoGame
{
    public abstract void Play(Player player);
}

public class Blackjack : CasinoGame
{
    public override void Play(Player player)
    {
        
    }
}

public class RouletteGame : CasinoGame
{
    private readonly string[] wheelNumbers =
        {"0", "32", "15", "19", "4", "21", "2", "25", "17", "34", "6", "27", "13", "36", "11", "30", "8", "23", "10", "5", "24", "16", "33", "1", "20", "14", "31", "9", "22", "18", "29", "7", "28", "12", "35", "3", "26"};
    private readonly Random random = new Random();

    public override void Play(Player player)
    {
        Console.WriteLine($"Добро пожаловать в игру в рулетку, {player.Username}!");

        decimal betAmount = GetValidBetAmount(player);

        string betType = GetValidBetType();

        string winningNumber = SpinWheel();

        Console.WriteLine($"Выпавшее число: {winningNumber}");

        if (CheckWin(betType, winningNumber))
        {
            decimal winnings = CalculateWinnings(betAmount, betType);
            player.UpdateBalance(winnings);
            Console.WriteLine($"Поздравляем! Вы выиграли {winnings} виртуальных денег.");
        }
        else
        {
            Console.WriteLine("К сожалению, вы проиграли. Попробуйте еще раз!");
        }
    }

    private decimal GetValidBetAmount(Player player)
    {
        while (true)
        {
            Console.Write("Введите сумму ставки: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal betAmount) && betAmount > 0 && betAmount <= player.Balance)
            {
                return betAmount;
            }
            else
            {
                Console.WriteLine("Некорректная сумма ставки. Пожалуйста, введите положительное число, не превышающее ваш баланс.");
            }
        }
    }

    private string GetValidBetType()
    {
        Console.Write("Выберите тип ставки (например, 'четное', 'нечетное', 'красное', 'черное', 'число'): ");
        string betType = Console.ReadLine().ToLower();


        return betType;
    }

    private string SpinWheel()
    {
        int randomIndex = random.Next(0, wheelNumbers.Length);
        return wheelNumbers[randomIndex];
    }

    private bool CheckWin(string betType, string winningNumber)
    {
        // Пример: для числовой ставки
        if (int.TryParse(betType, out int chosenNumber))
        {
            return winningNumber == betType;
        }

        // Пример: для ставки на четное или нечетное
        if (betType.ToLower() == "четное" || betType.ToLower() == "нечетное")
        {
            int parsedWinningNumber;
            if (int.TryParse(winningNumber, out parsedWinningNumber))
            {
                return (parsedWinningNumber % 2 == 0 && betType.ToLower() == "четное") ||
                       (parsedWinningNumber % 2 != 0 && betType.ToLower() == "нечетное");
            }
        }

       
        if (betType.ToLower() == "красное" || betType.ToLower() == "черное")
        {
            // Здесь нужна информация о цвете выпавшего числа (красное или черное)
            // Предположим, что у нас есть метод GetColorForNumber, который возвращает "красное" или "черное"
            string color = GetColorForNumber(winningNumber);
            return color.ToLower() == betType.ToLower();
        }

        return false;
    }

    private decimal CalculateWinnings(decimal betAmount, string betType)
    {
        // Пример: для числовой ставки
        if (int.TryParse(betType, out int chosenNumber))
        {
            return betAmount * 36; // Выигрыш при угадывании числа
        }

        // Пример: для других типов ставок (четное/нечетное, красное/черное)
        // Установим коэффициент выигрыша в зависимости от типа ставки
        decimal winMultiplier = 2; // Пусть для примера коэффициент будет 2 для большинства ставок

        // Уточним коэффициент для числовых ставок
        if (betType.ToLower() == "четное" || betType.ToLower() == "нечетное" || betType.ToLower() == "красное" || betType.ToLower() == "черное")
        {
            winMultiplier = 1; // Для чет/нечет, красное/черное коэффициент будет 1
        }

        return betAmount * winMultiplier;
    }

    private string GetColorForNumber(string number)
    {
        if (int.TryParse(number, out int parsedNumber))
        {
            if (parsedNumber == 0) 
            {
                return "зеленое";
            }
            else if ((parsedNumber >= 1 && parsedNumber <= 10) || (parsedNumber >= 19 && parsedNumber <= 28))
            {
                return parsedNumber % 2 == 0 ? "красное" : "черное";
            }
            else if ((parsedNumber >= 11 && parsedNumber <= 18) || (parsedNumber >= 29 && parsedNumber <= 36))
            {
                return parsedNumber % 2 == 0 ? "черное" : "красное";
            }
        }

        return string.Empty; 
    }
}

public class Poker : CasinoGame
{
    public override void Play(Player player)
    {
        
    }
}

public class SlotMachine : CasinoGame
{
    public override void Play(Player player)
    {
        
    }
}

public class BettingSystem
{
    public void PlaceBet(Player player, decimal amount)
    {
        //Ставки
    }

    public void CalculateWinnings(Player player, decimal multiplier)
    {
        // Расчет выигрышей и обновление баланса
    }
}



public class BonusSystem
{
    public void GrantRegistrationBonus(Player player)
    {
       
    }
}



public class RankSystem
{
    public void UpdatePlayerRank(Player player)
    {
        // Обновление ранга игрока в зависимости от его активности и успехов
    }
}



public class Program
{
    static void Main()
    {

        AccountManager accountManager = new AccountManager();
        accountManager.RegisterPlayer();

        while (true)
        {

            Console.WriteLine("\n1. Внести пополнение");
            Console.WriteLine("2. Вывести средства");
            Console.WriteLine("3. Играть");
            Console.WriteLine("4. Выйти");

            Console.Write("Выберите действие (введите номер): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    accountManager.DepositMoney();
                    break;
                case "2":
                    accountManager.WithdrawMoney();
                    break;
                case "3":
                    accountManager.PlayGame();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, введите корректный номер.");
                    break;
            }
        }
    }
}