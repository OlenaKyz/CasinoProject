







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