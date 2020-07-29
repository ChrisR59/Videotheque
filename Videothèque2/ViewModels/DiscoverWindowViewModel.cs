using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videothèque2.Models;

namespace Videothèque2.ViewModels
{
    class DiscoverWindowViewModel
    {
        private ObservableCollection<Discover> listDiscover;
        private Discover discover;
        public ObservableCollection<Discover> ListDiscover { get => listDiscover; set => listDiscover = value; }
        public Discover Discover { get => discover; set => discover = value; }


        public DiscoverWindowViewModel()
        {
            Discover = new Discover();
            ListDiscover = Discover.GetAllDiscover();
        }

    }
}
