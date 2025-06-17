using PTANonCrown.Data.Context;
using PTANonCrown.Data.Models;
using PTANonCrown.Data.Services;

namespace PTANonCrown.Data.Repository
{
    public interface ILookupRepository : IBaseRepository<BaseLookup>
    {
    }

    public class LookupRepository : BaseRepository<BaseLookup>, ILookupRepository
    {
        private readonly AppDbContext _context;

        public LookupRepository(AppDbContext context) : base(context)
        {
            AppLogger.Log("LookupRepository", "LookupRepository");

            _context = context;
        }

        public List<SoilLookup> GetSoilLookups()
        {
            AppLogger.Log("GetSoilLookups", "LookupRepository");

            return _context.SoilLookup.ToList();
        }
        public List<EcodistrictLookup> GetEcodistrictLookups()
        {
            AppLogger.Log("GetEcodistrictLookups", "LookupRepository");

            return _context.EcodistrictLookup.ToList();
        }


        public List<VegLookup> GetVegLookups()
        {
            AppLogger.Log("GetVegLookups", "LookupRepository");

            return _context.VegLookup.ToList();
        }     
        
        public List<ExposureLookup> GetExposureLookups()
        {
            AppLogger.Log("GetExposureLookups", "LookupRepository");

            return _context.ExposureLookup.ToList();
        }
        


    }
}
