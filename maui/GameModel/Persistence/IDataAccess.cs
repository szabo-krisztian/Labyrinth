namespace Persistence;

public interface IDataAccess
{    
    Task<Map> LoadAsync(String path);

    Task<Map> LoadAsync(Stream path);

    Task SaveAsync(String path, Map table);
}