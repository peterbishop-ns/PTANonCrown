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


        public async Task<List<Ecosite>> GetEcositeLookups()
        {
            AppLoggerData.Log("GetEcositeLookups", "LookupRepository");
            var context = _databaseService.GetContext();
            var query = context.Ecosite;
            return await query.ToListAsync();
        }




        public async Task<List<EcositeSoilVeg>> GetEcositeSoilVegLookups()
        {
            AppLoggerData.Log("GetEcositeSoilVegLookups", "LookupRepository");
            var context = _databaseService.GetContext();
            var query = context.EcositeSoilVeg;
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
