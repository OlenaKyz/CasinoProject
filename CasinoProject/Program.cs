using System.Collections;
using System.Numerics;

Random rand= new Random();



abstract class Game
{

    public void AddPlayer() { }
    public void GameOn() { }
}

class Game21 : Game
{
    List<ArrayList> players = new List<ArrayList>();

    public void AddPlayer()
    {
        Console.WriteLine("enter name:");
        string name=Console.ReadLine();
        Console.WriteLine("deposit:");
        int deposit=Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("For registration you get 100 bonuses.");
        ArrayList list = new ArrayList() { 0, new Player(name, deposit + 100) };
        players.Add(list);
    }

    public int NumCard(int val)
    {
        if(val==11) return 2;
        if(val==12) return 3;
        if(val==13) return 4;
        return val;
    }

    public void GameOn()
    {
        int vin = 0;
        while(vin!=1)
        {
            for(int  i=0; i<players.Count; i++)
            {

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