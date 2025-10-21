using PTANonCrown.Data.Context;
using PTANonCrown.Data.Models;
using PTANonCrown.Data.Services;
using System.Collections.ObjectModel;

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
            try
            {
                AppLogger.Log("LookupRepository", "LookupRepository constructor start");
                _context = context;
                AppLogger.Log("LookupRepository", "Context assigned");
            }
            catch (Exception ex)
            {
                AppLogger.Log("LookupRepository", $"Exception: {ex.Message}");
                throw;
            }
        }

        public List<Soil> GetSoilLookups()
        {
            AppLogger.Log("GetSoilLookups", "LookupRepository");

            return _context.Soil.ToList();
        }
        public List<Ecodistrict> GetEcodistrictLookups()
        {
            AppLogger.Log("GetEcodistrictLookups", "LookupRepository");

            return _context.Ecodistrict.ToList();
        }


        public List<Vegetation> GetVegLookups()
        {
            AppLogger.Log("GetVegLookups", "LookupRepository");

            return _context.Vegetation.ToList();
        }     
        
        public List<Exposure> GetExposureLookups()
        {
            AppLogger.Log("GetExposureLookups", "LookupRepository");

            return _context.Exposure.ToList();
        }
        


    }
}
