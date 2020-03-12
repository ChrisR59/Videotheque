using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Videothèque2.Models;

namespace Videothèque2.ViewModels
{
    public class ListFilmWindowViewModel:ViewModelBase
    {
        private List<Film> lFilm = new List<Film>();
        private Film f = new Film();

        public ListFilmWindowViewModel()
        {
            LFilm = f.GetFilms();
        }

        public List<Film> LFilm { get => lFilm; set => lFilm = value; }
    }
}
