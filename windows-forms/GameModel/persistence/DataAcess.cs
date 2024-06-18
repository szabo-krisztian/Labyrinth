using EnumsNM;
using System.Drawing;
using GameModel.persistence;

namespace DataAccessNM;

public class DataAccess
{
    public static void SaveFile(string path, int mapSize, Gamemode gamemode, Point playerPosition)
    {
        string newContent = "";

        newContent += gamemode.ToString();
        newContent += "\n" + mapSize.ToString();
        newContent += "\n" + playerPosition.X.ToString() + " " + playerPosition.Y.ToString();

        File.WriteAllText(path, newContent);
    }

    public static void ReadFromFile(string path, Map map)
    {
        if (map == null)
        {
            throw new ArgumentNullException(nameof(map), "map cannot be null.");
        }

        using (StreamReader sr = new StreamReader(path))
        {
            string line;
            int i = 0;
            while ((line = sr.ReadLine()!) != null)
            {
                for (int j = 0; j < line.Length; j++)
                {
                    if (map[i,j] == null)
                    {
                        throw new ArgumentNullException("Map is null");
                    }

                    map[i, j] = new Cell(line[j], j, i);
                }
                i++;
            }
        }
    }

    public static Gamemode LoadFromFile(string path, out Point position, out MapSize mapSize)
    {
        using (StreamReader sr = new StreamReader(path))
        {
            Gamemode gamemode = GetGameMode(sr.ReadLine()!);
            mapSize = GetMapSize(sr.ReadLine()!);
            position = GetPlayerPosition(sr.ReadLine()!);
            return gamemode;
        }
    }

    public static Gamemode GetGameMode(string text)
    {
        switch (text)
        {
            case "Recursion":
                return Gamemode.Recursion;
            case "Laser":
                return Gamemode.Laser;
            case "Normal":
                return Gamemode.Normal;
            default:
                throw new Exception();
        }
    }

    public static MapSize GetMapSize(string text)
    {
        switch (text)
        {
            case "11":
                return MapSize.Small;
            case "21":
                return MapSize.Medium;
            case "35":
                return MapSize.Large;
            default:
                throw new Exception();
        }
    }

    public static Point GetPlayerPosition(string text)
    {
        string[] inputPosition = text.Split();
        return new Point(int.Parse(inputPosition[0]), int.Parse(inputPosition[1]));
    }
}