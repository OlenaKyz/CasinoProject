using System;
using System.Collections;
using System.Numerics;
using System.Reflection.Emit;
using static System.Formats.Asn1.AsnWriter;


int ch;
while(true)
{
    Console.WriteLine("Выберите игру: 1 - рулетка, 2 - 21 очко.");
    ch =Convert.ToInt32(Console.ReadLine());
    if (ch == 1 || ch == 2)
        break;
    Console.WriteLine("Нет игры с таким номером.");
}

Game21 game21 = new Game21();
if(ch == 1)
    Console.WriteLine("Игра рулетка");

if (ch == 2)
    game21.GameOn();



abstract class Game
{

    public virtual void AddPlayer() { }
    public virtual void GameOn() { }
}

class Game21 : Game
{
    List<Player> players = new List<Player>();
    List<ArrayList> deckCards = new List<ArrayList>() { new ArrayList() { "6 Пики", 6 }, new ArrayList() { "7 Пики", 7 }, new ArrayList() { "8 Пики", 8 }, new ArrayList() { "9 Пики", 9 },
        new ArrayList() { "10 Пики", 10 }, new ArrayList() { "Валет Пики", 2 }, new ArrayList() { "Дама Пики", 3 }, new ArrayList() { "Король Пики", 4 }, new ArrayList() { "Туз Пики", 1 },
        new ArrayList() { "6 Трефы", 6 }, new ArrayList() { "7 Трефы", 7 }, new ArrayList() { "8 Трефы", 8 }, new ArrayList() { "9 Трефы", 9 },
        new ArrayList() { "10 Трефы", 10 }, new ArrayList() { "Валет Трефы", 2 }, new ArrayList() { "Дама Трефы", 3 }, new ArrayList() { "Король Трефы", 4 }, new ArrayList() { "Туз Трефы", 1 },
        new ArrayList() { "6 Бубны", 6 }, new ArrayList() { "7 Бубны", 7 }, new ArrayList() { "8 Бубны", 8 }, new ArrayList() { "9 Бубны", 9 },
        new ArrayList() { "10 Бубны  ", 10 }, new ArrayList() { "Валет Бубны", 2 }, new ArrayList() { "Дама Бубны", 3 }, new ArrayList() { "Король Бубны", 4 }, new ArrayList() { "Туз Бубны", 1 },
        new ArrayList() { "6 Червы", 6 }, new ArrayList() { "7 Червы", 7 }, new ArrayList() { "8 Червы", 8 }, new ArrayList() { "9 Червы", 9 },
        new ArrayList() { "10 Червы", 10 }, new ArrayList() { "Валет Червы", 2 }, new ArrayList() { "Дама Червы", 3 }, new ArrayList() { "Король Червы", 4 }, new ArrayList() { "Туз Червы", 1 } };



    public override void AddPlayer()
    {
        Console.WriteLine("Введите имя игрока:");
        string name = Console.ReadLine();
        Console.WriteLine("Сколько денег на счет:");
        int deposit = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("За регистрацию бонус 100 баллов.");
        players.Add(new Player(name, deposit + 100));
    }
    int CardSelection(ref ArrayList usedCards)
    {
        Random rand = new Random();
        while (true)
        {
            int num = rand.Next(usedCards.Count);
            for(int i=0; i<usedCards.Count; i++)
            {
                int uC = (int)usedCards[i];
                if (num == uC)
                    continue;
            }
            usedCards.Add(num);
            return num;
        }
    }

    int Bet(Player player, int bank)
    {
        Console.WriteLine($"Банк - {bank}, ставка не должне превышать банк.");
        Console.WriteLine($"{player.Name} сделайте свою ставку:");
        while(true)
        {
            int bet = Convert.ToInt32(Console.ReadLine());
            if (bet <= bank && bet<= player.Balance)
                return bet;
            Console.WriteLine("Сделайте ставку не больше банка!");
        }

    }

    public override void GameOn()
    {
        Random rand = new Random();

        Console.WriteLine("Сколько игроков играет:");
        int countPlayers=Convert.ToInt32(Console.ReadLine());
        for(int i=0; i < countPlayers; i++)
            AddPlayer();

        int banker = rand.Next(countPlayers);
        int startBank = players[banker].Balance;
        int bank;
        Console.WriteLine($"игрок {players[banker].Name} банкир.");

        ArrayList usedCards= new ArrayList(); // для записи использованных карт (индекс)

        int tmpToNewCard, tmpToBanker=0, bet; //
        int bankerCard, playerCard; //выданная карта банкиру, игроку
        int cardScoreBanker, cardScorePlayer; //счет банкира, игрока

        while(true)
        {
            for(int i=0;  i < players.Count; i++)
            {

                if (i == banker)
                    continue;
                bank = players[banker].Balance;

                Console.WriteLine($"Играет игрок {players[i].Name} и банкир {players[banker].Name}");

                bet = Bet(players[i], bank);
                cardScorePlayer = 0;
                while (true)
                {
                    playerCard = CardSelection(ref usedCards);
                    cardScorePlayer += (int)deckCards[playerCard][1];
                    Console.WriteLine($"игрок {players[i].Name} - выпала карта {(string)deckCards[playerCard][0]}, общий счет {cardScorePlayer}");
                    if(cardScorePlayer==21)
                    {
                        Console.WriteLine($"игрок {players[i].Name} выиграл!");
                        players[i].Balance += bet;
                        players[banker].Balance -= bet;
                        goto Label;
                    }
                    if(cardScorePlayer>21)
                    {
                        Console.WriteLine($"игрок {players[i].Name} проиграл!");
                        players[i].Balance -= bet;
                        players[banker].Balance += bet;
                        if(players[i].Balance==0)
                        {
                            Console.WriteLine($"игрок {players[i].Name} выбыл из игры! Закончились деньги!");
                            players.RemoveAt(i);
                            i--;
                        }
                        goto Label;
                    }
                    Console.WriteLine($"{players[i].Name}, берете еще одну карту? Если да нажмите 1, нет - любую клавишу:");
                    tmpToNewCard = Convert.ToInt32( Console.ReadLine() );
                    if (tmpToNewCard != 1)
                        break;
                }

                    cardScoreBanker = 0;
                    while(cardScoreBanker<17)
                    {
                        bankerCard = CardSelection(ref usedCards);
                        cardScoreBanker += (int)deckCards[bankerCard][1];
                        Console.WriteLine($"Банкиру выпала карта {(string)deckCards[bankerCard][0]}, счет - {cardScoreBanker}");
                    }

                    if(cardScoreBanker == 21 || cardScoreBanker > cardScorePlayer && cardScoreBanker<21)
                    {
                        Console.WriteLine("Банкир выиграл!");
                        players[i].Balance -= bet;
                        players[banker].Balance += bet;
                    }
                    else
                    {
                        Console.WriteLine($"Игрок {players[i].Name} выиграл!");
                        players[i].Balance += bet;
                        players[banker].Balance -= bet;
                }

                Label:

                if (bank == 0)
                {
                    Console.WriteLine("Игра окончена! Банк пуст!");
                    break;
                }
                if (bank > startBank * 3)
                {
                    Console.WriteLine("Игра окончена! Банк превысил тройной размер!");
                    break;
                }
                if(players.Count==1)
                {
                    Console.WriteLine("Игра окончена! Все игроки выбыли!");
                    break;
                }
            }
        }
    }
}



class Player
{
    public string Name { get; set; }
    public int Balance { get; set; }
    public Player(string name, int balance)
    {
        Name = name;
        Balance = balance;
    }

}