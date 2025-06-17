using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }

    public List<T>? GetAll()
    {
        IQueryable<T> query = _context.Set<T>();

        var entityType = _context.Model.FindEntityType(typeof(T));
        if (entityType != null)  // Ensure entityType is not null
        {
            foreach (var navigation in entityType.GetNavigations()) // No need for ??
            {
                query = query.Include(navigation.Name);
            }
        }

        return query.ToList();
    }

    public T? GetById(int id) => _dbSet.Find(id) ?? null;

    public List<T> GetBySearch(string searchString, string propertyNameToSearch)
    { // method to search  a set by a searchString, Looking at a specific Property Name
        // e.g. Search all trials by Location
        IQueryable<T> query = _context.Set<T>();
        var results = query
            .Where(x => GetPropertyValue(x, propertyNameToSearch).ToString() != null &&
            GetPropertyValue(x, propertyNameToSearch).ToString().ToUpper() ==
            searchString.ToUpper()).
            ToList();

        return results;
    }

    public EntityEntry<T> GetEntry<T>(T item) where T : class
    {
        var entry = _context.Entry(item);
        return entry;
    }

    public void ResetProperty<T>(T entity, string property) where T : class
    {
        var entry = _context.Entry(entity);
        if (entry.State != EntityState.Detached)
        {
            var originalValue = entry.OriginalValues[property];

            // Reset the current value to the original Value
            entry.CurrentValues[property] = originalValue;

        }
    }

    public void Save(T entity)
    {
        var entry = _context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            _dbSet.Add(entity); // Insert if not tracked
        }
        else
        {
            _dbSet.Update(entity); // Update if already tracked
        }


        try
        {
            _context.SaveChanges(); // Commit changes
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            Console.WriteLine("Inner: " + ex.InnerException?.Message);

            foreach (var item_entry in ex.Entries)
            {
                Console.WriteLine($"Entity: {item_entry.Entity.GetType().Name}");
                Console.WriteLine($"State: {item_entry.State}");

                foreach (var prop in item_entry.CurrentValues.Properties)
                {
                    Console.WriteLine($"{prop.Name}: {item_entry.CurrentValues[prop]}");
                }
            }
        }
    }


    private static object GetPropertyValue(object obj, string propertyName)
    {
        PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
        var value = propertyInfo.GetValue(obj);
        return value;
    }
}