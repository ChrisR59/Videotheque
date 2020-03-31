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
    class GestionCycleViewModel : ViewModelBase
    {
        private ObservableCollection<CycleStatus> listCycle;
        private CycleStatus cycle;

        public ObservableCollection<CycleStatus> ListCycle { get => listCycle; set => listCycle = value; }
        public CycleStatus Cycle { get => cycle; set => cycle = value; }
        public ICommand AddCycleCommand { get; set; }

        public GestionCycleViewModel()
        {
            AddCycleCommand = new RelayCommand(AddCycle);
        }

        public void AddCycle()
        {
            if(Cycle.AddCycle())
            {
                MessageBox.Show("Un nouveau Cycle a été crée.");
            }
        }
    }
}
