using Microsoft.EntityFrameworkCore;
using System.IO;
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
            textBlock.Visibility = Visibility.Collapsed;
            img.Source = new BitmapImage(new Uri("/Map.png", UriKind.Relative));            
        }

        private void Vil1_Click(object sender, RoutedEventArgs e)
        {
            int id = 1;

            var result = GetVillage(id);

            var village = result.Keys.First();
            var location = result.Values.First();

            DisplayVillageInfo(village, location);
        }

        private void DisplayVillageInfo(Village village, Location location)
        {
            // Устанавливаем текст в `TextBox`
            Info.Text = $@"
                {village.Name}

                Расположение: {location.Region}, {location.District}
                Координаты: {location.Latitude}, {location.Longitude}

                === История ===
                {village.Description}

                === Демография ===
                Население до уничтожения: {village.PopulationBefore}

                === Разрушение деревни ===
                Дата уничтожения: {village.DateDestroyed:dd MMMM yyyy}
                Причина: {village.Cause}

                === Память ===
                Существует мемориал: {(village.MemorialExists ? "Да" : "Нет")}
                ";

            // Загружает изображение в `Image` компонент
            if (!string.IsNullOrEmpty(village.ImagePath) && File.Exists(village.ImagePath))
            {
                img.Source = new BitmapImage(new Uri(village.ImagePath, UriKind.Absolute));
                textBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                img.Source = null;
                textBlock.Text = "Нет изображения";
                textBlock.Visibility = Visibility.Visible;
            }
        }



        private Dictionary<Village, Location> GetVillage(int villageId)
        {
            using (var db = new MyDBContext())
            {
                // Join запрос потому что эффективней
                // С AsNoTracking() EF просто возвращает данные без лишних накладных расходов.
                var result = (from v in db.Villages.AsNoTracking() 
                              join l in db.Locations.AsNoTracking() on v.Location_Id equals l.Id
                              where v.Id == villageId
                              select new { Village = v, Location = l })
                            .FirstOrDefault();

                return result != null
                    ? new Dictionary<Village, Location> { { result.Village, result.Location } }
                    : throw new Exception("Поля с таким Id нет");
            }
        }

    }
}