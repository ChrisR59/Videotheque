using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Videothèque2.Models;
using Videothèque2.Views;

namespace Videothèque2.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
        private ObservableCollection<Element> listElements = new ObservableCollection<Element>();
        private Element e = new Element();

        public ObservableCollection<Element> ListElements { get => listElements; set => listElements = value; }
        public ICommand LFilmCommand { get; set; }
        public ICommand LSerieCommand { get; set; }
        public ICommand AEltCommand { get; set; }
        public ICommand ProgrammationCommand { get; set; }

        public MainWindowViewModel()
        {
            ListElements = e.GetProgram();
            LFilmCommand = new RelayCommand(() =>
            {
                ListFilmWindow lFw = new ListFilmWindow();
                lFw.Show();
            });

            LSerieCommand = new RelayCommand(() =>
            {
                ListSerieWindow lSw = new ListSerieWindow();
                lSw.Show();
            });

            AEltCommand = new RelayCommand(() =>
            {
                AddElementWindow aEw = new AddElementWindow();
                aEw.Show();
            });

            ProgrammationCommand = new RelayCommand(() =>
            {
                ProgrammationWindow p = new ProgrammationWindow();
                p.Show();
            });
        }
    }
}
