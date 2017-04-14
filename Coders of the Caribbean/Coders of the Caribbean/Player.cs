using System;

using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

internal class Player
{
    #region Fields
    public List<Barrels> BarrelsList;
    public List<Ships> ShipList;
    public int NumberOfTurn;
    #endregion

    static void Main(string[] args)
    {
        var player = new Player {
            NumberOfTurn = 0
        };
        // game loop
        while (true)
        {
            player.NumberOfTurn++;
            player.ShipList = new List<Ships>();
            player.BarrelsList = new List<Barrels>();

            int myShipCount = int.Parse(Console.ReadLine()); // the number of remaining ships
            int entityCount = int.Parse(Console.ReadLine()); // the number of entities (e.g. ships, mines or cannonballs)
            for (int i = 0; i < entityCount; i++)
            {
                string[] inputs = Console.ReadLine().Split(' ');
                int entityId = int.Parse(inputs[0]);
                string entityType = inputs[1];
                int x = int.Parse(inputs[2]);
                int y = int.Parse(inputs[3]);
                int arg1 = int.Parse(inputs[4]);
                int arg2 = int.Parse(inputs[5]);
                int arg3 = int.Parse(inputs[6]);
                int arg4 = int.Parse(inputs[7]);

                if(entityType == "SHIP")
                {
                    player.ShipList.Add(new Ships
                    {
                        ID = entityId,
                        PosX = x,
                        PosY = y,
                        Orientation = arg1,
                        Speed = arg2,
                        RumUnit = arg3,
                        Team = arg4
                       

                    });


                }
                if (entityType == "BARREL")
                {
                    player.BarrelsList.Add(new Barrels
                    {
                        ID = entityId,
                        PosX = x,
                        PosY = y,
                        Rum=arg1
                    });
                }



            }
            for (int i = 0; i < myShipCount; i++)
            {

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                Console.WriteLine("MOVE 11 10"); // Any valid action, such as "WAIT" or "MOVE x y"
            }
        }
    }
}


#region classes
public class Ships
{
    public int ID { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public int Orientation { get; set; }
    public int Speed { get; set; }
    public int RumUnit { get; set; }
    public int Team { get; set; }
}
public class Barrels
{
    public int ID { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public int Rum { get; set; }
}
#endregion