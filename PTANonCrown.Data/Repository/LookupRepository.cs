using PTANonCrown.Data.Context;
using PTANonCrown.Data.Models;

namespace PTANonCrown.Data.Repository
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

        public List<SoilLookup> GetSoilLookups()
        {
            return _context.SoilLookup.ToList();
        }

     



        public List<VegLookup> GetVegLookups()
        {
            return _context.VegLookup.ToList();
        }

       

    }
}
