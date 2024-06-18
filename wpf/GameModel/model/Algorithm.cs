using System.Drawing;
using MapNM;

namespace AlgorithmNM;

public class Algorithm
{
    private readonly Map map;

    public Algorithm(Map map)
    {
        this.map = map;
    }

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
        return notOutOfMap && notHeadingToWall;
    }

    public HashSet<Point> GetCellsToLight(Point playerPosition)
    {
        HashSet<Point> cellsToLight = new();
        BruteForce(cellsToLight, playerPosition);
        return cellsToLight;
    }

    #region private sub-functions

    private void BruteForce(HashSet<Point> cellsToLight, Point playerPosition)
    {
        foreach (Point innerSide in GetSurroundingCells(playerPosition))
        {
            cellsToLight.Add(innerSide);

            foreach (Point outerSide in GetSurroundingCells(innerSide))
            {
                cellsToLight.Add(outerSide);

                Point upLeftCorner = new(playerPosition.X - 1, playerPosition.Y - 1);
                if (outerSide == upLeftCorner)
                {
                    UpLeftCornerCase(upLeftCorner, cellsToLight);
                }

                Point upRightCorner = new(playerPosition.X + 1, playerPosition.Y - 1);
                if (outerSide == upRightCorner)
                {
                    UpRightCornerCase(upRightCorner, cellsToLight);
                }

                Point downLeftCorner = new(playerPosition.X - 1, playerPosition.Y + 1);
                if (outerSide == downLeftCorner)
                {
                    DownLeftCorner(downLeftCorner, cellsToLight);
                }

                Point downRightCorner = new(playerPosition.X + 1, playerPosition.Y + 1);
                if (outerSide == downRightCorner)
                {
                    DownRightCorner(downRightCorner, cellsToLight);
                }
            }
        }
    }

    private HashSet<Point> GetSurroundingCells(Point position)
    {
        HashSet<Point> cells = new();

        if (PlayerCanMove(position, new Point(0, 1)))
        {
            cells.Add(new Point(position.X, position.Y + 1));
        }
        if (PlayerCanMove(position, new Point(0, -1)))
        {
            cells.Add(new Point(position.X, position.Y - 1));
        }
        if (PlayerCanMove(position, new Point(1, 0)))
        {
            cells.Add(new Point(position.X + 1, position.Y));
        }
        if (PlayerCanMove(position, new Point(-1, 0)))
        {
            cells.Add(new Point(position.X - 1, position.Y));
        }

        return cells;
    }

    private void UpLeftCornerCase(Point upLeftCorner, HashSet<Point> cellsToLight)
    {
        int cellsLit = 0;
        if (PlayerCanMove(upLeftCorner, new Point(0, -1)))
        {
            cellsToLight.Add(new Point(upLeftCorner.X, upLeftCorner.Y - 1));
            cellsLit++;
        }

        if (PlayerCanMove(upLeftCorner, new Point(-1, 0)))
        {
            cellsToLight.Add(new Point(upLeftCorner.X - 1, upLeftCorner.Y));
            cellsLit++;
        }

        if (cellsLit != 0 && PlayerCanMove(upLeftCorner, new Point(-1, -1)))
        {
            cellsToLight.Add(new Point(upLeftCorner.X - 1, upLeftCorner.Y - 1));
        }
    }

    private void UpRightCornerCase(Point upRightCorner, HashSet<Point> cellsToLight)
    {
        int cellsLit = 0;
        if (PlayerCanMove(upRightCorner, new Point(0, -1)))
        {
            cellsToLight.Add(new Point(upRightCorner.X, upRightCorner.Y - 1));
            cellsLit++;
        }

        if (PlayerCanMove(upRightCorner, new Point(1, 0)))
        {
            cellsToLight.Add(new Point(upRightCorner.X + 1, upRightCorner.Y));
            cellsLit++;
        }

        if (cellsLit != 0 && PlayerCanMove(upRightCorner, new Point(1, -1)))
        {
            cellsToLight.Add(new Point(upRightCorner.X + 1, upRightCorner.Y - 1));
        }
    }

    private void DownLeftCorner(Point downLeftCorner, HashSet<Point> cellsToLight)
    {
        int cellsLit = 0;
        if (PlayerCanMove(downLeftCorner, new Point(0, 1)))
        {
            cellsToLight.Add(new Point(downLeftCorner.X, downLeftCorner.Y + 1));
            cellsLit++;
        }

        if (PlayerCanMove(downLeftCorner, new Point(-1, 0)))
        {
            cellsToLight.Add(new Point(downLeftCorner.X - 1, downLeftCorner.Y));
            cellsLit++;
        }

        if (cellsLit != 0 && PlayerCanMove(downLeftCorner, new Point(-1, 1)))
        {
            cellsToLight.Add(new Point(downLeftCorner.X - 1, downLeftCorner.Y + 1));
        }
    }

    private void DownRightCorner(Point downRightCorner, HashSet<Point> cellsToLight)
    {
        int cellsLit = 0;
        if (PlayerCanMove(downRightCorner, new Point(0, 1)))
        {
            cellsToLight.Add(new Point(downRightCorner.X, downRightCorner.Y + 1));
            cellsLit++;
        }

        if (PlayerCanMove(downRightCorner, new Point(1, 0)))
        {
            cellsToLight.Add(new Point(downRightCorner.X + 1, downRightCorner.Y));
            cellsLit++;
        }

        if (cellsLit != 0 && PlayerCanMove(downRightCorner, new Point(1, 1)))
        {
            cellsToLight.Add(new Point(downRightCorner.X + 1, downRightCorner.Y + 1));
        }
    }

    #endregion
}
