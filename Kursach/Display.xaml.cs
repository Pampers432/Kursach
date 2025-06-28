using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsReturningToMain)
            {
                Application.Current.Shutdown();
            }
        }

        // Выход
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Вернуться на главную
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            IsReturningToMain = true;
            Close();
        }

        public Display(int id, string tableName)
        {
            InitializeComponent();
            

            this.id = id;
            this.tableName = tableName;

            img.Source = null;
            Info.Text = "";
            textBlock.Visibility = Visibility.Collapsed;


            if (tableName == "Монумент") DisplayMonumentInfo(id);
            else if (tableName == "Деревня") DisplayVillageInfo(id);
            else if (tableName == "Гетто") DisplayGhettoInfo(id);
            else if (tableName == "Могила") DisplayMassGraveInfo(id);
        }

        private void SetImgSource(string Path)
        {
            if (string.IsNullOrEmpty(Path))
            {
                img.Source = null;
                textBlock.Text = "Нет изображения";
                textBlock.Visibility = Visibility.Visible;
                return;
            }

            string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.TrimStart('\\', '/'));

            if (File.Exists(fullPath))
            {
                img.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                textBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                img.Source = null;
                textBlock.Text = "Изображение не найдено";
                textBlock.Visibility = Visibility.Visible;
            }
        }

        // <summary>
        // Получение информаци о деревне
        // <summary>
        private void DisplayVillageInfo(int id)
        {
            var result = GetVillageFromDb(id);

            var village = result.Keys.First();
            var location = result.Values.First();

            GetVillageInfo(village, location);
        }

        private void GetVillageInfo(Village village, Location location)
        {
            // Устанавливает текст в `TextBox`
            Info.Text = $@"                            Деревня {village.Name}

                Расположение: {location.Region} область, {location.District} округ (координаты: {location.Latitude}, {location.Longitude}) 
                До уничтожения в деревне проживало {village.PopulationBefore} человек. 
                Причина уничтожения: {village.Cause}

                Описание: {village.Description} {(village.MemorialExists ? "На месте трагедии установлен мемориальный комплекс." : "Мемориал на месте событий отсутствует.")}
";


            // Загружает изображение в `Image` компонент
            string relativePath = village.ImagePath;
            SetImgSource(relativePath);
        }

        private Dictionary<Village, Location> GetVillageFromDb(int villageId)
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

        // <summary>
        // Получение информаци о монументе
        // <summary>
        private void DisplayMonumentInfo(int id)
        {
            var result = GetMonumentFromDb(id);

            var monument = result.Keys.First();
            var location = result.Values.First();

            SetMonumentInfo(monument, location);
        }

        private void SetMonumentInfo(Monument monument, Location location)
        {
            // Устанавливает текст в `TextBox`
            Info.Text = $@"                            {monument.Name}

                Расположение: {location.Region} область, {location.District} округ (координаты: {location.Latitude}, {location.Longitude}).

                Описание: {monument.Description}
";


            // Загружает изображение в `Image` компонент
            SetImgSource(monument.ImagePath);
        }

        private Dictionary<Monument, Location> GetMonumentFromDb(int monumentId)
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

        // <summary>
        // Получение информаци о Монументе
        // <summary>
        private void DisplayGhettoInfo(int id)
        {
            var result = GetGhettoFromDb(id);
            var ghetto = result.Keys.First();
            var location = result.Values.First();

            SetGhettoInfo(ghetto, location);
        }
        
        private void SetGhettoInfo(Ghetto ghetto, Location location)
        {
            Info.Text = $@"                            {ghetto.Name}

                Расположение: {location.Region} область, {location.District} округ (координаты: {location.Latitude}, {location.Longitude}).
                Период существования: {ghetto.EstablishedDate} - {ghetto.LiquidationDate}
                Общее количество узников составляет около: {ghetto.Population} Примерное количество жертв: {ghetto.VictimsCount}

                Описание: {ghetto.Description}
    ";

            SetImgSource(ghetto.ImagePath);
        }
        private Dictionary<Ghetto, Location> GetGhettoFromDb(int ghettoId)
        {
            using (var db = new MyDBContext())
            {
                var result = (from g in db.Ghettos.AsNoTracking()
                              join l in db.Locations.AsNoTracking() on g.Location_Id equals l.Id
                              where g.Id == ghettoId
                              select new { Ghetto = g, Location = l })
                           .FirstOrDefault();

                if (result == null)
                    throw new Exception("Гетто с таким Id не найдено");

                return new Dictionary<Ghetto, Location> { { result.Ghetto, result.Location } };
            }
        }


        // <summary>
        // Получение информаци о Братской могиле
        // <summary>
        private void DisplayMassGraveInfo(int id)
        {
            var result = GetMassGrave(id);
            var massGrave = result.Keys.First();
            var location = result.Values.First();

            SetMassGraveInfo(massGrave, location);
        }

        private void SetMassGraveInfo(MassGrave massGrave, Location location)
        {
            Info.Text = $@"                            {massGrave.Name}

                Расположение: {location.Region} область, {location.District} округ (координаты: {location.Latitude}, {location.Longitude}).
                Дата создания: {massGrave.InstallationDate} Примерное количество жертв: {massGrave.VictimsCount}

                Описание: {massGrave.Description}
    ";

            SetImgSource(massGrave.ImagePath);
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
