using Microsoft.EntityFrameworkCore;
using PTANonCrown.Data.Context;
using PTANonCrown.Data.Models;
using PTANonCrown.Data.Services;
using System.Collections.ObjectModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<List<Soil>> GetSoilLookupsAsync()
        {
            AppLoggerData.Log("GetSoilLookupsAsync", "LookupRepository");

            var context = _databaseService.GetContext();

            var query = context.Soil
                .OrderBy(s => s.SoilType)
                .ThenBy(s => s.ShortCode);

            return await query.ToListAsync();
        }


        public async Task<List<Ecodistrict>> GetEcodistrictLookups()
        {
            AppLoggerData.Log("GetEcodistrictLookups", "LookupRepository");
            var context = _databaseService.GetContext();
            var query = context.Ecodistrict;
            return await query.ToListAsync();
        }




        public async Task<List<EcodistrictSoilVeg>> GetEcodistrictSoilVegLookups()
        {
            AppLoggerData.Log("GetEcodistrictSoilVegLookups", "LookupRepository");
            var context = _databaseService.GetContext();
            var query = context.EcodistrictSoilVeg;
            return await query.ToListAsync();
        }


        public async Task<List<Vegetation>> GetVegLookups()
        {
            AppLoggerData.Log("GetVegLookups", "LookupRepository");
            var context = _databaseService.GetContext();
            var query = context.Vegetation
             .OrderBy(s => s.ShortCode);

            return await query.ToListAsync();
        }





    }
}
