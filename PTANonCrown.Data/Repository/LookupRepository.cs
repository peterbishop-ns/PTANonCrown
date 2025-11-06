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
        public LookupRepository(DatabaseService databaseService) : base(databaseService)
        {

        }

        public List<Soil?> GetSoilLookups()
        {
            AppLoggerData.Log("GetSoilLookups", "LookupRepository");
            using var context = _databaseService.GetContext();

            var soils = context.Soil
                .OrderBy(s => s.SoilType)
                .ThenBy(s => s.ShortCode)
                .ToList<Soil?>();


            return soils;
        }
        public List<Ecodistrict> GetEcodistrictLookups()
        {
            AppLoggerData.Log("GetEcodistrictLookups", "LookupRepository");
            using var context = _databaseService.GetContext();
            var ecodistricts = context.Ecodistrict.ToList<Ecodistrict>();
            return ecodistricts;
        }




                public List<EcodistrictSoilVeg> GetEcodistrictSoilVegLookups()
        {
            AppLoggerData.Log("GetEcodistrictSoilVegLookups", "LookupRepository");
            using var context = _databaseService.GetContext();
            return context.EcodistrictSoilVeg.ToList();
        }


        public List<Vegetation> GetVegLookups()
        {
            AppLoggerData.Log("GetVegLookups", "LookupRepository");
            using var context = _databaseService.GetContext();
            var vegs = context.Vegetation
             .OrderBy(s => s.ShortCode)
             .ToList<Vegetation?>();

            return vegs;
        }     
        

        


    }
}
