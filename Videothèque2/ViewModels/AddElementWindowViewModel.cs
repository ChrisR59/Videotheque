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
        private bool radioFilm;
        private bool radioSerie;
        private string title;
        private string genre;
        private string content;
        private string director;
        private string stars;
        private string poster;
        private DateTime dateAdd;

        public Film Film { get => film; set => film = value; }
        public Serie Serie { get => serie; set => serie = value; }
        public bool RadioFilm { get => radioFilm; set => radioFilm = value; }
        public bool RadioSerie { get => radioSerie; set => radioSerie = value; }
        public string Title
        {
            get => title;
            set
            {
                title = value;

                if (RadioFilm)
                    Film.Title = value;
                else if (RadioSerie)
                    Serie.Title = value;

                RaisePropertyChanged();
            }
        }
        public string Genre
        {
            get => genre;
            set
            {
                genre = value;

                if (RadioFilm)
                    Film.Genre = value;
                else if (RadioSerie)
                    Serie.Genre = value;

                RaisePropertyChanged();
            }
        }
        public string Content
        {
            get => content;
            set
            {
                content = value;

                if (RadioFilm)
                    Film.Content = value;
                else if (RadioSerie)
                    Serie.Content = value;

                RaisePropertyChanged();
            }
        }
        public string Director
        {
            get => director;
            set
            {
                director = value;

                if (RadioFilm)
                    Film.Director = value;
                else if (RadioSerie)
                    Serie.Director = value;

                RaisePropertyChanged();
            }
        }
        public string Stars
        {
            get => stars;
            set
            {
                stars = value;

                if (RadioFilm)
                    Film.Stars = value;
                else if (RadioSerie)
                    Serie.Stars = value;

                RaisePropertyChanged();
            }
        }
        public string Poster
        {
            get => poster;
            set
            {
                poster = value;

                if (RadioFilm)
                    Film.Poster = value;
                else if (RadioSerie)
                    Serie.Poster = value;

                RaisePropertyChanged();
            }
        }
        public DateTime DateAdd
        {
            get => dateAdd;
            set
            {
                dateAdd = value;

                if (RadioFilm)
                    Film.DateAdd = value;
                else if (RadioSerie)
                    Serie.DateAdd = value;

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
        public ICommand AddCommand { get; set; }
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
            Film = new Film();
            Serie = new Serie();
            discover = new Discover();
            DateAdd = DateTime.Now;

            AddCommand = new RelayCommand(AddElement);
            AddPosterCommand = new RelayCommand(AddPoster);
            ADiscoverCommand = new RelayCommand(AddDiscover);
        }



        private void AddElement()
        {
            if (Title != null && Content != null && Director != null && Genre != null && Stars != null && Poster != null)
            {
                if (RadioFilm)
                    AddFilm();
                else if (RadioSerie)
                    AddSerie();
            }
            else
                MessageBox.Show("L'un des champs est vide.");
        }
        /**
         * Resume :
         *      Add a movie in the Bdd and then reset attribute film, TitleF,ContentF, DateAddF
         */
        private void AddFilm()
        {
            if (Film.Add())
            {
                MessageBox.Show("Le Film '" + Film.Title + "' a bien été ajouté");
                Film = new Film();
                InitAttrib();
            }
            else
                MessageBox.Show("Error Insertion");
        }

        /**
         * Resume : 
         *      Add a serie in the Bdd and then reset attribute serie, TitleS, NbSeasonS, ContentS,DateAddS
         */
        private void AddSerie()
        {
            if (Serie.Add())
            {
                MessageBox.Show("La série '" + Title + "' a bien été ajouté");
                Serie = new Serie();
                InitAttrib();
            }
            else
                MessageBox.Show("Error insertion");
        }


        /**
         * 
         */
        private void AddDiscover()
        {
            if (TitleD != null && ReleaseDateD != null)
            {
                if (discover.AddDiscover())
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
                Poster = MoveImageToImageFolder(open.FileName);
            }
        }

        /**
         * 
         */
        private void InitAttrib()
        {
            Title = null;
            Genre = null;
            Content = null;
            Director = null;
            Stars = null;
            Poster = null;
            DateAdd = DateTime.Now;
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
