using PTANonCrown.Data.Context;
using PTANonCrown.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Data.Sqlite;

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
           await RefreshTableAsync<TreeSpecies>("TreeSpecies.csv", t => t.ShortCode);
           await RefreshTableAsync<Treatment>("Treatments.csv", t => t.ShortCode);

           await RefreshJunctionAsync("EcodistrictSoilVeg.csv");

            // --- ensure all changes are written ---
            await _db.SaveChangesAsync(); // commit any pending inserts/updates
            await _db.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint(FULL);"); // flush WAL to app.db

            
        }

        private async Task RefreshTableAsync<T>(string csvPath, Func<T, string> keySelector) where T : class
        {
            var records = CsvLoader.LoadCsv<T>(csvPath);

            var dbSet = _db.Set<T>();

            // Delete all existing records
            dbSet.RemoveRange(dbSet);

            // Add all CSV records
            await dbSet.AddRangeAsync(records);

            await _db.SaveChangesAsync();
        }
        private async Task RefreshJunctionAsync(string csvPath)
        {
            // Load CSV
            var records = CsvLoader.LoadCsv<EcodistrictSoilVeg>(csvPath)
                .Where(r => !string.IsNullOrWhiteSpace(r.SoilCode)
                         && !string.IsNullOrWhiteSpace(r.VegCode)
                         && !string.IsNullOrWhiteSpace(r.EcositeGroup))
                .ToList();

            foreach (var rec in records)
            {
                var existing = await _db.EcodistrictSoilVeg.FindAsync(rec.SoilCode, rec.VegCode, rec.EcositeGroup);
                if (existing == null)
                    await _db.EcodistrictSoilVeg.AddAsync(rec);
                else
                    _db.Entry(existing).CurrentValues.SetValues(rec);
            }

            await _db.SaveChangesAsync();


        }
    }
}
