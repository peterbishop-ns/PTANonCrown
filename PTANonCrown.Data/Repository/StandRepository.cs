using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Context;
using PTANonCrown.Data.Models;
using PTANonCrown.Data.Services;
using System.Collections.Generic;

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

        public List<Treatment>? GetTreatments()
        {
            AppLoggerData.Log("GetTreatments", "StandRepository");
            var context = _databaseService.GetContext();
            var dbSet = context.Set<Treatment>();

            IQueryable<Treatment> query = context.Set<Treatment>();

            return query.ToList();
        }

        public List<TreeSpecies> GetTreeSpecies()
        {
            AppLoggerData.Log("GetTreeSpecies", "StandRepository");
            var context = _databaseService.GetContext();
            var dbSet = context.Set<TreeSpecies>();
            return context.Set<TreeSpecies>()
                .OrderBy(i => i.Name == "Unknown" ? 0 : 1)  // "Unknown" gets priority
                .ThenBy(i => i.Name)                        // Then sort the rest alphabetically
                .ToList();
        }
    }
}
