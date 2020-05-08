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
        private ObservableCollection<CycleContent> listCycleContent;
        private Element element;
        private CycleContent cycleC;
        private CycleStatus cycleS;
        public ObservableCollection<Element> ListElements { get; set; }
        public ObservableCollection<CycleContent> ListCycleContent { get => listCycleContent; set => listCycleContent = value; }
        public Element Element { get => element; set => element = value; }
        public CycleContent CycleC { get => cycleC; set => cycleC = value; }
        public CycleStatus CycleS { get => cycleS; set => cycleS = value; }


        public ICommand ValidateViewCommand { get; set; }//Button validation vu element
        public ICommand UpdateListCommand { get; set; }//Button actualiser la liste
        public ICommand LFilmCommand { get; set; }
        public ICommand LSerieCommand { get; set; }
        public ICommand AEltCommand { get; set; }
        public ICommand CyclesCommand { get; set; }
        public ICommand ProgrammationCommand { get; set; }

        public MainWindowViewModel()
        {
            //Recuperer l'id du cycle en cours
            Element = new Element();
            CycleS = new CycleStatus();
            CycleC = new CycleContent();
            CycleS.GetIdCycle();
            ListElements = Element.GetProgram();
            ListCycleContent = CycleC.GetCycleActually(CycleS.Id);

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

            CyclesCommand = new RelayCommand(() =>
            {
               GestionCycleWindow gc = new GestionCycleWindow();
               gc.Show();
            });

            ProgrammationCommand = new RelayCommand(() =>
            {
               ProgrammationWindow pc = new ProgrammationWindow();
               pc.Show();
            });

            ValidateViewCommand = new RelayCommand(EditElt);
            UpdateListCommand = new RelayCommand(UpList);
        }

        private void EditElt()
        {
            if (CycleC != null)
            {
                Boolean resCycle = false;
                CycleC.ToWatch = true;
                if (CycleC.Type == "Film")
                {
                    Film f = CycleC.GetOneFilm();
                    f.LastView = DateTime.Now;
                    f.NbView++;
                    f.ToWatch = false;
                    Boolean resFilm = f.UpdateLastView();
                    resCycle = CycleC.UpdateToWatch();
                    if (resFilm && resCycle)
                        MessageBox.Show("Le date du film a bien été modifié");
                }
                else if (CycleC.Type == "Serie")
                {
                    Serie s = CycleC.GetOneSerie();
                    s.LastView = DateTime.Now;
                    s.NbView++;
                    s.ToWatch = false;
                    Boolean resSerie = s.UpdateLastView();
                    resCycle = CycleC.UpdateToWatch();
                    if (resSerie && resCycle)
                        MessageBox.Show("La date de la série a bien été modifié");
                }
                //Ne se coche pas manque un RaisePropertyChanged
                RaisePropertyChanged("ListCycleContent");
            }
        }

        private void UpList()
        {
            CycleC = new CycleContent();
            ListCycleContent = CycleC.GetCycleActually(CycleS.Id);
            RaisePropertyChanged("ListCycleContent");
        }
    }
}
