







using System.Numerics;

abstract class Game
{

    public void AddPlayer() { }
    public void GameOn() { }
}

class Game21 : Game
{
    List<Player> players = new List<Player>();

    public void AddPlayer()
    {
        Console.WriteLine("enter name:");
        string name=Console.ReadLine();
        Console.WriteLine("deposit:");
        int deposit=Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();
        Console.WriteLine("");
    }

    public void GameOn()
    {

    }

}



class Player
{
    public string Name { get; set; }
    public int Balance { get; set; }
    Player(string name, int balance)
    {
        Name = name;
        Balance = balance;
    }

}