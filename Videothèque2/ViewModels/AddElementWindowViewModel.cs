using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Videothèque2.Models;

namespace Videothèque2.ViewModels
{
    public class AddElementWindowViewModel:ViewModelBase
    {
        private Film film;
        private Serie serie;

        public string TitleF
        {
            get => film.Title;
            set
            {
                film.Title = value;
                RaisePropertyChanged();
            }
        }

        public DateTime LastViewF
        {
            get => film.LastView;
            set
            {
                film.LastView = value;
                RaisePropertyChanged();
            }
        }

        public string TitleS
        {
            get => serie.Title;
            set
            {
                serie.Title = value;
                RaisePropertyChanged();
            }
        }
        public string NbSeasonS
        {
            get => serie.NbSeason;
            set
            {
                serie.NbSeason = value;
                RaisePropertyChanged();
            }
        }

        public DateTime LastViewS
        {
            get => serie.LastView;
            set
            {
                serie.LastView = value;
                RaisePropertyChanged();
            }
        }
        public ICommand AFilmCommand { get; set; }
        public ICommand ASerieCommand { get; set; }

        public AddElementWindowViewModel()
        {
            film = new Film();
            serie = new Serie();
            LastViewF = DateTime.Now;
            LastViewS = DateTime.Now;

            AFilmCommand = new RelayCommand(AddFilm);
            ASerieCommand = new RelayCommand(AddSerie);
        }

        private void AddFilm()
        {
            if (film.Add())
            {
                MessageBox.Show("add film");
                film = new Film();
                TitleF = "";
                LastViewF = DateTime.Now;
            }
            else
                MessageBox.Show("Error Insertion");
        }

        private void AddSerie()
        {
            if (serie.Add())
            {
                MessageBox.Show("add serie");
                serie = new Serie();
                TitleS = "";
                NbSeasonS = "";
                LastViewS = DateTime.Now;
            }
            else
                MessageBox.Show("Error insertion");
        }
    }
}
