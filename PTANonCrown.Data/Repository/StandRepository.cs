using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Context;
using PTANonCrown.Data.Models;
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
            _context = context;
        }

        public List<Stand>? GetAll()
        {
            IQueryable<Stand> query = _context.Set<Stand>()
                .Include(s => s.Plots)
                    .ThenInclude(p => p.PlotTreeLive)
                .Include(s => s.Plots)
                    .ThenInclude(p=> p.PlotTreatments);

            return query.ToList();
        }

        public List<Treatment>? GetTreatments()
        {
            IQueryable<Treatment> query = _context.Set<Treatment>();

            return query.ToList();
        }

        public List<TreeSpecies> GetTreeSpecies()
        {
            return _context.Set<TreeSpecies>()
    .OrderBy(i => i.Name == "Unknown" ? 0 : 1)  // "Unknown" gets priority
    .ThenBy(i => i.Name)                        // Then sort the rest alphabetically
    .ToList();
        }
    }
}
