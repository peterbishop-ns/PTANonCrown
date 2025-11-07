using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PTANonCrown.Data.Models;
using PTANonCrown.Data.Services;
using System.Reflection;

namespace PTANonCrown.Data.Repository;

public interface IBaseRepository<T> where T : class
{
    void Add(T entity);

    void Delete(int id);

    List<T>? GetAll();

    T? GetById(int id);

    List<T> GetBySearch(string searchString, string propertyNameToSearch);

    EntityEntry<T> GetEntry<T>(T item) where T : class;

    void ResetProperty<T>(T item, string propertyName) where T : class;

    T Save(T entity);
}

public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
{
    protected readonly DatabaseService _databaseService;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public void Add(T entity)
    {
        using var context = _databaseService.GetContext();
        var dbSet = context.Set<T>();
        dbSet.Add(entity);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        using var context = _databaseService.GetContext();
        var dbSet = context.Set<T>();
        var entity = dbSet.Find(id);
        if (entity != null)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }
    }

    public List<T>? GetAll()
    {
        using var context = _databaseService.GetContext();
        IQueryable<T> query = context.Set<T>();

        var entityType = context.Model.FindEntityType(typeof(T));
        if (entityType != null)  // Ensure entityType is not null
        {
            foreach (var navigation in entityType.GetNavigations()) // No need for ??
            {
                query = query.Include(navigation.Name);
            }
        }

        return query.ToList();
    }

    public T? GetById(int id)
    {
        using var context = _databaseService.GetContext();
        return context.Set<T>().Find(id);
    }
    public List<T> GetBySearch(string searchString, string propertyNameToSearch)
    { // method to search  a set by a searchString, Looking at a specific Property Name
      // e.g. Search all trials by Location
        using var context = _databaseService.GetContext();

        IQueryable<T> query = context.Set<T>();
        var results = query
            .Where(x => GetPropertyValue(x, propertyNameToSearch).ToString() != null &&
            GetPropertyValue(x, propertyNameToSearch).ToString().ToUpper() ==
            searchString.ToUpper()).
            ToList();

        return results;
    }

    public EntityEntry<T> GetEntry<T>(T item) where T : class
    {
        using var context = _databaseService.GetContext();

        var entry = context.Entry(item);

        AppLoggerData.Log($"GetEntry. entry = {entry} from item = {item}", "BaseRepository");

        return entry;
    }

    public void ResetProperty<T>(T entity, string property) where T : class
    {
        using var context = _databaseService.GetContext();

        var entry = context.Entry(entity);
        if (entry.State != EntityState.Detached)
        {
            var originalValue = entry.OriginalValues[property];

            // Reset the current value to the original Value
            entry.CurrentValues[property] = originalValue;

        }
    }

    public T Save(T entity)
    {
        using var context = _databaseService.GetContext();
        var dbSet = context.Set<T>();

        if (entity.ID != 0)
        {
            // Existing entity, fetch from DB
            var existing = dbSet.Find(entity.ID);
            if (existing != null)
            {
                AppLoggerData.Log($"Existing entity- {existing} {existing.ID}", "BaseRepository");

                // Copy values into tracked entity
                context.Entry(existing).CurrentValues.SetValues(entity);
            }
            else
            {
                // If not found, treat as new
                AppLoggerData.Log($"Entity not found, adding as new- {entity}", "BaseRepository");
                dbSet.Add(entity);
            }
        }
        else
        {
            // New entity
            AppLoggerData.Log($"New entity- {entity}", "BaseRepository");
            dbSet.Add(entity);
        }

        context.SaveChanges(); // EF updates ID for new entities automatically
        AppLoggerData.Log($"Saved entity ID: {entity.ID}", "BaseRepository");

        return entity; // Return entity with updated ID
    }



    private static object GetPropertyValue(object obj, string propertyName)
    {
        PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
        var value = propertyInfo.GetValue(obj);
        return value;
    }
}