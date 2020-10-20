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
using Videothèque2.Tools;
using Videothèque2.Views;

namespace Videothèque2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<int> listCycle;
        private ObservableCollection<CycleContent> listCycleContent;
        private CycleContent cycleC;
        private CycleStatus cycleS;
        private int idCycle;
        public ObservableCollection<int> ListCycle { get => listCycle; set => listCycle = value; }
        public ObservableCollection<CycleContent> ListCycleContent { get => listCycleContent; set => listCycleContent = value; }
        public CycleContent CycleC { get => cycleC; set => cycleC = value; }
        public CycleStatus CycleS { get => cycleS; set => cycleS = value; }
        public int IdCycle 
        { 
            get => idCycle;
            set
            {
                idCycle = value;
                CycleC = new CycleContent();
                ListCycleContent = CycleC.GetCurrentCycle(IdCycle);
                RaisePropertyChanged("ListCycleContent");
            }
        }

        //Commands button
        public ICommand ValidateViewCommand { get; set; }
        public ICommand UpdateListCommand { get; set; } // Command inutile mise à jours de la list
        //Command Menu 
        public ICommand LFilmCommand { get; set; }
        public ICommand LSerieCommand { get; set; }
        public ICommand AEltCommand { get; set; }
        public ICommand ListCyclesCommand { get; set; }
        public ICommand ProgrammationCommand { get; set; }
        public ICommand DiscoverCommand { get; set; }
        public ICommand GestionCyclesCommand { get; set; }

        /*
         * Resume :
         *      Initialize a element, CycleS, CycleC
         *      Call GetIdCycle for get Id of the current cycle
         *      Call GetCycleActually for element list of the cycle
         *      Initalize command
         */
        public MainWindowViewModel()
        {
            CycleS = new CycleStatus();
            CycleC = new CycleContent();
            ListCycle = CycleS.GetAllCycle();
            IdCycle = CycleS.GetIdCycle();
            ListCycleContent = CycleC.GetCurrentCycle(IdCycle);
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

            DiscoverCommand = new RelayCommand(() =>
            {
                DiscoverWindow dicover = new DiscoverWindow();
                dicover.Show();
            });

            AEltCommand = new RelayCommand(() =>
            {
                AddElementWindow aEw = new AddElementWindow();
                aEw.Show();
            });

            ListCyclesCommand = new RelayCommand(() =>
            {
                ListCycleWindow lc = new ListCycleWindow();
                lc.Show();
            });

            GestionCyclesCommand = new RelayCommand(() =>
            {
                GestionCycleWindow gc = new GestionCycleWindow();
                gc.Show();
            });

            ProgrammationCommand = new RelayCommand(() =>
            {
                ProgrammationWindow pc = new ProgrammationWindow();
                pc.Show();
            });

            ValidateViewCommand = new RelayCommand(ValidViewElt);
            UpdateListCommand = new RelayCommand(UpList);
        }

        /*
         * Resume : 
         *      Confirm a editing of a serie or film
         *      Check if the element is a film or is a serie
         *      Check with a foreach if all elemnt of the cycle have been seen. Attribute "ToWatch"
         *      If all element of the cycle have been seen, end the current cycle and get next cycle
         */
        private void ValidViewElt()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Vous avez vu " + CycleC.Title + " ?", "Confirmation!", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (CycleC != null)
                {
                    UpEltToWatch();
                    Boolean cycleEnd = CheckCycle();
                    UpCycle(cycleEnd);
                    UpList();
                }
            }
        }

        /**
         * Resume :
         *      confirm that an Element of the Cycle has been viewed
         */
        private void UpEltToWatch()
        {
            Boolean resCycle = false;
            Boolean resFilm = false;
            Boolean resSerie = false;
            CycleC.ToWatch = true;

            switch (CycleC.Type)
            {
                case "Film":
                    Film f = CycleC.GetOneFilm();
                    f.LastView = DateTime.Now;
                    f.NbView++;
                    f.ToWatch = false;
                    resFilm = f.UpdateLastView();
                    resCycle = CycleC.UpdateToWatch();
                    break;
                case "Serie":
                    Serie s = CycleC.GetOneSerie();
                    s.LastView = DateTime.Now;
                    s.NbView++;
                    s.ToWatch = false;
                    resSerie = s.UpdateLastView();
                    resCycle = CycleC.UpdateToWatch();
                    break;
                case "Découverte":
                    resCycle = CycleC.UpdateToWatch();
                    break;
            }

            if(resCycle && (resFilm || resSerie))
                MessageBox.Show("La date de " + CycleC.Title + " a bien été modifié");
        }

        /**
         * Resume :
         *      check if the elements have all been viewed
         * Return
         *      a boolean which will confirm if the cycle is finished 
         */
        private Boolean CheckCycle()
        {
            Boolean CycleEnd = false;

            foreach (CycleContent c in ListCycleContent)
            {
                if (!c.ToWatch)
                {
                    CycleEnd = false;
                }
            }

            return CycleEnd;
        }

        /**
         * Resume :
         *      cycle change
         * Parameter
         *      Boolean which is true if the cycle is finished
         */
        private void UpCycle(Boolean CycleEnd)
        {
            if (CycleEnd)
            {
                CycleS.StatusC = Status.Termine;

                if (CycleS.EditStatusCycle())
                    MessageBox.Show("Cycle Terminé");

                CycleS = new CycleStatus();
                IdCycle = CycleS.GetNewCycle();

                if (CycleS.EditStatusCycle())
                    MessageBox.Show("Nouveau cycle chargé");
            }
        }

        /** potentiellement inutile mise à jours list
         * Resume : 
         *      Initialize a CycleC
         *      Call GetCycleActually method for initialize a list with a parameter CycleS.Id
         */
        private void UpList()
        {
            CycleC = new CycleContent();
            ListCycleContent = CycleC.GetCurrentCycle(IdCycle);
            RaisePropertyChanged("ListCycleContent");
            RaisePropertyChanged("IdCycle");
        }
    }
}
