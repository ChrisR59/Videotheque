using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videothèque2.Models;

namespace Videothèque2.ViewModels
{
    public class ListSerieWindowViewModel:ViewModelBase
    {
        private List<Serie> lSerie = new List<Serie>();
        private Serie s = new Serie();

        public ListSerieWindowViewModel()
        {
            LSerie = s.GetSerie();
        }

        public List<Serie> LSerie { get => lSerie; set => lSerie = value; }
    }
}
