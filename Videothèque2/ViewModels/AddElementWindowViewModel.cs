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

        public string ContentF
        {
            get => film.Content;
            set
            {
                film.Content = value;
                RaisePropertyChanged();
            }
        }

        public DateTime DateAddF
        {
            get => film.DateAdd;
            set
            {
                film.DateAdd = value;
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

        public string ContentS
        {
            get => serie.Content;
            set
            {
                serie.Content = value;
                RaisePropertyChanged();
            }
        }
        public DateTime DateAddS
        {
            get => serie.DateAdd;
            set
            {
                serie.DateAdd = value;
                RaisePropertyChanged();
            }
        }
        public ICommand AFilmCommand { get; set; }
        public ICommand ASerieCommand { get; set; }

        public AddElementWindowViewModel()
        {
            film = new Film();
            serie = new Serie();
            DateAddF = DateTime.Now;
            DateAddS = DateTime.Now;

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
                ContentF = "";
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
                ContentS = "";
            }
            else
                MessageBox.Show("Error insertion");
        }
    }
}
