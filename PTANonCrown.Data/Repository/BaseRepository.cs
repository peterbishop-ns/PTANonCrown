using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PTANonCrown.Data.Services;
using System.Diagnostics;
using System.Linq.Expressions;
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

    void Save(T entity);
}

public class BaseRepository<T> : IBaseRepository<T> where T : class
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

    public void Save(T entity)
    {
        using var context = _databaseService.GetContext();
        var dbSet = context.Set<T>();

        var entry = context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            dbSet.Add(entity); // Insert if not tracked
        }
        else
        {
            dbSet.Update(entity); // Update if already tracked
        }

        try
        {
            context.SaveChanges(); // Commit changes
        }
        catch (DbUpdateException dbEx) when (dbEx.InnerException is SqliteException sqlEx)
        {
            // Log the raw SQLite error
            AppLoggerData.Log("SQL Error", $"SQLite Error {sqlEx.SqliteErrorCode}: {sqlEx.Message}");

            // Inspect the failing entries
            foreach (var failedEntry in dbEx.Entries)
            {
                var entityName = failedEntry.Entity.GetType().Name;
                AppLoggerData.Log("SQL Error", $"Entity: {entityName}, State: {failedEntry.State}");

                foreach (var prop in failedEntry.CurrentValues.Properties)
                {
                    var value = failedEntry.CurrentValues[prop];
                    AppLoggerData.Log("SQL Error", $"  {prop.Name} = {value ?? "NULL"}");
                }
            }

            throw; // rethrow while preserving stack trace
        }
    }


    private static object GetPropertyValue(object obj, string propertyName)
    {
        PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
        var value = propertyInfo.GetValue(obj);
        return value;
    }
}