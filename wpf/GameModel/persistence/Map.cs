using CellNM;
using EnumsNM;
using DataAccessNM;

namespace MapNM;

public class Map
{
    public readonly int MAP_SIZE;
    protected Cell[,] _map;

    public Map(int mapSize)
    {
        MAP_SIZE = mapSize;
        _map = new Cell[mapSize, mapSize];
    }

    public Cell this[int row, int column]
    {
        get { return _map[row, column]; }
        set { _map[row, column] = value; }
    }

    public Cell[,] GetMap()
    {
        return _map;
    }

    public static Map Create(MapSize mapSize)
    {
        Map map;
        switch (mapSize)
        {
            case MapSize.Small:
                map = new SmallMap();
                DataAccess.ReadFromFile("../../../../GameModel/maps/small.txt", map);
                break;
            case MapSize.Medium:
                map = new MediumMap();
                DataAccess.ReadFromFile("../../../../GameModel/maps/medium.txt", map);
                break;
            case MapSize.Large:
                map = new LargeMap();
                DataAccess.ReadFromFile("../../../../GameModel/maps/large.txt", map);
                break;
            default:
                throw new Exception("Map creation error.");
        }
        return map;
    }
}

public class SmallMap : Map
{
    public SmallMap() : base(11) { }
}

public class MediumMap : Map
{
    public MediumMap() : base(21) { }
}

public class LargeMap : Map
{
    public LargeMap() : base(35) { }
}
