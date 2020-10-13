using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Videothèque2.Models;

namespace Videothèque2.ViewModels
{
    class GestionCycleWindowViewModel : ViewModelBase
    {
        private ObservableCollection<int> listCycleID;
        private ObservableCollection<CycleContent> cycleCView;
        private CycleStatus cycleS;
        private CycleContent cycleC;
        private Element element;
        //Cycle Statu item
        private int idCycle;
        public int IdCycle
        {
            get => idCycle;
            set
            {
                idCycle = value;
                CycleCView = CycleC.GetCurrentCycle(IdCycle);
                RaisePropertyChanged("CycleCView");
            }
        }
        public Element Element { get => element; set => element = value; }
        public CycleStatus CycleS { get => cycleS; set => cycleS = value; }
        public CycleContent CycleC
        {
            get => cycleC;
            set
            {
                cycleC = value;
                if (cycleC == null)
                    cycleC = new CycleContent();

                RaisePropertyChanged("Rank");
            }
        }
        public int Rank
        {
            get => CycleC.Rank;
            set
            {
                CycleC.Rank = value;
                RaisePropertyChanged("Rank");
            }
        }
        public ObservableCollection<int> ListCycleID { get => listCycleID; set => listCycleID = value; }
        public ObservableCollection<CycleContent> CycleCView { get => cycleCView; set => cycleCView = value; }

        public ICommand EditEltCycleCommand { get; set; }

        /*
         * Resume : 
         *      Initialize a Cycle Status, Element and Cycle Content
         *      Get List Cycle not Finish
         *      Initialize a commands with a methode as a parameter
         */
        public GestionCycleWindowViewModel()
        {
            CycleS = new CycleStatus();
            CycleC = new CycleContent();
            Element = new Element();
            ListCycleID = CycleS.GetCycleListNotFinish();

            EditEltCycleCommand = new RelayCommand(EditRank);
        }

        /**
         * Resume :
         *      Edit the rank rank of an element of a cycle
         */
        private void EditRank()
        {
            CycleC.UpdateRank();
            CycleCView = CycleC.GetCurrentCycle(IdCycle);
            RaisePropertyChanged("CycleCView");
        }
    }
}
