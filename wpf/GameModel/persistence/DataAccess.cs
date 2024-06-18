using System.Drawing;
using CellNM;
using MapNM;
using EnumsNM;

namespace DataAccessNM;

public class DataAccess
{
    public static void SaveFile(string path, Map map, Point playerPosition)
    {
        string content = $"{map.MAP_SIZE}\n{playerPosition.X} {playerPosition.Y}";
        File.WriteAllText(path, content);
    }

    public static void ReadFromFile(string path, Map map)
    {
        if (map == null)
        {
            throw new ArgumentNullException(nameof(map), "map cannot be null.");
        }

        using StreamReader sr = new(path);

        string line;
        int i = 0;

        while ((line = sr.ReadLine()!) != null)
        {
            for (int j = 0; j < line.Length; j++)
            {
                map[i, j] = new Cell(line[j], j, i);
            }
            i++;
        }
    }

    public static void LoadFromFile(string path, out Point position, out MapSize mapSize)
    {
        using StreamReader sr = new(path);

        mapSize = GetMapSize(sr.ReadLine()!);
        position = GetPlayerPosition(sr.ReadLine()!);
    }

    private static MapSize GetMapSize(string text)
    {
        return text switch
        {
            "11" => MapSize.Small,
            "21" => MapSize.Medium,
            "35" => MapSize.Large,
            _ => throw new Exception(),
        };
    }

    private static Point GetPlayerPosition(string text)
    {
        string[] inputPosition = text.Split();
        return new Point(int.Parse(inputPosition[0]), int.Parse(inputPosition[1]));
    }
}