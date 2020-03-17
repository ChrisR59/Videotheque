using System;
using System.Collections.Generic;
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
using Videothèque2.Models;
using Videothèque2.ViewModels;

namespace Videothèque2
{
    /// <summary>
    /// Logique d'interaction pour ListFilmWindow.xaml
    /// </summary>
    public partial class ListFilmWindow : Window
    {
        private Film f = new Film();
        public ListFilmWindow()
        {
            InitializeComponent();
            ListFilmView.ItemsSource = new ListFilmWindowViewModel().LFilm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            f = ListFilmView.SelectedItem as Film;
            if (f != null)
            {
                TitleF.Text = f.Title;
                ContentF.Text = f.Content;
                LastView.SelectedDate = f.LastView;
                NbViewF.Content = f.NbView;
            }
        }
    }
}
