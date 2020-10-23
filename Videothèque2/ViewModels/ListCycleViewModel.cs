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
    class ListCycleViewModel : ViewModelBase
    {
        private ObservableCollection<CycleStatus> listCycles;
        private CycleStatus cycle;
        public ObservableCollection<CycleStatus> ListCycles { get => listCycles; set => listCycles = value; }
        public List<StatusOfCycle> ListStatus { get; set; }
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
        public StatusOfCycle StatusC
        {
            get => Cycle.StatusC;
            set
            {
                Cycle.StatusC = value;
                RaisePropertyChanged("StatusC");
            }
        }

        public StatusOfCycle Status
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
        public ICommand CreateCycleCommand { get; set; }


        /*
         * Resume :
         *      Initialize a Cycle
         *      Call the GetCycle methode to get a list of cycle
         *      Initialize DeleteCycleCommand with a methode as a parameter
         */
        public ListCycleViewModel()
        {
            Cycle = new CycleStatus();
            ListCycles = Cycle.GetAll();
            ListStatus = Enum.GetValues(typeof(StatusOfCycle)).Cast<StatusOfCycle>().ToList();
            DeleteCycleCommand = new RelayCommand(DeleteCycle);
            EditCycleCommand = new RelayCommand(EditCycleStatus);
            CreateCycleCommand = new RelayCommand(NewCycle);
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
                    ListCycles = Cycle.GetAll();
                    RaisePropertyChanged("ListCycles");
                }
            }
        }

        /**
         * Resume :
         *      Create a new cycle
         */
        private void NewCycle()
        {
            Cycle.CheckCycleExist();
            Cycle.GetNumberCycle();

            if (Cycle.Add())
            {
                MessageBox.Show("Un nouveau Cycle a été crée.");
                ListCycles = Cycle.GetAll();
                RaisePropertyChanged("ListCycles");
            }
        }
    }
}
