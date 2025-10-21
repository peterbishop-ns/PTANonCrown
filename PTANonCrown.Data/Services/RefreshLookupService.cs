using PTANonCrown.Data.Context;
using PTANonCrown.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PTANonCrown.Data.Services
{
    public class LookupRefreshService
    {
        private readonly AppDbContext _db;

        public LookupRefreshService(AppDbContext db)
        {
            _db = db;
        }

        public async Task RefreshLookupsAsync()
        {
            await RefreshTableAsync<Soil>("Soil.csv", s => s.ShortCode);
            await RefreshTableAsync<Vegetation>("Vegetation.csv", v => v.ShortCode);
            await RefreshTableAsync<Ecodistrict>("Ecodistrict.csv", e => e.ShortCode);

            await RefreshJunctionAsync("EcodistrictSoilVeg.csv");

            // --- ensure all changes are written ---
            _db.SaveChangesAsync(); // commit any pending inserts/updates
            _db.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint(FULL);"); // flush WAL to app.db
        }

        private async Task RefreshTableAsync<T>(string csvPath, Func<T, string> keySelector) where T : class
        {
            var records = CsvLoader.LoadCsv<T>(csvPath);

            foreach (var rec in records)
            {
                var key = keySelector(rec);
                var existing = await _db.Set<T>().FindAsync(key);
                if (existing == null)
                    await _db.Set<T>().AddAsync(rec);
                else
                    _db.Entry(existing).CurrentValues.SetValues(rec); // update name if changed
            }

            await _db.SaveChangesAsync();
        }

        private async Task RefreshJunctionAsync(string csvPath)
        {
            var records = CsvLoader.LoadCsv<EcodistrictSoilVeg>(csvPath);

            foreach (var rec in records)
            {
                // check if combination exists
                var existing = await _db.EcodistrictSoilVeg
                    .FirstOrDefaultAsync(ev => ev.SoilCode == rec.SoilCode && ev.VegCode == rec.VegCode);

                if (existing == null)
                {
                    await _db.EcodistrictSoilVeg.AddAsync(rec);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    existing.EcodistrictCode = rec.EcodistrictCode; // update ecodistrict if changed
                    await _db.SaveChangesAsync();
                }
            }

            //await _db.SaveChangesAsync();
        }
    }
}
