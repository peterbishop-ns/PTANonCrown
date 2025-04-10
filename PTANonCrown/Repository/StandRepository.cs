using Microsoft.EntityFrameworkCore;
using PTANonCrown.Context;
using PTANonCrown.Models;

namespace PTANonCrown.Repository
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
                    .ThenInclude(p => p.PlotTreeLive);
            // .ThenInclude(t => t.TreeLookup);
            return query.ToList();
        }
    }
}