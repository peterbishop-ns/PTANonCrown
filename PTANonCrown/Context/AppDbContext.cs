using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using System.IO;
using Microsoft.Maui.Storage; // For FileSystem.AppDataDirectory
using PTANonCrown.Models;

namespace PTANonCrown.Context;

public class AppDbContext : DbContext
{
    public DbSet<Stand> Stands { get; set; } 
    public DbSet<Plot> Plots{ get; set; } 
    public DbSet<CoarseWoody> CoarseWoodys { get; set; } 
    public DbSet<TreeDead> TreesDead { get; set; } 
    public DbSet<TreeLive> TreesLive { get; set; } 

    public AppDbContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}