using System.Drawing;

namespace Persistence;

public class DataAccess : IDataAccess
{
    private readonly String? _directory = String.Empty;

    public DataAccess(String? saveDirectory = null)
    {
        _directory = saveDirectory;
    }

    public async Task SaveAsync(String path, Map map)
    {
        if (!String.IsNullOrEmpty(_directory))
            path = Path.Combine(_directory, path);

        using StreamWriter writer = new(path);
        
        writer.Write(map.MAP_SIZE);
        writer.Write("\n" + map.Player.Position.X + " " + map.Player.Position.Y + "\n");

        for (Int32 i = 0; i < map.MAP_SIZE; i++)
        {
            for (Int32 j = 0; j < map.MAP_SIZE; j++)
            {
                if (map[i,j].IsWall)
                {
                    await writer.WriteAsync("#");
                }
                else
                {
                    await writer.WriteAsync(".");
                }
            }
            await writer.WriteLineAsync();
        }
    }

    public async Task<Map> LoadAsync(String path)
    {
        if (!String.IsNullOrEmpty(_directory))
            path = Path.Combine(_directory, path);

        using StreamReader reader = new(path);
        
        Int32 mapSize = Int32.Parse(reader.ReadLine()!);
        String[] playerLocArr = reader.ReadLine()!.Split();
        Point playerLocation = new (Int32.Parse(playerLocArr[0]), Int32.Parse(playerLocArr[1]));

        Map map = new (mapSize, playerLocation);
            
        String line;
        for (Int32 i = 0; i < mapSize; i++)
        {
            line = await reader.ReadLineAsync() ?? String.Empty;
            for (Int32 j = 0; j < line.Length; j++)
            {
                map[i, j] = new Cell(line[j], j, i);
            }
        }
        return map;
    }

    public async Task<Map> LoadAsync(Stream path)
    {
        using StreamReader reader = new(path);

        Int32 mapSize = Int32.Parse(reader.ReadLine()!);
        String[] playerLocArr = reader.ReadLine()!.Split();
        Point playerLocation = new (Int32.Parse(playerLocArr[0]), Int32.Parse(playerLocArr[1]));

        Map map = new(mapSize, playerLocation);

        String line;
        for (Int32 i = 0; i < mapSize; i++)
        {
            line = await reader.ReadLineAsync() ?? String.Empty;
            for (Int32 j = 0; j < line.Length; j++)
            {
                map[i, j] = new Cell(line[j], j, i);
            }
        }
        return map;
    }
}