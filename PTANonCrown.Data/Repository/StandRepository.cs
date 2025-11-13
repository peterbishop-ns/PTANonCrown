using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Context;
using PTANonCrown.Data.Models;
using PTANonCrown.Data.Services;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace PTANonCrown.Data.Repository
{
    public interface IStandRepository : IBaseRepository<Stand>
    {
    }

    public class StandRepository : BaseRepository<Stand>, IStandRepository
    {

        public StandRepository(DatabaseService databaseService) : base(databaseService)
        {
            AppLoggerData.Log("StandRepository", "StandRepository");

        }

        public List<Stand>? GetAll()
        {
            AppLoggerData.Log("GetAll - start", "StandRepository");
            var context = _databaseService.GetContext();
            var dbSet = context.Set<Stand>();


            var hashStand = context.GetHashCode();

            IQueryable<Stand> query = context.Set<Stand>()
                .Include(s => s.Plots)
                    .ThenInclude(p => p.PlotTreeLive)
                .Include(s => s.Plots)
                    .ThenInclude(p => p.PlotTreatments)
                .Include(s => s.Plots)
                    .ThenInclude(p => p.PlotCoarseWoody)
                 .Include(s => s.Plots)
                    .ThenInclude(p => p.PlotTreeDead);
            

            AppLoggerData.Log("GetAll - end", "StandRepository");

            return query.ToList();
        }

        public EntityState GetEntityState<T>(T obj) where T : BaseModel
        {
            var context = _databaseService.GetContext();

            // Ensure the object is attached to the context
            var entry = context.Entry(obj);

            return entry.State;
        }


        public async Task<List<Treatment>> GetTreatmentsAsync()
        {
            AppLoggerData.Log("GetTreatmentsAsync", "StandRepository");

            var context = _databaseService.GetContext();

            var query = context.Set<Treatment>();

            return await query.ToListAsync();
        }

        public async Task<List<TreeSpecies>> GetTreeSpeciesAsync()
        {
            AppLoggerData.Log("GetTreeSpeciesAsync", "StandRepository");
            var context = _databaseService.GetContext();
            var dbSet = context.Set<TreeSpecies>();
            var query = context.Set<TreeSpecies>()
                .OrderBy(i => i.Name == "Unknown" ? 0 : 1)  // "Unknown" gets priority
                .ThenBy(i => i.Name);                        // Then sort the rest alphabetically
                
            return await query.ToListAsync();
        }
    }
}
