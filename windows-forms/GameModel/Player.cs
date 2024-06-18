using System.Drawing;

namespace PlayerNM;

public class Player
{
    public Point position { get; private set; }

    public void SetPosition(Point position)
    {
        this.position = position;
    }

    public Player(int mapSize)
    {
        position = new Point(0, mapSize - 1);
    }
}