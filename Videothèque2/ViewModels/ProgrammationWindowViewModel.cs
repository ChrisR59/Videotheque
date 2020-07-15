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
    class ProgrammationWindowViewModel:ViewModelBase
    {
        private ObservableCollection<int> listCycle;
        public ObservableCollection<int> ListCycle { get => listCycle; set => listCycle = value; }

        private ObservableCollection<Element> listElements;
        public ObservableCollection<Element> ListElements { get => listElements; set => listElements = value; }

        //Cycle Status
        private CycleStatus cycleS;
        public CycleStatus CycleS { get => cycleS; set => cycleS = value; }

        //Cycle Content
        private CycleContent cycleC;
        public CycleContent CycleC { get => cycleC; set => cycleC = value; }

        private Element element;
        public Element Element { get => element; set => element = value; }

        //Cycle Statu item
        private int idCycle;
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

        //Command
        public ICommand AddCycleCommand { get; set; }
        public ICommand AddEltCycleCommand { get; set; }

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
            ListElements = Element.GetProgram();
            AddCycleCommand = new RelayCommand(CreateCycle);
            AddEltCycleCommand = new RelayCommand(AddEltCycle);
        }

        /**
         * Resume : 
         *      Create a new Cycle. "Cycle Status"
         */
        public void CreateCycle()
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
        public void AddEltCycle()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment ajouter cet élément ?", "Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                CycleC.IdCycle = IdCycle;
                CycleC.GetRankElt();
                CycleC.Title = Element.Title;
                CycleC.Status = "A voir";
                CycleC.Type = Element.Type;
                CycleC.IdElt = Element.Id;
                if (CycleC.AddElement())
                {
                    if (Element != null)
                    {
                        if (Element.Type == "Film")
                        {
                            Film f = Element.GetOneFilm();
                            f.ToWatch = false;
                            Boolean res = f.UpFilmProgramm();
                        }
                        else if (Element.Type == "Serie")
                        {
                            Serie s = Element.GetOneSerie();
                            s.ToWatch = false;
                            Boolean res = s.UpSerieProgramm();
                        }
                    }
                    ListElements.Remove(Element);
                    RaisePropertyChanged("ListElements");
                    MessageBox.Show("Element bien ajouté au cycle.");
                    CycleC.Rank = 0;
                }
            }
        }
    }
}
