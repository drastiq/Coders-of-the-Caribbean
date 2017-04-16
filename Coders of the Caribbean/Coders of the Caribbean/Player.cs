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
    public Queue<Barrel> BarrelQueue;
    public Queue<Ship> ShipQueue;
    public int NumberOfTurn;
    public int ShipCount;
    public bool Move;
    public List<Barrel> BarrelsListToGet;
    public List<Cannonball> CannonBallList;
    public List<Mine> MineList;
    public List<Entity> TargetList;
    public Dictionary<Object,Object> PosXYdict;
    public Queue<Mine> MineQueue;
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
    #region printMethods
    static void PrintDict(Dictionary<int, int> d)
    {
        foreach (var pair in d)
            Console.WriteLine($"[{pair.Key}]:{pair.Value}");
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
            player.BarrelQueue = new Queue<Barrel>();
            player.PosXYdict = new Dictionary<Object, Object>(); //dictonary with posXY
            player.ShipQueue = new Queue<Ship>();//attack queue
            player.MineQueue = new Queue<Mine>();//mine to destr queueueue

            player.ShipCount = int.Parse(Console.ReadLine()); // the number of remaining ships
            int entityCount = int.Parse(Console.ReadLine()); // the number of entities (e.g. ships, mines or cannonballs)
#region ReadData
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
            #endregion
            player.BarrelQueue();
            
            foreach (var item in player.ShipList.Where(x => x.Team == 1).ToList().OrderBy(x => x.ID).ToList())
            {
                var debb = player.ShipQueue.ToList();
                DebList(debb);
                bool moved = false;
                Barrel barT = null;
                Ship shipA = null;
                Mine MineD = null;
                if (player.BarrelQueue.Any())
                {
                    barT = player.BarrelQueue.Dequeue();
                }
                if (player.MineQueue.Any())
                {
                    MineD = player.MineQueue.Dequeue();
                }
                if (player.ShipQueue.Any())
                {
                     shipA = player.ShipQueue.Dequeue();
                }


                if (barT != null)
                {
                    if (player.NumberOfTurn % 2 != 0 || shipA == null)
                    {
                        Console.WriteLine("MOVE {0} {1}", barT.PosX, barT.PosY);
                        barT = null;
                    }
                   if (shipA != null && player.NumberOfTurn % 2 == 0)
                    {
                        if (MineD != null) {
                            Console.WriteLine("FIRE {0} {1}", MineD.PosX, MineD.PosY);
                            MineD = null;

                        }
                        else {
                            Console.WriteLine("FIRE {0} {1}", shipA.PosX, shipA.PosY);
                            shipA = null;
                        }
                    }
                }
                else {
                    //
                    // shipA = player.ShipQueue.Peek();
                    if (shipA != null)
                    {
                        Console.WriteLine("FIRE {0} {1}", shipA.PosX, shipA.PosY);
                    }
                    else { Console.WriteLine("WAIT"); }
                

                   // shipA = null;
                }



            }
            player.clear();

        }
    }
    #region botOP

    public void BuildQueueueu() {
        // BUILD ACTION QUEUE
        foreach (var myShip in ShipList.Where(x => x.Team == 1).OrderBy(x => x.ID).ToList())
        {
            var barelToget = FindClosestBarrel(myShip.PosX, myShip.PosY);
            var shipToattack = Attack(myShip.PosX, myShip.PosY);
            var MinetoDestroy = DestroyMine(myShip.PosX, myShip.PosY);
            BarrelQueue.Enqueue(barelToget);
            ShipQueue.Enqueue(shipToattack);
            MineQueue.Enqueue(MinetoDestroy);

        }
    }
   public void clear() {
        MineQueue.Clear();
       ShipQueue.Clear();
       BarrelQueue.Clear();
       BarrelsList.Clear();
        MineList.Clear();
    ShipList.Clear();




    }
    int HexagonDist(int x1, int y1, int x2, int y2)
    {
        int a1 = x1 - (int)Math.Floor((double)y1 / 2);
        int b1 = y1;
        int a2 = x2 - (int)Math.Floor((double)y2 / 2);
        int b2 = y2;
        int dx = a1 - a2;
        int dy = b1 - b2;
        return Math.Max(Math.Abs(dx), Math.Max(Math.Abs(dy), Math.Abs(dx + dy)));
    }


    Ship Attack(int x, int y) {
        
        int minDist = 5;
        Ship res = null;
        foreach(var ship in ShipList.Where(v=> v.Team==0).ToList())
        {
            int dist = HexagonDist(x, y, ship.PosX, ship.PosY);
            if (dist < minDist)
            {
                res = ship;
            }
        }
        return res;


    }

    Mine DestroyMine(int x, int y)
    {

        int minDist = 2;
        Mine res = null;
        foreach (var mine in MineList)
        {
            int dist = HexagonDist(x, y, mine.PosX, mine.PosY);
            if (dist < minDist)
            {
                res = mine;
            }
        }
        return res;


    }




    Barrel FindClosestBarrel(int x, int y)
    {
        Barrel res = null;
        int minDist = int.MaxValue;
        foreach (var bar in BarrelsList)
        {
            int dist = HexagonDist(x, y, bar.PosX, bar.PosY);
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
public class PosXY {
    public int PosX { get; set; }
    public int PosY { get; set; }

}


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