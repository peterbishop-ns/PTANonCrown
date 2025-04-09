using PTANonCrown.Models;
using PTANonCrown.Context;

namespace PTANonCrown.Repository
{
    public interface ILookupRepository : IBaseRepository<BaseLookup>
    {
    }

    public class LookupRepository : BaseRepository<BaseLookup>, ILookupRepository
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

        public List<SoilLookup> GetSoilLookups()
        {
            return _context.SoilLookup.ToList();
        }

        public List<VegLookup> GetVegLookups()
        {
            return _context.VegLookup.ToList();
        }        
        
        public List<TreatmentLookup> GetTreatmentLookups()
        {
            return _context.TreatmentLookup.OrderBy(i => i.Name).ToList();
        }
    }
}