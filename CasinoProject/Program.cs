using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Reflection.Emit;
using static System.Formats.Asn1.AsnWriter;

Console.WriteLine("Меню:");
Console.WriteLine("1 - зарегистрировать игрока.");
Console.WriteLine("2 - пополнить счет игрока.");
Console.WriteLine("3 - играть 21 ОЧКО");
Console.WriteLine("4 - играть РУЛЕТКА");
Console.WriteLine("5 - посмотреть рейтинг пользователей.");
Console.WriteLine("6 - ВЫХОД");
int ch;
Casino casino = new Casino();
while (true)
{
    Console.WriteLine("Сделайте выбор в меню:");
    ch = Convert.ToInt32(Console.ReadLine());
    if (ch == 6)
        break;
    switch (ch)
    {
        case 1:
            casino.AddAccount();
            break;
        case 2:
            Console.WriteLine("сумма пополнения:");
            int money = Convert.ToInt32(Console.ReadLine());
            casino.PlayerDeposit(money);
            break;
        case 3:
            casino.PlayGame21();
            break;
        case 4:
            casino.RoulleteGame();
            break;
        case 5:
            casino.PlayerRating();
            break;
        default:
            Console.WriteLine("в меню нет такой опции!");
            break;


    }
}





class PlayerAccount
{
    public string Login { get; set; }
    public string Password { get; set; }
    public int Balance { get; set; }
    public int Win { get; set; }
    public int Loss { get; set; }
    public int Visit { get; set; }


    public void Registration()
    {
        Console.WriteLine("Введите логин:");
        Login = Console.ReadLine();
        while (true)
        {
            Console.WriteLine("Введите пароль");
            Password = Console.ReadLine();
            Console.WriteLine("Повторите пароль");
            string password2 = Console.ReadLine();
            if (password2 != Password)
            {
                Console.WriteLine("Пароли не совпадают.");
                continue;
            }

            break;
        }
        Console.WriteLine("Вы получаете 100 баллов на счет за регистрацию.");
        Balance = 100;
        Win = 0;
        Loss = 0;
        Visit = 1;
    }
}






abstract class Game
{
    //public virtual void AddPlayer() { }
    public virtual void GameOn() { }
}







class Game21 : Game
{
    public List<PlayerAccount> players = new List<PlayerAccount>();
    List<ArrayList> deckCards = new List<ArrayList>() { new ArrayList() { "6 Пики", 6 }, new ArrayList() { "7 Пики", 7 }, new ArrayList() { "8 Пики", 8 }, new ArrayList() { "9 Пики", 9 },
        new ArrayList() { "10 Пики", 10 }, new ArrayList() { "Валет Пики", 2 }, new ArrayList() { "Дама Пики", 3 }, new ArrayList() { "Король Пики", 4 }, new ArrayList() { "Туз Пики", 1 },
        new ArrayList() { "6 Трефы", 6 }, new ArrayList() { "7 Трефы", 7 }, new ArrayList() { "8 Трефы", 8 }, new ArrayList() { "9 Трефы", 9 },
        new ArrayList() { "10 Трефы", 10 }, new ArrayList() { "Валет Трефы", 2 }, new ArrayList() { "Дама Трефы", 3 }, new ArrayList() { "Король Трефы", 4 }, new ArrayList() { "Туз Трефы", 1 },
        new ArrayList() { "6 Бубны", 6 }, new ArrayList() { "7 Бубны", 7 }, new ArrayList() { "8 Бубны", 8 }, new ArrayList() { "9 Бубны", 9 },
        new ArrayList() { "10 Бубны  ", 10 }, new ArrayList() { "Валет Бубны", 2 }, new ArrayList() { "Дама Бубны", 3 }, new ArrayList() { "Король Бубны", 4 }, new ArrayList() { "Туз Бубны", 1 },
        new ArrayList() { "6 Червы", 6 }, new ArrayList() { "7 Червы", 7 }, new ArrayList() { "8 Червы", 8 }, new ArrayList() { "9 Червы", 9 },
        new ArrayList() { "10 Червы", 10 }, new ArrayList() { "Валет Червы", 2 }, new ArrayList() { "Дама Червы", 3 }, new ArrayList() { "Король Червы", 4 }, new ArrayList() { "Туз Червы", 1 } };

