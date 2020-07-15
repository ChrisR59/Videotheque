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
    public class MainWindowViewModel: ViewModelBase
    {
        private ObservableCollection<int> listCycle;
        private ObservableCollection<CycleContent> listCycleContent;
        private CycleContent cycleC;
        private CycleStatus cycleS;
        private int idCycle;
        private List<Rating> listRating;
        private ObservableCollection<Rating> listRatingObs;
        private Rating rate;
        public ObservableCollection<int> ListCycle { get => listCycle; set => listCycle = value; }
        public ObservableCollection<CycleContent> ListCycleContent { get => listCycleContent; set => listCycleContent = value; }
        public CycleContent CycleC { get => cycleC; set => cycleC = value; }
        public CycleStatus CycleS { get => cycleS; set => cycleS = value; }
        public int IdCycle { get => idCycle; set => idCycle = value; }
        public List<Rating> ListRating { get => listRating; set => listRating = value; }
        public ObservableCollection<Rating> ListRatingObs { get => listRatingObs; set => listRatingObs = value; }
        public Rating Rate { get => rate; set => rate = value; }

        //Commands button
        public ICommand ValidateViewCommand { get; set; }
        public ICommand UpdateListCommand { get; set; }
        //Command Menu 
        public ICommand LFilmCommand { get; set; }
        public ICommand LSerieCommand { get; set; }
        public ICommand AEltCommand { get; set; }
        public ICommand CyclesCommand { get; set; }
        public ICommand ProgrammationCommand { get; set; }

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
            ListCycleContent = CycleC.GetCurrentCycle(CycleS.Id);
            ListRating = Enum.GetValues(typeof(Rating)).Cast<Rating>().ToList();
            ListRatingObs = new ObservableCollection<Rating>();
            foreach (Rating r in ListRating)
            {
                ListRatingObs.Add(r);
            }

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
            MessageBoxResult messageBoxResult = MessageBox.Show("Vous avez vu ce film/série ?", "Confirmation!", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (CycleC != null)
                {
                    Boolean resCycle = false;
                    CycleC.ToWatch = true;

                    if (CycleC.Type == "Film")
                    {
                        Film f = CycleC.GetOneFilm();
                        f.LastView = DateTime.Now;
                        //f.Rating = Rate;
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
                        //s.Rating = Rate;
                        s.NbView++;
                        s.ToWatch = false;
                        Boolean resSerie = s.UpdateLastView();
                        resCycle = CycleC.UpdateToWatch();

                        if (resSerie && resCycle)
                            MessageBox.Show("La date de la série a bien été modifié");
                    }

                    //Verifie si les élement ont tous était vu
                    Boolean cycleEnd = true;

                    foreach (CycleContent c in ListCycleContent)
                    {
                        if (!c.ToWatch)
                        {
                            cycleEnd = false;
                        }
                    }

                    //Changement de status du cycle
                    if (cycleEnd)
                    {
                        CycleS.StatusC = Status.Termine;

                        if (CycleS.EditStatusCycle())
                            MessageBox.Show("Cycle Terminé");

                        CycleS = new CycleStatus();
                        CycleS.GetNewCycle();

                        if (CycleS.EditStatusCycle())
                            MessageBox.Show("Nouveau cycle chargé");
                    }

                    UpList();
                }
            }
        }

        /**
         * Resume : 
         *      Initialize a CycleC
         *      Call GetCycleActually method for initialize a list with a parameter CycleS.Id
         */
        private void UpList()
        {
            CycleC = new CycleContent();
            ListCycleContent = CycleC.GetCurrentCycle(IdCycle);
            RaisePropertyChanged("ListCycleContent");
        }
    }
}
