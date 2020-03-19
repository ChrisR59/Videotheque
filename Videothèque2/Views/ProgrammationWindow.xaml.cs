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

namespace Videothèque2.Views
{
    /// <summary>
    /// Logique d'interaction pour ProgrammationWindow.xaml
    /// </summary>
    public partial class ProgrammationWindow : Window
    {
        private Element elt = new Element();
        public ProgrammationWindow()
        {
            InitializeComponent();
        }
    }
}
