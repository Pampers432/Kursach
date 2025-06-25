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

            if (tableName == "Деревня") Vil1_Click(id);
            //Добавить логику
            //Создать методы для обработки данных
            //Добавить логику
            //else if (tableName == "Гетто")
            //else if (tableName == "Могила")
            //else if (tableName == "Монумент")
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
            // Устанавливает текст в `TextBox`
            Info.Text = $@"
                Деревня {village.Name}

                Расположение: {location.Region} область, {location.District} округ (координаты: {location.Latitude}, {location.Longitude}) До уничтожения в деревне проживало {village.PopulationBefore} человек. Причина уничтожения: {village.Cause}

                Описание: {village.Description} {(village.MemorialExists ? "На месте трагедии установлен мемориальный комплекс." : "Мемориал на месте событий отсутствует.")}
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
            using (var db = new MyDBContext())
            {
                var result = (from m in db.Monuments.AsNoTracking()
                              join l in db.Locations.AsNoTracking() on m.Location_Id equals l.Id
                              where m.Id == monumentId
                              select new { Monument = m, Location = l })
                             .FirstOrDefault();

                return result != null
                    ? new Dictionary<Monument, Location> { { result.Monument, result.Location } }
                    : throw new Exception("Памятник с таким Id не найден");
            }
        }

        private Dictionary<Ghetto, Location> GetGhetto(int ghettoId)
        {
            using (var db = new MyDBContext())
            {
                var result = (from g in db.Ghettos.AsNoTracking()
                              join l in db.Locations.AsNoTracking() on g.Location_Id equals l.Id
                              where g.Id == ghettoId
                              select new { Ghetto = g, Location = l })
                             .FirstOrDefault();

                return result != null
                    ? new Dictionary<Ghetto, Location> { { result.Ghetto, result.Location } }
                    : throw new Exception("Гетто с таким Id не найдено");
            }
        }

        private Dictionary<MassGrave, Location> GetMassGrave(int massGraveId)
        {
            using (var db = new MyDBContext())
            {
                var result = (from mg in db.MassGraves.AsNoTracking()
                              join l in db.Locations.AsNoTracking() on mg.Location_Id equals l.Id
                              where mg.Id == massGraveId
                              select new { MassGrave = mg, Location = l })
                             .FirstOrDefault();

                return result != null
                    ? new Dictionary<MassGrave, Location> { { result.MassGrave, result.Location } }
                    : throw new Exception("Массовое захоронение с таким Id не найдено");
            }
        }
    }
}
