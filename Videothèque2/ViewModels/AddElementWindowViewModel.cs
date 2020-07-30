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
    public class AddElementWindowViewModel : ViewModelBase
    {
        private Film film;
        private Serie serie;
        private Discover discover;

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
        public string GenreF
        {
            get => film.Genre;
            set
            {
                film.Genre = value;
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
        public string DirectorF
        {
            get => film.Director;
            set
            {
                film.Director = value;
                RaisePropertyChanged();
            }
        }
        public string StarsF
        {
            get => film.Stars;
            set
            {
                film.Stars = value;
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
        public string GenreS
        {
            get => serie.Genre;
            set
            {
                serie.Genre = value;
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
        public string DirectorS
        {
            get => serie.Director;
            set
            {
                serie.Director = value;
                RaisePropertyChanged();
            }
        }
        public string StarsS
        {
            get => serie.Stars;
            set
            {
                serie.Stars = value;
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

        //Attribute Related to the Discover
        public string TitleD
        {
            get => discover.Title;
            set
            {
                discover.Title = value;
                RaisePropertyChanged();
            }
        }

        public string ReleaseDateD
        {
            get => discover.ReleaseDate;
            set
            {
                discover.ReleaseDate = value;
                RaisePropertyChanged();
            }
        }

        //Command
        public ICommand AFilmCommand { get; set; }
        public ICommand ASerieCommand { get; set; }
        public ICommand AddPosterCommand { get; set; }
        public ICommand ADiscoverCommand { get; set; }

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
            discover = new Discover();
            DateAddF = DateTime.Now;
            DateAddS = DateTime.Now;

            AFilmCommand = new RelayCommand(AddFilm);
            ASerieCommand = new RelayCommand(AddSerie);
            AddPosterCommand = new RelayCommand(AddPoster);
            ADiscoverCommand = new RelayCommand(AddDiscover);
        }

        /**
         * Resume :
         *      Add a movie in the Bdd and then reset attribute film, TitleF,ContentF, DateAddF
         */
        private void AddFilm()
        {
            if (TitleF != null && ContentF != null && DirectorF != null && GenreF != null && StarsF != null && PosterF != null)
            {
                if (film.Add())
                {
                    MessageBox.Show("Le Film '" + film.Title + "' a bien été ajouté");

                    film = new Film();
                    TitleF = null;
                    GenreF = null;
                    ContentF = null;
                    DirectorF = null;
                    StarsF = null;
                    PosterF = null;
                    DateAddF = DateTime.Now;
                }
                else
                    MessageBox.Show("Error Insertion");
            }
            else
                MessageBox.Show("L'un des champs est vide.");
        }

        /**
         * Resume : 
         *      Add a serie in the Bdd and then reset attribute serie, TitleS, NbSeasonS, ContentS,DateAddS
         */
        private void AddSerie()
        {
            if (TitleS != null && ContentS != null && GenreS != null && DirectorS != null && StarsS != null && PosterS != null)
            {
                if (serie.Add())
                {
                    MessageBox.Show("La série '" + TitleS + "' a bien été ajouté");
                    serie = new Serie();
                    TitleS = null;
                    GenreS = null;
                    ContentS = null;
                    DirectorS = null;
                    StarsS = null;
                    PosterS = null;
                    DateAddS = DateTime.Now;
                }
                else
                    MessageBox.Show("Error insertion");
            }
            else
                MessageBox.Show("L'un des champs est vide.");
        }


        /**
         * 
         */
        private void AddDiscover()
        {
            if(TitleD != null && ReleaseDateD != null)
            {
                if(discover.AddDiscover())
                {
                    MessageBox.Show("La découverte '" + TitleD + "' a bien été ajouté");
                    discover = new Discover();
                    TitleD = null;
                    ReleaseDateD = null;
                }
                else
                    MessageBox.Show("Error insertion");
            }
            else
                MessageBox.Show("L'un des champs est vide.");
        }

        /*
         * Remuse :
         *      opens a dialog box to choose a file.
         */
        private void AddPoster()
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == true)
            {
                PosterS = PosterF = MoveImageToImageFolder(open.FileName);
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
            File.Copy(urlToMove, urlAfterMove, true);
            return urlAfterMove;
        }
    }
}
