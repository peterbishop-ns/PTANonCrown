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
        private readonly AppDbContext _context;

        public StandRepository(AppDbContext context) : base(context)
        {
            AppLogger.Log("StandRepository", "StandRepository");

            _context = context;
        }

        public List<Stand>? GetAll()
        {
            AppLogger.Log("GetAll - start", "StandRepository");
            var hashStand = _context.GetHashCode();

            IQueryable<Stand> query = _context.Set<Stand>()
                .Include(s => s.Plots)
                    .ThenInclude(p => p.PlotTreeLive)
                .Include(s => s.Plots)
                    .ThenInclude(p=> p.PlotTreatments);
            AppLogger.Log("GetAll - end", "StandRepository");

            return query.ToList();
        }

        public List<Treatment>? GetTreatments()
        {
            AppLogger.Log("GetTreatments", "StandRepository");

            IQueryable<Treatment> query = _context.Set<Treatment>();

            return query.ToList();
        }

        public List<TreeSpecies> GetTreeSpecies()
        {
            AppLogger.Log("GetTreeSpecies", "StandRepository");

            return _context.Set<TreeSpecies>()
    .OrderBy(i => i.Name == "Unknown" ? 0 : 1)  // "Unknown" gets priority
    .ThenBy(i => i.Name)                        // Then sort the rest alphabetically
    .ToList();
        }
    }
}
