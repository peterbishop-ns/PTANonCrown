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

        public List<Soil?> GetSoilLookups()
        {
            AppLogger.Log("GetSoilLookups", "LookupRepository");

            var soils = _context.Soil
                .OrderBy(s => s.SoilType)
                .ThenBy(s => s.ShortCode)
                .ToList<Soil?>();

            soils.Insert(0, new Soil() { ShortCode = null, SoilType = -1, Name = null, SoilPhaseShort = null});  // prepend a null entry

            return soils;
        }
        public List<Ecodistrict> GetEcodistrictLookups()
        {
            AppLogger.Log("GetEcodistrictLookups", "LookupRepository");

            return _context.Ecodistrict.ToList();
        }
                public List<EcodistrictSoilVeg> GetEcodistrictSoilVegLookups()
        {
            AppLogger.Log("GetEcodistrictSoilVegLookups", "LookupRepository");

            return _context.EcodistrictSoilVeg.ToList();
        }


        public List<Vegetation> GetVegLookups()
        {
            AppLogger.Log("GetVegLookups", "LookupRepository");

            var vegs = _context.Vegetation
             .OrderBy(s => s.ShortCode)
             .ToList<Vegetation?>();

            vegs.Insert(0, new Vegetation() { ShortCode = null, Name = null });  // prepend a null entry

            return vegs;
        }     
        

        


    }
}
