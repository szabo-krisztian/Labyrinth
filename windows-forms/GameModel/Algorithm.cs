using System.Drawing;
using GameModelArgs;
using GameModel.persistence;

namespace AlgorithmNM;

public abstract class Algorithm
{
    protected Map map;

    public Algorithm(Map map)
    {
        this.map = map;
    }

    public abstract HashSet<LightPair> GetCellsToLight(Point playerPosition);

    public bool PlayerCanMove(Point start, Point direction)
    {
        bool notOutOfMap = start.X + direction.X <= map.MAP_SIZE - 1 &&
                           start.X + direction.X > -1 &&
                           start.Y + direction.Y <= map.MAP_SIZE - 1 &&
                           start.Y + direction.Y > -1;

        if (!notOutOfMap)
        {
            return false;
        }

        bool notHeadingToWall = !map[start.Y + direction.Y, start.X + direction.X].IsWall;
        return notHeadingToWall;
    }

    protected HashSet<LightPair> GetSurroundingCells(Point position)
    {
        HashSet<LightPair> cells = new HashSet<LightPair>();

        if (PlayerCanMove(position, new Point(0, 1)))
        {
            cells.Add(new LightPair(new Point(position.X, position.Y + 1)));
        }
        if (PlayerCanMove(position, new Point(0, -1)))
        {
            cells.Add(new LightPair(new Point(position.X, position.Y - 1)));
        }
        if (PlayerCanMove(position, new Point(1, 0)))
        {
            cells.Add(new LightPair(new Point(position.X + 1, position.Y)));
        }
        if (PlayerCanMove(position, new Point(-1, 0)))
        {
            cells.Add(new LightPair(new Point(position.X - 1, position.Y)));
        }

        return cells;
    }
}

public class Recursion : Algorithm
{
    public Recursion(Map map) : base(map) { }

    public override HashSet<LightPair> GetCellsToLight(Point playerPosition)
    {
        HashSet<LightPair> cellsToLight = new HashSet<LightPair>();
        LightRecursion(7, 7, GetSurroundingCells(playerPosition), cellsToLight);
        return cellsToLight;
    }

    private void LightRecursion(int depth, int maxDepth, HashSet<LightPair> previousRing, HashSet<LightPair> cellsToLight)
    {
        if (depth == 0)
        {
            return;
        }

        HashSet<LightPair> currentRing = new HashSet<LightPair>();
        foreach (LightPair prevCell in previousRing)
        {
            int blue = (55 / maxDepth) * depth;
            int green = (255 / maxDepth) * depth;
            prevCell.SetRGB(Color.FromArgb(0, green, blue));
            cellsToLight.Add(prevCell);

            foreach (LightPair subCell in GetSurroundingCells(prevCell.cellLocation))
            {
                currentRing.Add(subCell);
            }
        }
        currentRing.ExceptWith(cellsToLight);
        LightRecursion(depth - 1, maxDepth, currentRing, cellsToLight);
    }
}

public class Normal : Algorithm
{
    public Normal(Map map) : base(map) { }

    public override HashSet<LightPair> GetCellsToLight(Point playerPosition)
    {
        HashSet<LightPair> cellsToLight = new HashSet<LightPair>();
        BruteForce(cellsToLight, playerPosition);
        return cellsToLight;
    }

    private void BruteForce(HashSet<LightPair> cellsToLight, Point playerPosition)
    {
        foreach (LightPair innerSide in GetSurroundingCells(playerPosition))
        {
            cellsToLight.Add(innerSide);

            foreach (LightPair outerSide in GetSurroundingCells(innerSide.cellLocation))
            {
                cellsToLight.Add(outerSide);

                Point upLeftCorner = new Point(playerPosition.X - 1, playerPosition.Y - 1);
                if (outerSide.cellLocation == upLeftCorner)
                {
                    UpLeftCornerCase(upLeftCorner, cellsToLight);
                }

                Point upRightCorner = new Point(playerPosition.X + 1, playerPosition.Y - 1);
                if (outerSide.cellLocation == upRightCorner)
                {
                    UpRightCornerCase(upRightCorner, cellsToLight);
                }

                Point downLeftCorner = new Point(playerPosition.X - 1, playerPosition.Y + 1);
                if (outerSide.cellLocation == downLeftCorner)
                {
                    DownLeftCorner(downLeftCorner, cellsToLight);
                }

                Point downRightCorner = new Point(playerPosition.X + 1, playerPosition.Y + 1);
                if (outerSide.cellLocation == downRightCorner)
                {
                    DownRightCorner(downRightCorner, cellsToLight);
                }
            }
        }
    }

    #region BruteForce submethods

