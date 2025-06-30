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


        public static void CreateTables(Location location, Village village, Monument monument, Ghetto ghetto, MassGrave massGrave, Victim victim)
        {
            CreateLocation(location);
            CreateVillage(village);
            CreateMonument(monument);
            CreateGhetto(ghetto);
            CreateMassGrave(massGrave);
            CreateVictim(victim);
        }

        public static void CreateLocation(Location location)
        {
            using (MyDBContext db = new MyDBContext())
            {
                db.Locations.Add(location);
                db.SaveChanges();
            }
        }

        public static void CreateVillage(Village village)
        {
            using (MyDBContext db = new MyDBContext())
            {
                db.Villages.Add(village);
                db.SaveChanges();
            }
        }

        public static void CreateMonument(Monument monument)
        {
            using (MyDBContext db = new MyDBContext())
            {
                db.Monuments.Add(monument);
                db.SaveChanges();
            }
        }

        public static void CreateGhetto(Ghetto ghetto)
        {
            using (MyDBContext db = new MyDBContext())
            {
                db.Ghettos.Add(ghetto);
                db.SaveChanges();
            }
        }

        public static void CreateMassGrave(MassGrave massGrave)
        {
            using (MyDBContext db = new MyDBContext())
            {
                db.MassGraves.Add(massGrave);
                db.SaveChanges();
            }
        }

        public static void CreateVictim(Victim victim)
        {
            using (MyDBContext db = new MyDBContext())
            {
                db.Victims.Add(victim);
                db.SaveChanges();
            }
        }        
    }
}