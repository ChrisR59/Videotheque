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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Videothèque2.Models;
using Videothèque2.ViewModels;

namespace Videothèque2
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Element elt = new Element();
        public MainWindow()
        {
            InitializeComponent();
            ListView.ItemsSource = new MainWindowViewModel().ListElements;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListView.ItemsSource = new MainWindowViewModel().ListElements;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            elt = ListView.SelectedItem as Element;
            if (elt != null)
            {
                if(elt.Type == "Film")
                {
                    Film f = elt.GetOneFilm();
                    f.LastView = DateTime.Now;
                    f.ToWatch = false;
                    Boolean res = f.UpdateLastView();
                    if (res)
                        MessageBox.Show("Le date du film a bien été modifié");
                }
                else if(elt.Type == "Serie")
                {
                    Serie s = elt.GetOneSerie();
                    s.LastView = DateTime.Now;
                    s.ToWatch = false;
                    Boolean res = s.UpdateLastView();
                    if (res)
                        MessageBox.Show("La date de la série a bien été modifié");
                }
            }
        }
    }
}
