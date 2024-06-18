using System.Drawing;

namespace Persistence;

public class Map
{
    public readonly int MAP_SIZE;
    private Player _player;

    public Player Player
    {
        get { return _player; }
    }

    private Cell[,] _map;

    public Map(int mapSize, Point playerPosition)
    {
        MAP_SIZE = mapSize;
        _map = new Cell[mapSize, mapSize];
        _player = new Player(playerPosition);
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
}