    int CardSelection(ref ArrayList usedCards)
    {
        Random rand = new Random();
        while (true)
        {
            int num = rand.Next(usedCards.Count);
            for (int i = 0; i < usedCards.Count; i++)
            {
                int uC = (int)usedCards[i];
                if (num == uC)
                    continue;
            }
            usedCards.Add(num);
            return num;
        }
    }

    int Bet(PlayerAccount player, int bank)
    {
        Console.WriteLine($"Банк - {bank}, ставка не должне превышать банк.");
        Console.WriteLine($"{player.Login} сделайте свою ставку:");
        while (true)
        {
            int bet = Convert.ToInt32(Console.ReadLine());
            if (bet <= bank && bet <= player.Balance)
                return bet;
            Console.WriteLine("Сделайте ставку не больше банка!");
        }

    }
    public override void GameOn()
    {
        Random rand = new Random();

        int banker = rand.Next(players.Count);
        int startBank = players[banker].Balance;
        int bank;
        Console.WriteLine($"игрок {players[banker].Login} банкир.");

        ArrayList usedCards = new ArrayList(); // для записи использованных карт (индекс)

        int tmpToNewCard, tmpToBanker = 0;
        int bet; // ставка
        int chExc = 1; //маркер окончания игры
        int bankerCard, playerCard; //выданная карта банкиру, игроку
        int cardScoreBanker, cardScorePlayer; //счет банкира, игрока

        while (chExc != 0)
        {
            for (int i = 0; i < players.Count; i++)
            {

                if (i == banker)
                    continue;
                bank = players[banker].Balance;

                Console.WriteLine($"Играет игрок {players[i].Login} и банкир {players[banker].Login}");

                bet = Bet(players[i], bank);
                cardScorePlayer = 0;
                while (true)
                {
                    playerCard = CardSelection(ref usedCards);
                    cardScorePlayer += (int)deckCards[playerCard][1];
                    Console.WriteLine($"игрок {players[i].Login} - выпала карта {(string)deckCards[playerCard][0]}, общий счет {cardScorePlayer}");
                    if (cardScorePlayer == 21)
                    {
                        Console.WriteLine($"игрок {players[i].Login} выиграл!");
                        players[i].Balance += bet;
                        players[i].Win++;
                        players[banker].Balance -= bet;
                        players[banker].Loss++;
                        goto Label;
                    }
                    if (cardScorePlayer > 21)
                    {
                        Console.WriteLine($"игрок {players[i].Login} проиграл!");
                        players[i].Balance -= bet;
                        players[i].Loss++;
                        players[banker].Balance += bet;
                        players[banker].Win++;
                        if (players[i].Balance == 0)
                        {
                            Console.WriteLine($"игрок {players[i].Login} выбыл из игры! Закончились деньги!");
                            players.RemoveAt(i);
                            i--;
                        }
                        goto Label;
                    }
                    Console.WriteLine($"{players[i].Login}, берете еще одну карту? Если да нажмите 1, нет - любую клавишу:");
                    tmpToNewCard = Convert.ToInt32(Console.ReadLine());
                    if (tmpToNewCard != 1)
                        break;
                }

                cardScoreBanker = 0;
                while (cardScoreBanker < 17)
                {
                    bankerCard = CardSelection(ref usedCards);
                    cardScoreBanker += (int)deckCards[bankerCard][1];
                    Console.WriteLine($"Банкиру выпала карта {(string)deckCards[bankerCard][0]}, счет - {cardScoreBanker}");
                }

                if (cardScoreBanker == 21 || cardScoreBanker > cardScorePlayer && cardScoreBanker < 21)
                {
                    Console.WriteLine("Банкир выиграл!");
                    players[i].Balance -= bet;
                    players[i].Loss++;
                    players[banker].Balance += bet;
                    players[banker].Win++;
                }
                else
                {
                    Console.WriteLine($"Игрок {players[i].Login} выиграл!");
                    players[i].Balance += bet;
                    players[i].Win++;
                    players[banker].Balance -= bet;
                    players[banker].Loss++;
                }

            Label:

                if (bank == 0)
                {
                    Console.WriteLine("Игра окончена! Банк пуст!");
                    chExc = 0;
                }
                if (bank > startBank * 3)
                {
                    Console.WriteLine("Игра окончена! Банк превысил тройной размер!");
                    chExc = 0;
                }
                if (players.Count == 1)
                {
                    Console.WriteLine("Игра окончена! Все игроки выбыли!");
                    chExc = 0;
                }
                Console.WriteLine("закончить игру - 0, продолжить любую кнопку.");
                chExc = Convert.ToInt32(Console.ReadLine());
                if (chExc != 0)
                    continue;
                break;
            }
        }
    }

}




