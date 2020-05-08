using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    class GestionCycleViewModel : ViewModelBase
    {
        public ObservableCollection<CycleStatus> ListCycles { get; set; }
        private CycleStatus cycle;

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

        public CycleStatus Cycle { get => cycle; set => cycle = value; }
        public ICommand DeleteCycleCommand { get; set; }

        public GestionCycleViewModel()
        {
            Cycle = new CycleStatus();
            ListCycles = Cycle.GetCycles();
            DeleteCycleCommand = new RelayCommand(DeleteCycle);
        }

        private void DeleteCycle()
        {
            if (Cycle != null && Cycle.DeleteOne())
            {
                ListCycles.Remove(Cycle);
                RaisePropertyChanged("ListCycles");
            }

        }
    }
}
