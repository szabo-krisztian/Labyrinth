using System.Drawing;

namespace PlayerNM;

public class Player
{
    public Point Position
    {
        get { return _position; }
    }

    private Point _position;

    public Player(Point position)
    {
        _position = position;
    }

    public void MoveOnX(int dx)
    {
        _position.X += dx;
    }

    public void MoveOnY(int dy)
    {
        _position.Y += dy;
    }
}