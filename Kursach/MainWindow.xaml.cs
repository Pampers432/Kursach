using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kursach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyDBContext.InitializeDatabase();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Info.Text = "";
        }
    }


    //Создает первые записи для БД
    public class Seeder
    {
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
            MyDBContext.CreateLocation(location1);

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
            MyDBContext.CreateVillage(village);

            // Monument и Location
            var location2 = new Location
            {
                Region = "Минская",
                District = "Логойский",
                Latitude = "54.0364",
                Longitude = "27.7280"
            };
            MyDBContext.CreateLocation(location2);

            var monument = new Monument
            {
                Location_Id = location2.Id,
                Name = "Мемориал Хатынь",
                InstallationDate = new DateTime(1969, 7, 5),
                Cause = "В память о погибших жителях деревни",
                ImagePath = null,
                Description = "Известный мемориальный комплекс"
            };
            MyDBContext.CreateMonument(monument);

            // Ghetto и Location
            var location3 = new Location
            {
                Region = "Витебская",
                District = "Полоцкий",
                Latitude = "55.4872",
                Longitude = "28.7855"
            };
            MyDBContext.CreateLocation(location3);

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
            MyDBContext.CreateGhetto(ghetto);

            // MassGrave и Location
            var location4 = new Location
            {
                Region = "Брестская",
                District = "Каменецкий",
                Latitude = "52.3370",
                Longitude = "23.8125"
            };
            MyDBContext.CreateLocation(location4);

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
            MyDBContext.CreateMassGrave(massGrave);

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
            MyDBContext.CreateVictim(victim);
        }
    }
}