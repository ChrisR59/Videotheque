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

namespace Videothèque2.ViewModels
{
    class GestionCycleViewModel : ViewModelBase
    {
        public ObservableCollection<CycleStatus> ListCycles { get; set; }
        private CycleStatus cycle;
        public CycleStatus Cycle { get => cycle; set => cycle = value; }

        //Attribute related in the cycle
        public int Id
        {
            get => Cycle.Id;
            set
            {
                Cycle.Id = value;
                RaisePropertyChanged("Id");
            }
        }
        public Status StatusC
        {
            get => Cycle.StatusC;
            set
            {
                Cycle.StatusC = value;
                RaisePropertyChanged("Title");
            }
        }

        //Command
        public ICommand DeleteCycleCommand { get; set; }


        /*
         * Resume :
         *      Initialize a Cycle
         *      Call the GetCycle methode to get a list of cycle
         *      Initialize DeleteCycleCommand with a methode as a parameter
         */
        public GestionCycleViewModel()
        {
            Cycle = new CycleStatus();
            ListCycles = Cycle.GetCycles();
            DeleteCycleCommand = new RelayCommand(DeleteCycle);
        }

        /*
         * Resume :
         *      Delete a cycle with a MessageBoxResult for confirmation
         *      Delete in the Bdd and in the listCycles
         */
        private void DeleteCycle()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer ce cycle?", "Confirmation suppression", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Cycle != null && Cycle.DeleteOne())//!! si supression du cycle en cours changé status du cycle suivant
                {
                    ListCycles.Remove(Cycle);
                    RaisePropertyChanged("ListCycles");
                }
            }
        }
    }
}
