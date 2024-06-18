using System.Drawing;

namespace GameModel.persistence;

public class Cell
{
    public bool IsWall { get; private set; }
    public Point Position { get; private set; }

    public Cell(char value, int x, int y)
    {
        IsWall = value != '.';
        Position = new Point(x, y);
    }
}