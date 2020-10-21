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
    class ListCycleViewModel : ViewModelBase
    {
        private ObservableCollection<CycleStatus> listCycles;
        public ObservableCollection<CycleStatus> ListCycles 
        { 
            get => listCycles;
            set => listCycles = value; 
        }
        public List<Status> ListStatus { get; set; }
        private CycleStatus cycle;
        public CycleStatus Cycle 
        { 
            get => cycle;
            set => cycle = value;
        }

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
                RaisePropertyChanged("StatusC");
            }
        }

        public Status Status
        {
            get => Cycle.StatusC;
            set
            {
                Cycle.StatusC = value;
                RaisePropertyChanged("Status");
            }
        }

        //Command
        public ICommand DeleteCycleCommand { get; set; }
        public ICommand EditCycleCommand { get; set; }


        /*
         * Resume :
         *      Initialize a Cycle
         *      Call the GetCycle methode to get a list of cycle
         *      Initialize DeleteCycleCommand with a methode as a parameter
         */
        public ListCycleViewModel()
        {
            Cycle = new CycleStatus();
            ListCycles = Cycle.GetCycleList();
            ListStatus = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
            DeleteCycleCommand = new RelayCommand(DeleteCycle);
            EditCycleCommand = new RelayCommand(EditCycleStatus);
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

        /**
         * Resume :
         *      edit status of a cycle
         */
        private void EditCycleStatus()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment modifier le status du cycle ?", "Confirmation modification", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if(Cycle.UpdateCycleStatus())
                {
                    MessageBox.Show("Le Status du cycle " + Cycle.Id + " a bien été modifié.");
                    ListCycles = Cycle.GetCycleList();
                    RaisePropertyChanged("ListCycles");
                }
            }
        }
    }
}
