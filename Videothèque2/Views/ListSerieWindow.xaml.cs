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
    /// Logique d'interaction pour ListSerieWindow.xaml
    /// </summary>
    public partial class ListSerieWindow : Window
    {
        private Serie s = new Serie();
        public ListSerieWindow()
        {
            InitializeComponent();
            ListSerieView.ItemsSource = new ListSerieWindowViewModel().LSerie;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            s = ListSerieView.SelectedItem as Serie;
            if (s != null)
            {
                TitleS.Text = s.Title;
                LastView.SelectedDate = s.LastView;
                NbSeason.Text = s.NbSeason;
            }
        }
    }
}