//___________________________________________________________________________________________________________________________________
class RouletteGame : Game
{
    private List<PlayerAccount> players;
    private bool betOnRed;


    public RouletteGame(List<PlayerAccount> players)
    {
        this.players = players;
    }

    public override void GameOn()
    {
        Console.WriteLine("**Игра в рулетку**");
        Console.WriteLine("Цвета: Зеленое (0), Красное (1), Черное (2)");

        foreach (PlayerAccount player in players)
        {
            Console.WriteLine($"{player.Login}, сделайте ставку на цвет:");
            int colorChoice = Convert.ToInt32(Console.ReadLine());

            bool betOnGreen = (colorChoice == 0);
            bool betOnRed = (colorChoice == 1);

            Console.WriteLine($"{player.Login}, сделайте ставку:");
            int bet = Convert.ToInt32(Console.ReadLine());
            if (bet > player.Balance)
            {
                Console.WriteLine("Недостаточно средств!");
                continue;
            }

            Random rand = new Random();
            int number = rand.Next(37);
            string color = GetColor(number);

            Console.WriteLine($"Выпало число {number} ({color})");

            if (IsWin(number, color, bet, betOnGreen, betOnRed))
            {
                Console.WriteLine($"{player.Login}, вы выиграли!");
                player.Balance += bet;
            }
            else
            {
                Console.WriteLine($"{player.Login}, вы проиграли!");
                player.Balance -= bet;
            }
        }
    }

    private bool IsWin(int number, string color, int bet, bool betOnGreen, bool betOnRed)
    {
        if (color == "Зеленое" && !betOnGreen)
        {
            return false;
        }

        if (color == "Зеленое" && betOnGreen)
        {
            return (number == 0);
        }

        if ((color == "Красное" && betOnRed) || (color == "Черное" && !betOnRed))
        {
            return (number % 2 == 0 && color == "Черное" && !betOnRed) ||
                   (number % 2 != 0 && color == "Красное" && betOnRed);
        }

        return false;
    }

    private string GetColor(int number)
    {
        if (number == 0)
            return "Зеленое";
        return (number % 2 == 0) ? "Черное" : "Красное";
    }
}

//___________________________________________________________________________________________________________________________________



public class Casino
{
    List<PlayerAccount> players = new List<PlayerAccount>();

    public void AddAccount()
    {
        PlayerAccount player = new PlayerAccount();
        player.Registration();
        players.Add(player);
    }


    void PlayersForGame(List<PlayerAccount> playersForGame)
    {
        Console.WriteLine("Выбор игроков для игры.");
        Console.WriteLine("0-играет, не играет - любая");
        int tmp;
        for (int i = 0; i < players.Count; i++)
        {
            Console.WriteLine(players[i].Login);
            tmp = Convert.ToInt32(Console.ReadLine());
            if (tmp == 0)
                playersForGame.Add(players[i]);
        }
    }

    public void PlayGame21()
    {
        Game21 game21 = new Game21();
        if (players.Count > 1)
            PlayersForGame(game21.players);
        else
            Console.WriteLine("нет достаточного колличества зарегистрированных игроков.");

        if (game21.players.Count > 1)
            game21.GameOn();
        else
            Console.WriteLine("в игру могут играть не меньше двух игроков.");
    }

    public void PlayerRating()
    {
        var sortedPeople1 = from p in players
                            orderby p.Win descending
                            select p;
        int i = 1;
        foreach (var p in sortedPeople1)
        {
            Console.WriteLine($"{i}. {p.Login}, побед - {p.Win}");
            i++;
        }
    }

    public void PlayerDeposit(int money)
    {
        Console.WriteLine("Логин:");
        string log = Console.ReadLine();
        Console.WriteLine("Пароль:");
        string pass = Console.ReadLine();
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Login == log && players[i].Password == pass)
                players[i].Balance += money;
        }
    }

    public void RoulleteGame()
    {
        RouletteGame rouletteGame = new RouletteGame(players);
        rouletteGame.GameOn();
    }
}