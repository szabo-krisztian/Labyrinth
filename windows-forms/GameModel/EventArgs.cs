using GameModel.persistence;
using System.Drawing;

namespace GameModelArgs;

public class NewGameArgs : EventArgs
{
    public Size sizeOfMap { get; }
    public Point playerPosition { get; }
    public Size sizeOfcell { get; }
    public int MAP_SIZE { get; }
    public int CELL_SIZE { get; }
    public Cell[,] map { get; }

    public NewGameArgs(Size sizeOfMap, Point playerPosition, Size sizeOfCell, int mapSize, int cellSize, Cell[,] map)
    {
        this.sizeOfMap = sizeOfMap;
        this.sizeOfcell = sizeOfCell;
        this.playerPosition = playerPosition;
        this.MAP_SIZE = mapSize;
        this.CELL_SIZE = cellSize;
        this.map = map;
    }
}


public class LightPair
{
    public Color RGB { get; private set; }
    public Point cellLocation { get; }

    public void SetRGB(Color RGB)
    {
        this.RGB = RGB;
    }

    public LightPair(Color RGB, Point cellLocation)
    {
        this.RGB = RGB;
        this.cellLocation = cellLocation;
    }

    public LightPair(Point cellLocation)
    {
        this.RGB = Color.Orange;
        this.cellLocation = cellLocation;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is LightPair))
        {
            return false;
        }
        return this.cellLocation == ((LightPair)obj).cellLocation;
    }

    public override int GetHashCode()
    {
        return cellLocation.X.GetHashCode() ^ cellLocation.Y.GetHashCode();
    }
}

public class PlayerMovedArgs : EventArgs
{
    public Point newPosition { get; }
    public HashSet<LightPair> cellsToLight { get; }
    public HashSet<Point> cellsToFree { get; }

    public PlayerMovedArgs(Point newPosition, HashSet<LightPair> cellsToLight, HashSet<Point> cellsToFree)
    {
        this.newPosition = newPosition;
        this.cellsToLight = cellsToLight;
        this.cellsToFree = cellsToFree;
    }
}