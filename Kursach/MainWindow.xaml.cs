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
        public bool VillageVisibility = true;
        public bool GhettoVisibility = true;
        public bool MonumentVisibility = true;
        public bool MassGraveVisibility = true;

        List<Image> Villages;
        List<Image> Ghettos;
        List<Image> MassGraves;
        List<Image> Monuments;

        public MainWindow()
        {
            InitializeComponent();
            MyDBContext.InitializeDatabase();

            Villages = new List<Image>
            {
                Хатынь
            };
            Ghettos = new List<Image>();
            MassGraves = new List<Image>();
            Monuments = new List<Image>();
        }

        // Кнопка очистки
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Search.Text = "Поиск";

            Village_Img.Visibility = Visibility.Visible;
            Ghetto_Img.Visibility = Visibility.Visible;
            MassGrave_Img.Visibility = Visibility.Visible;
            Monument_Img.Visibility = Visibility.Visible;

            SetAllGroupsVisible();
        }

        private void SetAllGroupsVisible()
        {
            ChangeVisibility(Villages, true);
            ChangeVisibility(Ghettos, true);
            ChangeVisibility(MassGraves, true);
            ChangeVisibility(Monuments, true);

            VillageVisibility = true;
            GhettoVisibility = true;
            MassGraveVisibility = true;
            MonumentVisibility = true;
        }

        public void ChangeVisibility(List<Image> images, bool VisibilityP)
        {
            if (VisibilityP)
                foreach (Image image in images)
                    image.Visibility = Visibility.Visible;

            else
                foreach (Image image in images)
                    image.Visibility = Visibility.Collapsed;
        }

        private void ToggleVisibility(Image icon, List<Image> group, ref bool visibilityFlag)
        {
            icon.Visibility = icon.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            visibilityFlag = icon.Visibility == Visibility.Visible;
            ChangeVisibility(group, visibilityFlag);
        }

        private void Village_Click(object sender, RoutedEventArgs e) =>
            ToggleVisibility(Village_Img, Villages, ref VillageVisibility);

        private void Ghetto_Click(object sender, RoutedEventArgs e) =>
            ToggleVisibility(Ghetto_Img, Ghettos, ref GhettoVisibility);

        private void MassGrave_Click(object sender, RoutedEventArgs e) =>
            ToggleVisibility(MassGrave_Img, MassGraves, ref MassGraveVisibility);

        private void Monument_Click(object sender, RoutedEventArgs e) =>
            ToggleVisibility(Monument_Img, Monuments, ref MonumentVisibility);

        private void SearchIcon_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Клик по значку выполнен!");
        }


        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Search.Text == "Поиск")
            {
                Search.Text = "";
                Search.Foreground = Brushes.Black;
            }
        }

        private void Search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Search.Text))
            {
                Search.Text = "Поиск";
                Search.Foreground = Brushes.Gray;
            }
        }


        private void Vil1_Click(object sender, RoutedEventArgs e)
        {
            int id = 1;

            //var result = GetVillage(id);

            //var village = result.Keys.First();
            //var location = result.Values.First();

            //DisplayVillageInfo(village, location);
        }

        //private void DisplayVillageInfo(Village village, Location location)
        //{
        //    // Устанавливаем текст в `TextBox`
        //    Info.Text = $@"
        //        Деревня {village.Name}

        //        Расположенная в {location.Region}, {location.District} (координаты: {location.Latitude}, {location.Longitude}), до трагических событий насчитывала {village.PopulationBefore} жителей. {village.DateDestroyed:dd MMMM yyyy} года поселение было уничтожено в результате {village.Cause.ToLower()}. {(village.MemorialExists ? "На месте трагедии установлен мемориальный комплекс." : "Мемориал на месте событий отсутствует.")}
        //        ";

        //    // Загружает изображение в `Image` компонент
        //    if (!string.IsNullOrEmpty(village.ImagePath) && File.Exists(village.ImagePath))
        //    {
        //        img.Source = new BitmapImage(new Uri(village.ImagePath, UriKind.Absolute));
        //        textBlock.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        img.Source = null;
        //        textBlock.Text = "Нет изображения";
        //        textBlock.Visibility = Visibility.Visible;
        //    }
        //}



        //private Dictionary<Village, Location> GetVillage(int villageId)
        //{
        //    using (var db = new MyDBContext())
        //    {
        //        // Join запрос потому что эффективней
        //        // С AsNoTracking() EF просто возвращает данные без лишних накладных расходов.
        //        var result = (from v in db.Villages.AsNoTracking() 
        //                      join l in db.Locations.AsNoTracking() on v.Location_Id equals l.Id
        //                      where v.Id == villageId
        //                      select new { Village = v, Location = l })
        //                    .FirstOrDefault();

        //        return result != null
        //            ? new Dictionary<Village, Location> { { result.Village, result.Location } }
        //            : throw new Exception("Поля с таким Id нет");
        //    }
        //}

    }
}