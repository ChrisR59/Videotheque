using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

        //Attribute related to the film
        public string TitleF
        {
            get => film.Title;
            set
            {
                film.Title = value;
                RaisePropertyChanged();
            }
        }
        public string PosterF
        {
            get => film.Poster;
            set
            {
                film.Poster = value;
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

        //Attribute related to the serie
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
        public string PosterS
        {
            get => serie.Poster;
            set
            {
                serie.Poster = value;
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

        //Command
        public ICommand AFilmCommand { get; set; }
        public ICommand ASerieCommand { get; set; }
        public ICommand AddPosterFCommand { get; set; }
        public ICommand AddPosterSCommand { get; set; }

        /*
         * Resume : 
         *      Initialize film and serie
         *      Initialize DateAddF and DateAddS on today date
         *      Initialize AFilmCommand, AddPosterCommand and ASerieCommand with a methode as a parameter
         */
        public AddElementWindowViewModel()
        {
            film = new Film();
            serie = new Serie();
            DateAddF = DateTime.Now;
            DateAddS = DateTime.Now;

            AFilmCommand = new RelayCommand(AddFilm);
            ASerieCommand = new RelayCommand(AddSerie);
            AddPosterFCommand = new RelayCommand(AddPosterF);
            AddPosterSCommand = new RelayCommand(AddPosterS);
        }

        /**
         * Resume :
         *      Add a movie in the Bdd and then reset attribute film, TitleF,ContentF, DateAddF
         */
        private void AddFilm()
        {
            if(TitleF != null && ContentF != null)
            {
                if (film.Add())
                {
                    MessageBox.Show("Le Film '" + film.Title + "' a bien été ajouté");
                }
                else
                    MessageBox.Show("Error Insertion");
            }
                
            film = new Film();
            TitleF = null;
            ContentF = null;
            DateAddF = DateTime.Now;
        }

        /**
         * Resume : 
         *      Add a serie in the Bdd and then reset attribute serie, TitleS, NbSeasonS, ContentS,DateAddS
         */
        private void AddSerie()
        {
            if(serie.Title != null || serie.NbSeason != null || serie.Content != null)
            {
                if (serie.Add())
                {
                    MessageBox.Show("La série '" + serie.Title + "' a bien été ajouté");
                }
                else
                    MessageBox.Show("Error insertion");
            }

            serie = new Serie();
            TitleS = null;
            NbSeasonS = null;
            ContentS = null;
            DateAddS = DateTime.Now;
        }

        /*
         * Remuse :
         *      opens a dialog box to choose a file.
         */
        private void AddPosterF()
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == true)
            {
                PosterF = MoveImageToImageFolder(open.FileName);
            }
        }

        /*
         * Remuse :
         *      opens a dialog box to choose a file.
         */
        private void AddPosterS()
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == true)
            {
                PosterS = MoveImageToImageFolder(open.FileName);
            }
        }

        /*
         * Resume : 
         *      Copy the file to its new location
         */
        private string MoveImageToImageFolder(string urlToMove)
        {
            if (!Directory.Exists("posters"))
            {
                Directory.CreateDirectory("posters");
            }
            string urlAfterMove = Path.Combine(Directory.GetCurrentDirectory(), "posters", Path.GetFileName(urlToMove));
            File.Copy(urlToMove, urlAfterMove);
            return urlAfterMove;
        }
    }
}
