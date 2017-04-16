using System;

using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

internal class Player
{
    #region Fields
    public List<Barrel> BarrelsList;
    public List<Ship> ShipList;
    public int NumberOfTurn;
    public int ShipCount;
    public bool Move;
    public List<Barrel> BarrelsListToGet;
    public List<Cannonball> CannonBallList;
    public List<Mine> MineList;
    public List<Entity> TargetList;
    #endregion


    #region DebugMethods
    static void TryCatch(Action a) { try { a(); } catch (Exception) { } }
    static int TryGetInt(Func<int> a) { try { return a(); } catch (Exception) { return 0; } }
    static void Deb(object o) => Console.Error.WriteLine(o);
    static void DebList(IEnumerable<object> e) => Console.Error.WriteLine(e.Aggregate((x, y) => $"{x} {y}"));
    static void DebObjList(IEnumerable<object> e) => TryCatch(() => Console.Error.WriteLine(e.Aggregate((x, y) => $"{x}\n{y}")));
    static void DebDict(Dictionary<int, int> d)
    {
        foreach (var pair in d)
            Console.Error.WriteLine($"[{pair.Key}]:{pair.Value}");
    }
    #endregion  



    static void Main(string[] args)
    {
        var player = new Player {
            NumberOfTurn = 0,
            Move = false,
            ShipCount=1

        };
        // game loop
        while (true)
        {
            player.NumberOfTurn++;
            
            player.ShipList = new List<Ship>();
            player.BarrelsList = new List<Barrel>();
            player.BarrelsListToGet = new List<Barrel>();
            player.MineList = new List<Mine>();
            player.CannonBallList = new List<Cannonball>();
            player.TargetList = new List<Entity>();

            player.ShipCount = int.Parse(Console.ReadLine()); // the number of remaining ships
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
                    player.ShipList.Add(new Ship
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
                    player.BarrelsList.Add(new Barrel
                    {
                        ID = entityId,
                        PosX = x,
                        PosY = y,
                        Rum=arg1
                    });
                }
                if (entityType == "CANNONBALL")
                {
                    player.CannonBallList.Add(new Cannonball
                    {
                        ID = entityId,
                        PosX = x,
                        PosY = y,
                        ShipId = arg1,
                        TurnsBeforeImpact = arg2
                    });
                }
                if (entityType =="MINE")
                {
                    player.MineList.Add(new Mine
                    {
                        ID = entityId,
                        PosX = x,
                        PosY = y
                    });
                }


            }

            

        }
    }
    /*
    /*
    #region OLD
    /* public void SendCommand()
     {


         //FindClosestBarrel(BarrelsList);
         //var myShip = ShipList.Where(x => x.Team == 1).ToList();
         if (BarrelsListToGet == null || !BarrelsListToGet.Any() || Move==true && NumberOfTurn%3==0)
         {
             attack();
             Move = false;
             //Console.WriteLine("WAIT");
         }
         else
         {
            // var sb = new StringBuilder();

             if( BarrelsListToGet[0]!=null)
             {
                 for(int i = 0; i< ShipCount;i++)
                 {
                     //  Console.Error.WriteLine("IN: " + i.ToString());
                    // Console.WriteLine("WAIT");
                     // Console.Error.WriteLine("IN: " + i.ToString());
                    Console.WriteLine("MOVE {0} {1}", BarrelsListToGet.ElementAtOrDefault(i).PosX, BarrelsListToGet.ElementAtOrDefault(i).PosY);
                    // Console.WriteLine("MOVE {0} {1}", BarrelsListToGet[i].PosX, BarrelsListToGet[i].PosY);
                    // Console.Error.WriteLine(BarrelsListToGet[2]+"HUJ");
                   //  Console.Error.WriteLine("DEBUG       MOVE {0} {1}", BarrelsListToGet[i].PosX, BarrelsListToGet[i].PosY);


                     //Move = true;
                 }

             }

            // Console.WriteLine(sb.ToString());
         }
         BarrelsListToGet.Clear();
     }
     public void BuildBarrelList(List<Barrel> bar,List<Ship> myship) {


        // var cords = BarrelsList.Where(x => x.PosX==myShip[0].PosX).ToList();

      //   var cords = BarrelsList.OrderBy(x => x.PosX == myShip[0].PosX).ThenBy(y => y.PosY == myShip[0].PosY).ToList();
         Barrel closestBareler = null;
         var dist = 1000000.0;

         foreach ( var myships in myship )
         {
             closestBareler = null;
             foreach (var barel in bar)
             {
                 var x = CartessianDist(barel, myships);
                 if (CartessianDist(barel, myships) < dist)
                 {
                     dist = x;
                     closestBareler = barel;
                    // Console.WriteLine("kkk");
                 }
             }
             BarrelsListToGet.Add(closestBareler);
             Console.Error.WriteLine("Barrels list size" + BarrelsListToGet.Count.ToString());

         }

         //return closestBareler;

     }
    
    public void BuildTargetList(List<Ship> myship, List<Ship> enemyship)
    {


        // var cords = BarrelsList.Where(x => x.PosX==myShip[0].PosX).ToList();

        //   var cords = BarrelsList.OrderBy(x => x.PosX == myShip[0].PosX).ThenBy(y => y.PosY == myShip[0].PosY).ToList();
        Ship closestShip = null;
        var dist = 1000000.0;

        foreach (var myships in myship)
        {
            closestShip = null;
            foreach (var ship in enemyship)
            {
                var x = CartessianDist(ship, myships);
                if (CartessianDist(ship, myships) < dist)
                {
                    dist = x;
                    closestShip = ship;
                    // Console.WriteLine("kkk");
                }
            }
            TargetList.Add(closestShip);
            Console.Error.WriteLine("Target list sieze" + TargetList.Count.ToString());
        }

        //return closestShip;

    }


    public void attack()
    {
       var enemyShip = ShipList.Where(x => x.Team == 0).ToList();
        for (int i = 0; i < ShipCount; i++)
        {
            if (TargetList.Any() && TargetList.First() != null && BarrelsListToGet.First() != TargetList.First())
            {

                Console.WriteLine("FIRE {0} {1}", TargetList.ElementAtOrDefault(i).PosX, TargetList.ElementAtOrDefault(i).PosY);
                enemyShip.Clear();
            }
            else {

                Console.WriteLine("FIRE {0} {1}", enemyShip.First().PosX, enemyShip.First().PosY);
            }
        }
    }

    //unused
    private double CartessianDist(Entity target,Entity source)
    {
        return Math.Pow(target.PosX - source.PosX, 2) + Math.Pow(target.PosY - source.PosY, 2);
    }
    //
#endregion
*/
    #region botOP
    public void Strategy()
    {
        switch (ShipCount)
        {
            case 1:
            //TODO strategy:
            case 2:
            //TODO Strategy:
            case 3:
                //todo

            default:
                break;
        }

    }

    static int HexagonDist(int x1, int y1, int x2, int y2)
    {
        int a1 = x1 - (int)Math.Floor((double)y1 / 2);
        int b1 = y1;
        int a2 = x2 - (int)Math.Floor((double)y2 / 2);
        int b2 = y2;
        int dx = a1 - a2;
        int dy = b1 - b2;
        return Math.Max(Math.Abs(dx), Math.Max(Math.Abs(dy), Math.Abs(dx + dy)));
    }

    static Barrel FindClosestBarrel(int x, int y)
    {
        Barrel res = null;
        int minDist = int.MaxValue;
        foreach (var bar in BarrelsList)
        {
            int dist = HexagonDist(x, y, bar.X, bar.Y);
            if (dist < minDist)
            {
                minDist = dist;
                res = bar;
            }
        }
        return res;
    }

}
#endregion

#region classes
public class Entity
{
    public int ID { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
}


public class Ship : Entity
{

    public int Orientation { get; set; }
    public int Speed { get; set; }
    public int RumUnit { get; set; }
    public int Team { get; set; }
}
public class Barrel : Entity
{

    public int Rum { get; set; }
}

public class Cannonball : Entity
{
    public int ShipId { get; set; }
    public int TurnsBeforeImpact { get; set; }
   
}

public class Mine : Entity
{
}

#endregion