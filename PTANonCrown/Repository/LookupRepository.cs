using PTANonCrown.Models;
using PTANonCrown.Context;

namespace PTANonCrown.Repository
{
    public interface ILookupRepository : IBaseRepository<TreeLookup>
    {
    }

    public class LookupRepository : BaseRepository<TreeLookup>, ILookupRepository
    {
        private readonly LookupDbContext _context;

        public LookupRepository(LookupDbContext context) : base(context)
        {
            _context = context;
        }

        public List<TreeLookup> GetTreeLookups()
        {
            return _context.TreeLookup.ToList();
        }
    }
}