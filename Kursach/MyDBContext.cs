using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Kursach
{
    public class MyDBContext : DbContext
    {
        public DbSet<Village> Villages { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Monument> Monuments { get; set; }
        public DbSet<Ghetto> Ghettos { get; set; }
        public DbSet<MassGrave> MassGraves { get; set; }
        public DbSet<Victim> Victims { get; set; }

        private const string DatabaseName = "KursachDB.db";
        private static string DatabasePath => Path.Combine(Directory.GetCurrentDirectory(), DatabaseName);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={DatabasePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Отключает каскадное удаление по умолчанию
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        // Для создания БД и таблицы, если их нет
        public static void InitializeDatabase()
        {
            using (var db = new MyDBContext())
            {
                db.Database.EnsureCreated(); 
            }
        }


        public static async Task CreateTablesAsync(Location location, Village village, Monument monument,
                                                   Ghetto ghetto, MassGrave massGrave, Victim victim)
        {
            await CreateLocationAsync(location);
            await CreateVillageAsync(village);
            await CreateMonumentAsync(monument);
            await CreateGhettoAsync(ghetto);
            await CreateMassGraveAsync(massGrave);
            await CreateVictimAsync(victim);
        }

        public static async Task CreateLocationAsync(Location location)
        {
            using (var db = new MyDBContext())
            {
                await db.Locations.AddAsync(location);
                await db.SaveChangesAsync();
            }
        }

        public static async Task CreateVillageAsync(Village village)
        {
            using (var db = new MyDBContext())
            {
                await db.Villages.AddAsync(village);
                await db.SaveChangesAsync();
            }
        }

        public static async Task CreateMonumentAsync(Monument monument)
        {
            using (var db = new MyDBContext())
            {
                await db.Monuments.AddAsync(monument);
                await db.SaveChangesAsync();
            }
        }

        public static async Task CreateGhettoAsync(Ghetto ghetto)
        {
            using (var db = new MyDBContext())
            {
                await db.Ghettos.AddAsync(ghetto);
                await db.SaveChangesAsync();
            }
        }

        public static async Task CreateMassGraveAsync(MassGrave massGrave)
        {
            using (var db = new MyDBContext())
            {
                await db.MassGraves.AddAsync(massGrave);
                await db.SaveChangesAsync();
            }
        }

        public static async Task CreateVictimAsync(Victim victim)
        {
            using (var db = new MyDBContext())
            {
                await db.Victims.AddAsync(victim);
                await db.SaveChangesAsync();
            }
        }
    }
}