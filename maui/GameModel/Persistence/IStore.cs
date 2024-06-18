namespace Persistence;

public interface IStore
{
    Task<IEnumerable<String>> GetFilesAsync();

    Task<DateTime> GetModifiedTimeAsync(String name);
}