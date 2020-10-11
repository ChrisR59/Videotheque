using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Videothèque2.Models;

namespace Videothèque2.ViewModels
{
    class ProgrammationWindowViewModel : ViewModelBase
    {
        private bool elementsAvailable;
        private bool elementsCycle;
        private ObservableCollection<int> listCycle;
        private ObservableCollection<Element> listView;
        private string title;
        public bool ElementsAvailable
        {
            get => elementsAvailable;
            set
            {
                elementsAvailable = value;
                if (value)
                {
                    Element = new Element();
                    ListView = Element.GetProgram();
                    RaisePropertyChanged("ListView");
                }
                RaisePropertyChanged();
            }
        }
        public bool ElementsCycle
        {
            get => elementsCycle;
            set
            {
                elementsCycle = value;
                if (value)
                {
                    Element = new Element();
                    ListView = Element.GetCycle(IdCycle);//list de cycleContent
                    RaisePropertyChanged("ListView");
                }
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<int> ListCycle { get => listCycle; set => listCycle = value; }
        public ObservableCollection<Element> ListView { get => listView; set => listView = value; }

        //Cycle Status
        private CycleStatus cycleS;
        public CycleStatus CycleS { get => cycleS; set => cycleS = value; }

        //Cycle Content
        private CycleContent cycleC;
        public CycleContent CycleC { get => cycleC; set => cycleC = value; }

        private Element element;
        private Element elementSave;
        public Element Element { get => element; set => element = value; }
        public Element ElementSave
        {
            get => elementSave;
            set
            {
                elementSave = value;
                Title = elementSave.Title;
                RaisePropertyChanged("Title");
            }
        }

        //Cycle Statu item
        private int idCycle;
        public int IdCycle
        {
            get => idCycle;
            set
            {
                idCycle = value;
                if (ElementsCycle)
                {
                    ListView = Element.GetCycle(IdCycle);//list de cycleContent
                    RaisePropertyChanged("ListView");
                }
            }
        }
        public Status Status
        {
            get => CycleS.StatusC;
            set
            {
                CycleS.StatusC = value;
                RaisePropertyChanged("StatusC");
            }

        }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        //Command
        public ICommand AddCycleCommand { get; set; }
        public ICommand AddEltCycleCommand { get; set; }
        public ICommand DelEltCycleCommand { get; set; }
        public ICommand SaveEltCommand { get; set; }
        public ICommand AddEltSaveCommand { get; set; }

        /*
         * Resume : 
         *      Initialize CycleS, CycleC and Element
         *      Call GetListCycle for a ListCycle
         *      Initialize Command
         */
        public ProgrammationWindowViewModel()
        {
            CycleS = new CycleStatus();
            CycleC = new CycleContent();
            Element = new Element();
            ListCycle = CycleS.GetCycleListNotFinish();
            ElementsAvailable = true;
            ListView = Element.GetProgram();
            Title = "Aucun élement sauvegardé";
            AddCycleCommand = new RelayCommand(CreateCycle);
            AddEltCycleCommand = new RelayCommand(AddEltCycle);
            DelEltCycleCommand = new RelayCommand(DelEltCycle, CanExecute);
            SaveEltCommand = new RelayCommand(SaveElt);
            AddEltSaveCommand = new RelayCommand(AddEltSave);
        }

        /**
         * Resume : 
         *      Create a new Cycle. "Cycle Status"
         */
        private void CreateCycle()
        {
            CycleS.CheckCycleExist();
            CycleS.GetNumberCycle();

            if (CycleS.NewCycle())
            {
                MessageBox.Show("Un nouveau Cycle a été crée.");
            }
        }

        /**
         * Resume : 
         *      Add an Element to the selected cycle
         *      Call UpdateElement for maj Film or Serie selected
         *      Edit BDD film or serie and Cycle Content
         *      Remove an elemenet of the listElements
         */
        private void AddEltCycle()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment ajouter " + Element.Title + " ?", "Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Boolean res = false;
                CycleC.IdCycle = IdCycle;
                CycleC.GetRankElt();
                CycleC.Title = Element.Title;
                CycleC.Status = "A voir";
                CycleC.Type = Element.Type;
                CycleC.NbElt = Element.NbElt;
                CycleC.IdElt = Element.Id;
                CycleC.Comment = Element.Comment;
                if (Element.Comment == null)
                    CycleC.Comment = "";

                if (CycleC.AddElement() && Element != null)
                {
                    switch (Element.Type)// Deprogram a element
                    {
                        case "Film":
                            Film f = new Film();
                            f = f.GetOneFilm(Element.Id);
                            f.ToWatch = false;
                            res = f.UpFilmProgramm();
                            break;
                        case "Serie":
                            Serie s = new Serie();
                            s = s.GetOneSerie(Element.Id);
                            s.ToWatch = false;
                            res = s.UpSerieProgramm();
                            break;
                        case "Découverte":
                            Discover d = new Discover();
                            d = d.GetOneDiscover(Element.Id);
                            d.ToWatch = false;
                            res = d.UpProgrammDiscover();
                            break;
                        default:
                            MessageBox.Show("Je ne reconnais pas cette élément.");
                            break;
                    }

                    if (res)
                    {
                        ListView.Remove(Element);
                        RaisePropertyChanged("ListView");
                        MessageBox.Show(Element.Title + " bien ajouté au cycle.");
                        CycleC.Rank = 0;
                        Element = new Element();
                    }
                }
            }
        }

