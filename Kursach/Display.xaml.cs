using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Display : Window
    {
        public bool IsReturningToMain { get; private set; } = false;
        int id;
        string tableName;

        public Display(int id, string tableName)
        {
            InitializeComponent();
            this.id = id;
            this.tableName = tableName;

            Vil1_Click(id);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsReturningToMain)
            {
                Application.Current.Shutdown();
            }
        }


        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            IsReturningToMain = true;
            Close();
        }

        private void Vil1_Click(int id)
        {
            var result = GetVillage(id);

            var village = result.Keys.First();
            var location = result.Values.First();

            DisplayVillageInfo(village, location);
        }

        private void DisplayVillageInfo(Village village, Location location)
        {
            // Устанавливаем текст в `TextBox`
            Info.Text = $@"
                Деревня {village.Name}

                Расположенная в {location.Region}, {location.District} (координаты: {location.Latitude}, {location.Longitude}), до трагических событий насчитывала {village.PopulationBefore} жителей. {village.DateDestroyed:dd MMMM yyyy} года поселение было уничтожено в результате {village.Cause.ToLower()}. {(village.MemorialExists ? "На месте трагедии установлен мемориальный комплекс." : "Мемориал на месте событий отсутствует.")}
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

        private Dictionary<Monument, Location> GetMonument(int monumentId)
        {
            var result = new Dictionary<Monument, Location>();
            return result;
        }

        private Dictionary<Ghetto, Location> GetGhetto(int ghettoId)
        {
            var result = new Dictionary<Ghetto, Location>();
            return result;
        }

        private Dictionary<MassGrave, Location> GetMassGrave(int massGraveId)
        {
            var result = new Dictionary<MassGrave, Location>();
            return result;
        }
    }
}
