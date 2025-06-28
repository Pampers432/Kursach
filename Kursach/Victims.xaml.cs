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
using Microsoft.EntityFrameworkCore;

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для Victims.xaml
    /// </summary>
    public partial class Victims : Window
    {
        private List<Victim> allVictims;

        public bool IsReturningToMain { get; private set; } = false;

        public Victims()
        {
            InitializeComponent();

            Thread.Sleep(100);

            using (var db = new MyDBContext()) 
            {
                allVictims = db.Victims.ToList();
                VictimList.ItemsSource = allVictims
                    .Select(v => new
                    {
                        Victim = v,
                        FullName = $"{v.LastName} {v.Name} {v.MiddleName}"
                    })
                    .ToList();
            }
        }

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

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Placeholder.Visibility = Visibility.Collapsed;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Placeholder.Visibility = string.IsNullOrWhiteSpace(SearchBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(query))
            {
                VictimList.ItemsSource = allVictims
                    .Where(v =>
                        $"{v.LastName} {v.Name} {v.MiddleName}".ToLower().Contains(query))
                    .Select(v => new
                    {
                        Victim = v,
                        FullName = $"{v.LastName} {v.Name} {v.MiddleName}"
                    })
                    .ToList();

                VictimList.DisplayMemberPath = "FullName";
            }
            else
            {
                VictimList.ItemsSource = allVictims
                    .Select(v => new
                    {
                        Victim = v,
                        FullName = $"{v.LastName} {v.Name} {v.MiddleName}"
                    })
                    .ToList();

                VictimList.DisplayMemberPath = "FullName";
            }
        }


        private void VictimList_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    var item = VictimList.SelectedItem;
    var selected = (Victim)item.GetType().GetProperty("Victim").GetValue(item);

    FullName.Text = $"{selected.LastName} {selected.Name} {selected.MiddleName}";
    BirthDate.Text = selected.BirthDate.ToShortDateString();
    DeathDate.Text = selected.DeathDate.ToShortDateString();
    VictimDescription.Text = selected.Description;

    // Загрузка изображения
    if (string.IsNullOrEmpty(selected.ImagePath))
    {
        VictimPhoto.Source = null;
        textBlock.Text = "Нет изображения";
        textBlock.Visibility = Visibility.Visible;
    }
    else
    {
        string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, selected.ImagePath.TrimStart('\\', '/'));

        if (File.Exists(fullPath))
        {
            VictimPhoto.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
            VictimPhoto.Visibility = Visibility.Visible;
            textBlock.Visibility = Visibility.Collapsed;
        }
        else
        {
            VictimPhoto.Source = null;
            textBlock.Text = "Нет изображения";
            textBlock.Visibility = Visibility.Visible;
        }
    }
}





    }
}
