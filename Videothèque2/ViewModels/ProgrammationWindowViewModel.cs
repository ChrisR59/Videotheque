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
        private Boolean unprogramm = false;
        private ObservableCollection<int> listCycle;
        private ObservableCollection<Element> listView;
        private CycleStatus cycleS;
        private CycleContent cycleC;
        private Element element;
        private Element elementSave;
        private int idCycle;
        private string title;

        public ObservableCollection<int> ListCycle { get => listCycle; set => listCycle = value; }
        public ObservableCollection<Element> ListView { get => listView; set => listView = value; }
        public CycleStatus CycleS { get => cycleS; set => cycleS = value; }
        public CycleContent CycleC { get => cycleC; set => cycleC = value; }
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
        public int IdCycle { get => idCycle; set => idCycle = value; }       
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
        public ICommand AddEltCycleCommand { get; set; }
        public ICommand UnprogramCommand { get; set; }
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
            ListView = Element.GetEltProgram();
            Title = "Aucun élement sauvegardé";
            AddEltCycleCommand = new RelayCommand(AddEltCycle);
            UnprogramCommand = new RelayCommand(UnprogramElement, CanExecute);
            SaveEltCommand = new RelayCommand(SaveElt);
            AddEltSaveCommand = new RelayCommand(AddEltSave);
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
                    UnprogramElement();
                }
            }
        }

        /**
         * Resume :
         *      Remove a element in the list
         */
        private void RemoveELtList(Boolean Unprogram)
        {
            if (Unprogram)
            {
                MessageBox.Show(Element.Title + " bien ajouté au cycle.");
                ListView.Remove(Element);
                RaisePropertyChanged("ListView");
                CycleC.Rank = 0;
                Element = new Element();
            }
        }

        /**
         * Resume :
         *      Unprogram a element 
         */
        private void UnprogramElement()
        {
            switch (Element.Type)
            {
                case "Film":
                    Film f = new Film();
                    f = f.GetOneWithId(Element.Id);
                    f.ToWatch = false;
                    unprogramm = f.UpdateToWatchFilm();
                    break;
                case "Serie":
                    Serie s = new Serie();
                    s = s.GetOneWithId(Element.Id);
                    s.ToWatch = false;
                    unprogramm = s.UpdateToWatchSerie();
                    break;
                case "Découverte":
                    Discover d = new Discover();
                    d = d.GetOneWithId(Element.Id);
                    d.ToWatch = false;
                    unprogramm = d.UpadateToWatchDiscover();
                    break;
                default:
                    MessageBox.Show("Je ne reconnais pas cette élément.");
                    break;
            }
            RemoveELtList(unprogramm);
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
         * Resume :
         *      Save a element selected
         */
        private void SaveElt()
        {
            ElementSave = Element;
        }

        /**
         * Resume :
         *      Check if the button "Supprimé" can be use
         *      UTILITE DE LA METHODE??????????
         */
        private Boolean CanExecute()
        {
            Boolean res = true;

            return res;
        }
    }
}
