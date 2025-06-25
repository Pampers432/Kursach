using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        List<Image> AllImgs;
 
        public MainWindow()
        {
            InitializeComponent();
            MyDBContext.InitializeDatabase();

            Villages = new List<Image>
            {
                Хатынь
            };

            Ghettos = new List<Image>
            {
                ПолоцкоеГетто
            };

            MassGraves = new List<Image>
            {
                БратскаяМогилаПриКаменецке
            };
            Monuments = new List<Image>
            {
                МемориалХатынь
            };

            AllImgs = Villages
                        .Concat(Ghettos)
                        .Concat(MassGraves)
                        .Concat(Monuments)
                        .ToList();
        }

        // Кнопка очистки
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchField.Text = "Поиск";

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
            string pattern = SearchField.Text; 
            if (string.IsNullOrEmpty(pattern))
            {
                return;
            }

            foreach (Image img in AllImgs)
            {
                if (!img.Name.StartsWith(pattern))
                {
                    img.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchField.Text == "Поиск")
            {
                SearchField.Text = "";
                SearchField.Foreground = Brushes.Black;
            }
        }

        private void Search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchField.Text))
            {
                SearchField.Text = "Поиск";
                SearchField.Foreground = Brushes.Gray;
            }
        }

        /// <summary>
        /// Функционал
        /// </summary>
        private void Хатынь_Click(object sender, MouseButtonEventArgs e)
        {
            int id = 1;
            MessageBox.Show("Картинка кликнута!");
            MoveToDisplay(id, "Деревня");            
        }

        private void МемориалХатынь_Click(object sender, MouseButtonEventArgs e)
        {
            int id = 1;
            MessageBox.Show("Картинка кликнута!");
        }

        private void ПолоцкоеГетто_Click(object sender, MouseButtonEventArgs e)
        {
            int id = 1;
            MessageBox.Show("Картинка кликнута!");
        }

        private void БратскаяМогилаПриКаменецке_Click(object sender, MouseButtonEventArgs e)
        {
            int id = 1;
            MessageBox.Show("Картинка кликнута!");
            
        }

        private void MoveToDisplay(int id, string tableName)
        {
            Display display = new Display(id, tableName);
            //Добавить логику занесения данных из бд на форму
            display.ShowDialog();

            if (display.IsReturningToMain)
                Show(); 
            else
                Close();
        }
    }
}