    private void UpLeftCornerCase(Point upLeftCorner, HashSet<LightPair> cellsToLight)
    {
        int cellsLit = 0;
        if (PlayerCanMove(upLeftCorner, new Point(0, -1)))
        {
            cellsToLight.Add(new LightPair(new Point(upLeftCorner.X, upLeftCorner.Y - 1)));
            cellsLit++;
        }

        if (PlayerCanMove(upLeftCorner, new Point(-1, 0)))
        {
            cellsToLight.Add(new LightPair(new Point(upLeftCorner.X - 1, upLeftCorner.Y)));
            cellsLit++;
        }

        if (cellsLit != 0 && PlayerCanMove(upLeftCorner, new Point(-1,-1)))
        {
            cellsToLight.Add(new LightPair(new Point(upLeftCorner.X - 1, upLeftCorner.Y - 1)));
        }
    }

    private void UpRightCornerCase(Point upRightCorner, HashSet<LightPair> cellsToLight)
    {
        int cellsLit = 0;
        if (PlayerCanMove(upRightCorner, new Point(0, -1)))
        {
            cellsToLight.Add(new LightPair(new Point(upRightCorner.X, upRightCorner.Y - 1)));
            cellsLit++;
        }

        if (PlayerCanMove(upRightCorner, new Point(1, 0)))
        {
            cellsToLight.Add(new LightPair(new Point(upRightCorner.X + 1, upRightCorner.Y)));
            cellsLit++;
        }

        if (cellsLit != 0 && PlayerCanMove(upRightCorner, new Point(1, -1)))
        {
            cellsToLight.Add(new LightPair(new Point(upRightCorner.X + 1, upRightCorner.Y - 1)));
        }
    }

    private void DownLeftCorner(Point downLeftCorner, HashSet<LightPair> cellsToLight)
    {
        int cellsLit = 0;
        if (PlayerCanMove(downLeftCorner, new Point(0, 1)))
        {
            cellsToLight.Add(new LightPair(new Point(downLeftCorner.X, downLeftCorner.Y + 1)));
            cellsLit++;
        }

        if (PlayerCanMove(downLeftCorner, new Point(-1, 0)))
        {
            cellsToLight.Add(new LightPair(new Point(downLeftCorner.X - 1, downLeftCorner.Y)));
            cellsLit++;
        }

        if (cellsLit != 0 && PlayerCanMove(downLeftCorner, new Point(-1, 1)))
        {
            cellsToLight.Add(new LightPair(new Point(downLeftCorner.X - 1, downLeftCorner.Y + 1)));
        }
    }

    private void DownRightCorner(Point downRightCorner, HashSet<LightPair> cellsToLight)
    {
        int cellsLit = 0;
        if (PlayerCanMove(downRightCorner, new Point(0, 1)))
        {
            cellsToLight.Add(new LightPair(new Point(downRightCorner.X, downRightCorner.Y + 1)));
            cellsLit++;
        }

        if (PlayerCanMove(downRightCorner, new Point(1, 0)))
        {
            cellsToLight.Add(new LightPair(new Point(downRightCorner.X + 1, downRightCorner.Y)));
            cellsLit++;
        }

        if (cellsLit != 0 && PlayerCanMove(downRightCorner, new Point(1, 1)))
        {
            cellsToLight.Add(new LightPair(new Point(downRightCorner.X + 1, downRightCorner.Y + 1)));
        }
    }

    #endregion
}

public class Laser : Algorithm
{
    public Laser(Map map) : base(map) { }

    public override HashSet<LightPair> GetCellsToLight(Point playerPosition)
    {
        HashSet<LightPair> cellsToLight = new HashSet<LightPair>();

        int left = 0;
        while (PlayerCanMove(new Point(playerPosition.X + left, playerPosition.Y), new Point(-1 , 0)))
        {
            cellsToLight.Add(new LightPair(Color.FromArgb(128, 0, 128), new Point(playerPosition.X + left - 1, playerPosition.Y)));
            left--;
        }

        int right = 0;
        while (PlayerCanMove(new Point(playerPosition.X + right, playerPosition.Y), new Point(1, 0)))
        {
            cellsToLight.Add(new LightPair(Color.FromArgb(128, 0, 128), new Point(playerPosition.X + right + 1, playerPosition.Y)));
            right++;
        }

        int down = 0;
        while (PlayerCanMove(new Point(playerPosition.X, playerPosition.Y + down), new Point(0, 1)))
        {
            cellsToLight.Add(new LightPair(Color.FromArgb(128, 0, 128), new Point(playerPosition.X, playerPosition.Y + down + 1)));
            down++;
        }

        int up = 0;
        while (PlayerCanMove(new Point(playerPosition.X, playerPosition.Y + up), new Point(0, -1)))
        {
            cellsToLight.Add(new LightPair(Color.FromArgb(128, 0, 128), new Point(playerPosition.X, playerPosition.Y + up - 1)));
            up--;
        }

        return cellsToLight;
    }
}