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
    public List<Barrels> BarrelsListToGet;
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
            player.BarrelsListToGet = new List<Barrels>();
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
            
            player.BarrelsListToGet.Add(player.FindClosestBarrel(player.BarrelsList));
            
            player.SendCommand();

        }
    }
    public void SendCommand()
    {

        //FindClosestBarrel(BarrelsList);
     
        if (BarrelsListToGet == null || !BarrelsListToGet.Any())
        {
            Console.WriteLine("WAIT");
        }
        else
        {
            var sb = new StringBuilder();

            foreach(var barrelToget in BarrelsListToGet)
            {
                sb.AppendFormat("MOVE {0} {1}", barrelToget.PosX, barrelToget.PosY);
               
            }
            
            Console.WriteLine(sb.ToString());
        }
    }
    public Barrels FindClosestBarrel(List<Barrels> bar) {
        var myShip = ShipList.Where(x => x.Team == 1);
        
       // var cords = BarrelsList.Where(x => x.PosX==myShip[0].PosX).ToList();
       
     //   var cords = BarrelsList.OrderBy(x => x.PosX == myShip[0].PosX).ThenBy(y => y.PosY == myShip[0].PosY).ToList();
        Barrels closestBareler = null;
        var dist = 1000000.0;
        foreach (var ship in ShipList)
        {
            foreach (var barel in bar)
            {
                var x = closestInCartessian(barel, ship);
                if (closestInCartessian(barel, ship) < dist)
                {
                    dist = x;
                    closestBareler = barel;
                   // Console.WriteLine("kkk");
                }

          
            }
        }

        return closestBareler;

    }



    private double closestInCartessian(Barrels target,Ships source)
    {
        return Math.Pow(target.PosX - source.PosX, 2) + Math.Pow(target.PosY - source.PosY, 2);
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