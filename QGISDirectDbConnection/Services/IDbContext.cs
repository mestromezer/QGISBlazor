namespace QGISDirectDatabaseConnectionApi.Services;

public interface IDbContext<T>
{
    public Task<List<T>> GetItems();
    public Task<T> GetItem(int id);
    public Task AddItem(T newbie);
    public Task DeleteItem(int id);
    public Task UpdateItem(T newbie);
}
