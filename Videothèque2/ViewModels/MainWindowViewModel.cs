using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Videothèque2.Models;
using Videothèque2.Views;

namespace Videothèque2.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
        public ObservableCollection<Element> ListElements { get; set; }
        private Element element = new Element();


        public ICommand LFilmCommand { get; set; }
        public ICommand LSerieCommand { get; set; }
        public ICommand AEltCommand { get; set; }
        public ICommand ProgrammationCommand { get; set; }
        public ICommand ProgramEltCommand { get; set; }
        public ICommand UpdateListCommand { get; set; }
        public ICommand CycleCommand { get; set; }
        public Element Element { get => element; set => element = value; }

        public MainWindowViewModel()
        {
            ListElements = Element.GetProgram();

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

            ProgramEltCommand = new RelayCommand(EditElt);
            UpdateListCommand = new RelayCommand(UpList);
        }

        private void EditElt()
        {
            if (Element != null)
            {
                if (Element.Type == "Film")
                {
                    Film f = Element.GetOneFilm();
                    f.LastView = DateTime.Now;
                    f.NbView++;
                    f.ToWatch = false;
                    Boolean res = f.UpdateLastView();
                    if (res)
                        MessageBox.Show("Le date du film a bien été modifié");
                }
                else if (Element.Type == "Serie")
                {
                    Serie s = Element.GetOneSerie();
                    s.LastView = DateTime.Now;
                    s.NbView++;
                    s.ToWatch = false;
                    Boolean res = s.UpdateLastView();
                    if (res)
                        MessageBox.Show("La date de la série a bien été modifié");
                }
                Element.ToWatch = false;
                UpList();
            }
        }

        private void UpList()
        {
            Element = new Element();
            ListElements = Element.GetProgram();
            RaisePropertyChanged("ListElements");
        }
    }
}
