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
        private readonly DatabaseService _databaseService;

        public LookupRefreshService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }


        public async Task RefreshLookupsAsync()
        {
            // Get the shared AppDbContext from DatabaseService
            var db = _databaseService.GetContext();

            await RefreshTableAsync<Soil>(db, "Soil.csv");
           await RefreshTableAsync<Vegetation>(db, "Vegetation.csv");
           await RefreshTableAsync<Ecodistrict>(db, "Ecodistrict.csv");
           await RefreshTableAsync<TreeSpecies>(db, "TreeSpecies.csv");
           await RefreshTableAsync<Treatment>(db, "Treatments.csv");

           await RefreshJunctionAsync(db, "EcodistrictSoilVeg.csv");

            // --- ensure all changes are written ---
            await db.SaveChangesAsync(); // commit any pending inserts/updates
            await db.Database.ExecuteSqlRawAsync("PRAGMA wal_checkpoint(FULL);"); // flush WAL to app.db

            
        }

        private async Task RefreshTableAsync<T>(
            AppDbContext db,
            string csvPath) where T : class
        {
            var records = CsvLoader.LoadCsv<T>(csvPath);
            var dbSet = db.Set<T>();

            dbSet.RemoveRange(dbSet);
            await dbSet.AddRangeAsync(records);
            await db.SaveChangesAsync();
        }

        private async Task RefreshJunctionAsync(AppDbContext db,
                string csvPath)
        {
            // Load CSV
            var records = CsvLoader.LoadCsv<EcodistrictSoilVeg>(csvPath)
                .Where(r => !string.IsNullOrWhiteSpace(r.SoilCode)
                         && !string.IsNullOrWhiteSpace(r.VegCode)
                         && !string.IsNullOrWhiteSpace(r.EcositeGroup))
                .ToList();

            foreach (var rec in records)
            {
                var existing = await db.EcodistrictSoilVeg.FindAsync(rec.SoilCode, rec.VegCode, rec.EcositeGroup);
                if (existing == null)
                    await db.EcodistrictSoilVeg.AddAsync(rec);
                else
                    db.Entry(existing).CurrentValues.SetValues(rec);
            }

            await db.SaveChangesAsync();


        }
    }
}