        /**
         * Resume : 
         *      Add an Element to the save in the cycle
         *      Call UpdateElement for maj Film or Serie selected
         *      Edit BDD film or serie and Cycle Content
         */
        private void AddEltSave()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment ajouter " + Element.Title + "?", "Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                CycleC.IdCycle = IdCycle;
                CycleC.GetRankElt();
                CycleC.Title = ElementSave.Title;
                CycleC.Status = "A voir";
                CycleC.Type = ElementSave.Type;
                CycleC.NbElt = ElementSave.NbElt;
                CycleC.IdElt = ElementSave.Id;
                CycleC.Comment = ElementSave.Comment;
                if (ElementSave.Comment == null)
                    CycleC.Comment = "";

                if (CycleC.AddElement() && ElementSave != null)
                {
                    MessageBox.Show(Element.Title + " bien ajouté au cycle.");
                    CycleC.Rank = 0;
                }
            }
        }

        /**
         * 
         */
        private void SaveElt()
        {
            ElementSave = Element;
        }


        /*
         * Resume :
         *      Remove a cycle
         */
        private void DelEltCycle()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer " + Element.Title + "?", "Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                bool del = false;
                //Supprimer dans CycleContent
                CycleC.IdCycle = Element.Id;
                CycleC.Type = Element.Type;
                CycleC.Id = Element.IdCycle;
                if (CycleC.DelElement())
                {
                    if (CycleC.Type == "Film")
                    {
                        if (Element.UnWatchFilm())
                        {
                            Element.ToWatch = false;
                            del = true;
                        }
                    }
                    else if (CycleC.Type == "Serie")
                    {
                        if (Element.UnWatchSerie())
                        {
                            Element.ToWatch = false;
                            del = true;
                        }
                    }
                    else if (CycleC.Type == "Découverte")
                    {
                        if (Element.UnWatchDiscover())
                        {
                            Element.ToWatch = false;
                            del = true;
                        }
                    }
                }

                if (del)
                {
                    ListView.Remove(Element);
                    RaisePropertyChanged("ListView");
                    MessageBox.Show(Element.Title + " bien supprimé au cycle.");
                    CycleC = new CycleContent();
                    Element = new Element();
                }
                //Deprogrammer dans Film ou Serie
            }

        }

        /**
         * Resume :
         *      Check if the button "Supprimé" can be use
         */
        private Boolean CanExecute()
        {
            Boolean res = true;

            if (ElementsAvailable)
                res = false;

            return res;
        }
    }
}
