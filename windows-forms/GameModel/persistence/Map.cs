using DataAccessNM;
using EnumsNM;

namespace GameModel.persistence;

public class Map
{
    public readonly int MAP_SIZE;
    public readonly int CELL_SIZE;
    protected Cell[,] _map;

    public Map(int mapSize, int cellSize)
    {
        MAP_SIZE = mapSize;
        CELL_SIZE = cellSize;
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
                DataAccess.ReadFromFile("./maps/lab_small.txt", map);
                break;
            case MapSize.Medium:
                map = new MediumMap();
                DataAccess.ReadFromFile("./maps/lab_medium.txt", map);
                break;
            case MapSize.Large:
                map = new LargeMap();
                DataAccess.ReadFromFile("./maps/lab_large.txt", map);
                break;
            default:
                throw new Exception("Map creation error.");
        }
        return map;
    }
}

public class SmallMap : Map
{
    public SmallMap() : base(11, 85) { }
}

public class MediumMap : Map
{
    public MediumMap() : base(21, 42) { }
}

public class LargeMap : Map
{
    public LargeMap() : base(35, 25) { }
}
