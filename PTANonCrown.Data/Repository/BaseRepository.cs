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
        var context = _databaseService.GetContext();
        var dbSet = context.Set<T>();
        dbSet.Add(entity);
        context.SaveChanges();
    }

    public void Delete(int id)
    {
        var context = _databaseService.GetContext();
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
        var context = _databaseService.GetContext();
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
        var context = _databaseService.GetContext();
        return context.Set<T>().Find(id);
    }
    public List<T> GetBySearch(string searchString, string propertyNameToSearch)
    { // method to search  a set by a searchString, Looking at a specific Property Name
      // e.g. Search all trials by Location
        var context = _databaseService.GetContext();

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
        var context = _databaseService.GetContext();

        var entry = context.Entry(item);

        AppLoggerData.Log($"GetEntry. entry = {entry} from item = {item}", "BaseRepository");

        return entry;
    }

    public void ResetProperty<T>(T entity, string property) where T : class
    {
        var context = _databaseService.GetContext();

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
        try
        {

            var context = _databaseService.GetContext();

            var entry = context.Entry(entity);

            AppLoggerData.Log($"Trying to save entity {entity} - ({entry.State})", "BaseRepository");


            if (entry.State == EntityState.Detached)
            {
                // Entity not tracked yet — decide add vs update based on key
                if (entity.ID == 0)
                {
                    context.Add(entity);
                }
                else
                {
                    // Attach but mark as modified
                    context.Attach(entity);
                    entry.State = EntityState.Modified;
                }
            }

            try
            {
                foreach (var prop in entry.Properties)
                {
                    AppLoggerData.Log($"{prop.Metadata.Name}: Current={prop.CurrentValue}, Original={prop.OriginalValue}, IsModified={prop.IsModified}", "BaseRepository");
                }
                context.SaveChanges(); 
                AppLoggerData.Log($"Saved entity ID: {entity.ID}", "BaseRepository");
            }
            catch (DbUpdateException dbEx)
            {
                AppLoggerData.Log($"DbUpdateException saving {entity.GetType().Name} ID: {entity.ID}. SQLite error: {dbEx.InnerException?.Message ?? dbEx.Message}", "BaseRepository");
                throw;
            }
            catch (Exception ex)
            {
                AppLoggerData.Log($"Unexpected exception saving {entity.GetType().Name} ID: {entity.ID}: {ex}", "BaseRepository");
                throw;
            }

            // FLush the WAL file so; when the working db gets copied to the save directory, we need all changes included int he db file. 

            using var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "PRAGMA wal_checkpoint(FULL);";
            context.Database.OpenConnection();
            command.ExecuteNonQuery();



            return entity;
        }
        catch (Exception ex)
        {
            AppLoggerData.Log($"Save failed for {entity.GetType().Name}: {ex}", "BaseRepository");
            throw;
        }



       
    }




    private static object GetPropertyValue(object obj, string propertyName)
    {
        PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
        var value = propertyInfo.GetValue(obj);
        return value;
    }
}