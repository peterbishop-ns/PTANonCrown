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

            RefreshJunctionAsync("EcodistrictSoilVeg.csv");

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
            // Load CSV
            var records = CsvLoader.LoadCsv<EcodistrictSoilVeg>(csvPath)
                .Where(r => !string.IsNullOrWhiteSpace(r.SoilCode)
                         && !string.IsNullOrWhiteSpace(r.VegCode)
                         && !string.IsNullOrWhiteSpace(r.EcositeGroup))
                .ToList();

            // Wipe existing data
            var allExisting = _db.EcodistrictSoilVeg.ToList();
            _db.EcodistrictSoilVeg.RemoveRange(allExisting);
            await _db.SaveChangesAsync();

            // Add all records
            await _db.EcodistrictSoilVeg.AddRangeAsync(records);

            // Commit
            await _db.SaveChangesAsync();
        }
    }
}
