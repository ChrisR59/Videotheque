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
    class DiscoverWindowViewModel:ViewModelBase
    {
        private ObservableCollection<Discover> listDiscover;
        private Discover discover;
        public ObservableCollection<Discover> ListDiscover { get => listDiscover; set => listDiscover = value; }
        public Discover Discover { get => discover; set => discover = value; }

        public ICommand DeleteDiscoverCommand { get; set; }

        public DiscoverWindowViewModel()
        {
            Discover = new Discover();
            ListDiscover = Discover.GetAllDiscover();

            DeleteDiscoverCommand = new RelayCommand(DelDiscover);
        }

        private void DelDiscover()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer cette Decouverte?", "Confirmation Suppression", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Discover.Id != 0)
                {
                    if (Discover.DeleteOneDiscover())
                    {
                        MessageBox.Show("La Decouverte a bien été supprimé.");
                        ListDiscover = Discover.GetAllDiscover();
                        RaisePropertyChanged("ListDiscover");
                    }
                }
            }
        }

    }
}
