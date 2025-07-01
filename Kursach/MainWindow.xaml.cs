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

            // В конструкторе:
            Villages = new List<Image>
            {
                Хатынь, Багута, Батуринка, Водица, Заболотье, Туры, Сухой_Остров, Терюха
            };
            Monuments = new List<Image> { Мемориал_Хатынь, Мемориал_Батуринка, Мемориал_Водица, Мемориал_Заболотье, Мемориал_Туры };
            MassGraves = new List<Image> 
            {
                Братская_Могила_При_Каменецке,
                Братская_Могила_Багута,
                Братская_Могила_Батуринка,
                Братская_Могила_Водица,
                Братская_Могила_Заболотье,
                Братская_Могила_Туры,
                Братская_Могила_Сухой_Остров,
                Братская_Могила_Терюха
            };

            Ghettos = new List<Image>
            {
                Полоцкое_Гетто,
                Минское_Гетто,  
                Брестское_Гетто,  
                Смолевичское_Гетто,
                ДавидГородокское_Гетто 
            };

            AllImgs = Villages
                        .Concat(Ghettos)
                        .Concat(MassGraves)
                        .Concat(Monuments)
                        .ToList();
        }
        
        // Выход
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            visibilityFlag = !visibilityFlag;
            icon.Opacity = visibilityFlag ? 1.0 : 0.5;

            ChangeVisibility(group, visibilityFlag);
        }



        // Фильтры
        private void Village_Click(object sender, RoutedEventArgs e) =>
            ToggleVisibility(Village_Img, Villages, ref VillageVisibility);

        private void Ghetto_Click(object sender, RoutedEventArgs e) =>
            ToggleVisibility(Ghetto_Img, Ghettos, ref GhettoVisibility);

        private void MassGrave_Click(object sender, RoutedEventArgs e) =>
            ToggleVisibility(MassGrave_Img, MassGraves, ref MassGraveVisibility);

        private void Monument_Click(object sender, RoutedEventArgs e) =>
            ToggleVisibility(Monument_Img, Monuments, ref MonumentVisibility);


        // Поиск
        private void SearchIcon_Click(object sender, RoutedEventArgs e)
        {
            string pattern = SearchField.Text; 
            if (string.IsNullOrEmpty(pattern) || pattern == "Поиск")
            {
                return;
            }

            foreach (Image img in AllImgs)
            {
                if (!img.Name.Contains(pattern) && !img.Name.ToLower().Contains(pattern.ToLower()))
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
    
        // Деревни
        private void Хатынь_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(1, "Деревня");            
        }

        private void Багута_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(2, "Деревня");
        }

        private void Батуринка_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(3, "Деревня");
        }

        private void Водица_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(4, "Деревня");
        }

        private void Заболотье_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(5, "Деревня"); 
        }

        private void Туры_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(6, "Деревня");

        private void СухойОстров_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(7, "Деревня");

        private void Терюха_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(8, "Деревня");

        private void Романовичи_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(9, "Деревня");

        private void Прудок_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(12, "Деревня");

        // Мемориалы
        private void МемориалХатынь_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(1, "Монумент");
        }

        private void МемориалБатуринка_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(2, "Монумент");
        }

        private void МемориалВодица_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(3, "Монумент"); 
        }

        private void МемориалЗаболотье_Click(object sender, MouseButtonEventArgs e)
        {    
            MoveToDisplay(4, "Монумент"); 
        }

        private void МемориалТуры_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(5, "Монумент");

        // Гетто
        private void ПолоцкоеГетто_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(1, "Гетто");
        }

        private void МинскоеГетто_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(2, "Гетто");

        private void БрестскоеГетто_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(3, "Гетто");

        private void СмолевичскоеГетто_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(4, "Гетто");

        private void ДавидГородокскоеГетто_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(5, "Гетто");

        // Брасткие могилы
        private void БратскаяМогилаПриКаменецке_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(1, "Могила");
        }

        private void БратскаяМогилаБагута_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(2, "Могила");
        }

        private void БратскаяМогилаБатуринка_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(3, "Могила");
        }

        private void БратскаяМогилаВодица_Click(object sender, MouseButtonEventArgs e)
        {
            MoveToDisplay(4, "Могила"); 
        }

        private void БратскаяМогилаЗаболотье_Click(object sender, MouseButtonEventArgs e)
        { 
            MoveToDisplay(5, "Могила"); 
        }

        private void БратскаяМогилаТуры_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(6, "Могила");

        private void БратскаяМогилаСухойОстров_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(7, "Могила");

        private void БратскаяМогилаТерюха_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(8, "Могила");

        private void БратскаяМогилаРомановичи_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(9, "Могила");

        private void БратскаяМогилаПрудок_Click(object sender, MouseButtonEventArgs e) => MoveToDisplay(10, "Могила");


        // Переход между формами
        private void MoveToDisplay(int id, string tableName)
        {
            Display display = new Display(id, tableName);
            display.ShowDialog();

            if (display.IsReturningToMain)
                Show();
        }

        private void VictimsButton_Click(object sender, RoutedEventArgs e)
        {
            Victims victims = new Victims();
            victims.ShowDialog();

            if (victims.IsReturningToMain)
                Show();
        }
    }
}