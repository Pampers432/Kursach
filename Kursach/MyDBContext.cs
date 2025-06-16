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


        //Создает первые записи для БД
        public static void Seed()
        {
            // Village и Location
            var location1 = new Location
            {
                Region = "Гомельская",
                District = "Жлобинский",
                Latitude = "52.8940",
                Longitude = "30.0330"
            };
            CreateLocation(location1);

            var village = new Village
            {
                Location_Id = location1.Id,
                Name = "Хатынь",
                PopulationBefore = "150",
                DateDestroyed = new DateTime(1943, 3, 22),
                Cause = "Карательная операция фашистов",
                MemorialExists = true,
                ImagePath = null,
                Description = "Деревня, уничтоженная в годы Второй мировой войны"
            };
            CreateVillage(village);

            // Monument и Location
            var location2 = new Location
            {
                Region = "Минская",
                District = "Логойский",
                Latitude = "54.0364",
                Longitude = "27.7280"
            };
            CreateLocation(location2);

            var monument = new Monument
            {
                Location_Id = location2.Id,
                Name = "Мемориал Хатынь",
                InstallationDate = new DateTime(1969, 7, 5),
                Cause = "В память о погибших жителях деревни",
                ImagePath = null,
                Description = "Известный мемориальный комплекс"
            };
            CreateMonument(monument);

            // Ghetto и Location
            var location3 = new Location
            {
                Region = "Витебская",
                District = "Полоцкий",
                Latitude = "55.4872",
                Longitude = "28.7855"
            };
            CreateLocation(location3);

            var ghetto = new Ghetto
            {
                Location_Id = location3.Id,
                Name = "Полоцкое гетто",
                EstablishedDate = new DateTime(1941, 7, 10),
                LiquidationDate = new DateTime(1942, 2, 28),
                VictimsCount = 12000,
                Population = 14000,
                ImagePath = null,
                Description = "Место массового заключения и уничтожения евреев"
            };
            CreateGhetto(ghetto);

            // MassGrave и Location
            var location4 = new Location
            {
                Region = "Брестская",
                District = "Каменецкий",
                Latitude = "52.3370",
                Longitude = "23.8125"
            };
            CreateLocation(location4);

            var massGrave = new MassGrave
            {
                Location_Id = location4.Id,
                Name = "Братская могила в деревне",
                InstallationDate = new DateTime(1945, 5, 9),
                Cause = "Массовое убийство мирных жителей",
                VictimsCount = 400,
                ImagePath = null,
                Description = "Место памяти жертв фашизма"
            };
            CreateMassGrave(massGrave);

            // Victim (привязка к MassGrave)
            var victim = new Victim
            {
                MassGrave_Id = massGrave.Id,
                LastName = "Иванов",
                Name = "Петр",
                MiddleName = "Семенович",
                BirthDate = new DateTime(1905, 6, 12),
                DeathDate = new DateTime(1942, 9, 1),
                ImagePath = null,
                Description = "Один из погибших в ходе карательной операции"
            };
            CreateVictim(victim);
        }
    }
}