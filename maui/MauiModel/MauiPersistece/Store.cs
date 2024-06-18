using Persistence;

namespace MauiModel;

public class Store : IStore
{
    /// <summary>
    /// Fájlok lekérdezése.
    /// </summary>
    /// <returns>A fájlok listja.</returns>
    public async Task<IEnumerable<String>> GetFilesAsync()
    {
        return await Task.Run(() => Directory.GetFiles(FileSystem.AppDataDirectory)
            .Select(Path.GetFileName)
            .Where(name => name?.EndsWith(".stl") ?? false)
            .OfType<String>());
    }

    /// <summary>
    /// Módosítás idejének lekérdezése.
    /// </summary>
    /// <param name="name">A fájl neve.</param>
    /// <returns>Az utols módosítás ideje.</returns>
    public async Task<DateTime> GetModifiedTimeAsync(String name)
    {
        var info = new FileInfo(Path.Combine(FileSystem.AppDataDirectory, name));

        return await Task.Run(() => info.LastWriteTime);
    }
}