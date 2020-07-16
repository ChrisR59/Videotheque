using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Videothèque2.Models;
using Videothèque2.Tools;

namespace Videothèque2.ViewModels
{
    public class ListFilmWindowViewModel:ViewModelBase
    {
        private ObservableCollection<Film> listFilmView;
        private List<Rating> listRating;
        public ObservableCollection<Film> ListFilmView { get => listFilmView; set => listFilmView = value; }
        public List<Rating> ListRating { get => listRating; set => listRating = value; }

        private Film film;
        public Film Film
        {
            get => film;
            set
            {
                film = value;
                if (film == null)
                    film = new Film();
                RaisePropertyChanged("Id");
                RaisePropertyChanged("Title");
                RaisePropertyChanged("Genre");
                RaisePropertyChanged("Content");
                RaisePropertyChanged("Director");
                RaisePropertyChanged("Stars");
                RaisePropertyChanged("Poster");
                RaisePropertyChanged("LastView");
                RaisePropertyChanged("NbView");
                RaisePropertyChanged("Comment");
                RaisePropertyChanged("Rating");
            }
        }
        

        //List a attributes a film
        public int Id
        {
            get => Film.Id;
            set
            {
                Film.Id = value;
                RaisePropertyChanged("Id");
            }
        }
        public string Title
        {
            get => Film.Title;
            set
            {
                Film.Title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string Genre
        {
            get => Film.Genre;
            set {
                Film.Genre = value;
                RaisePropertyChanged("Genre");
            }
        }        
        public string Content
        {
            get => Film.Content;
            set
            {
                Film.Content = value;
                RaisePropertyChanged("Content");
            }
        }
        public string Director
        {
            get => Film.Director;
            set
            {
                Film.Director = value;
                RaisePropertyChanged("Director");
            }
        }
        public string Stars
        {
            get => Film.Stars;
            set
            {
                Film.Stars = value;
                RaisePropertyChanged("Stars");
            }
        }
        public string Poster
        {
            get => Film.Poster;
            set
            {
                Film.Poster = value;
                RaisePropertyChanged("Poster");
            }
        }
        public DateTime LastView
        {
            get => Film.LastView;
            set
            {
                Film.LastView = value;
                RaisePropertyChanged("LastView");
            }
        }
        public int NbView
        {
            get => Film.NbView;
            set
            {
                Film.NbView = value;
                RaisePropertyChanged("NbView");
            }
        }
        public string ToWatchString
        {
            get => Film.ToWatchString;
            set
            {
                Film.ToWatchString = value;
                RaisePropertyChanged("ToWatchString");
            }
        }
        public string Comment
        {
            get => Film.Comment;
            set
            {
                Film.Comment = value;
                RaisePropertyChanged("Comment");
            }
        }
        public Rating Rating
        {
            get => Film.Rating;
            set
            {
                Film.Rating = value;
                RaisePropertyChanged("Rating");
            }
        }



        //Command
        public ICommand EditFilmCommand { get; set; }
        public ICommand DeleteFilmCommand { get; set; }
        public ICommand ProgramEltCommand { get; set; }
        public ICommand EditPosterCommand { get; set; }

        /*
         * Resume :
         *      Initalize a film
         *      Call the GetFilms methode to get a list of film
         *      Initialize a commands with a methode as a parameter
         *      For the EditFilmCommand added EditCanExecute in parameter
         */
        public ListFilmWindowViewModel()
        {
            Film = new Film();
            ListFilmView = Film.GetFilms();
            ListRating = Enum.GetValues(typeof(Rating)).Cast<Rating>().ToList();
            EditFilmCommand = new RelayCommand(EditFilm, EditCanExecute);
            DeleteFilmCommand = new RelayCommand(DeleteFilm);
            ProgramEltCommand = new RelayCommand(ProgramFilm);
            EditPosterCommand = new RelayCommand(EditPoster);
        }

        /*
         * Remuse :
         *      opens a dialog box to choose a file.
         */
        private void EditPoster()
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == true)
            {
                Poster = MoveImageToImageFolder(open.FileName);
            }
        }

        /**
         * Resume :
         *      Check if the button "Modifieré can be use
         */
        private Boolean EditCanExecute()
        {
            Boolean res = true;

            if(Title == null && Content == null)
                res = false;

            return res;
        }

        /**
         * Resume :
         *      Confirm a editing of a film 
         *      Update a film with a MessageBoxResult for confirmation
         *      edit in the Bdd with the UpdateFilm method
         *      call EditList Method for Maj a list of the film
         */
        private void EditFilm()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment modifier ce film?", "Confirmation modification", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Film.Title != null && Film.Content != null)
                {
                    if (Film.UpdateFilm())
                    {
                        MessageBox.Show("Le Film a bien été modifié.");
                        UpList();
                    }
                }
            }
        }

        /**
         * Resume :
         *      Confirm a programming of an elements in the list. Attribute ToWatch
         *      Update a film with a MessageBoxResult for confirmation
         *      edit in the Bdd with the UpdateElement method
         *      call EditList Method for Maj a list of the film
         */
        private void ProgramFilm()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment programmer ce film?", "Confirmation programmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Film != null)
                {
                    Film.ToWatch = true;
                    Film.UpFilmProgramm();
                    UpList();
                }
            }
        }

        /**
         * Resume :
         *      Confirm a deleting of a film
         *      Delete a film with a MessageBoxResult for confirmation
         *      Delete in the Bdd with the DeleteFilm method
         *      call EditList Method for Maj a list of the film
         */
        private void DeleteFilm()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer ce film?", "Confirmation Suppression", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Film.Id != 0)
                {
                    if (Film.DeleteFilm())
                    {
                        MessageBox.Show("Le Film a bien été supprimé.");
                        UpList();
                    }
                }
            }
        }

        /**
         * Resume : 
         *      Maj of the film list
         *      Call GetFilms method for initialize a list
         */
        private void UpList()
        {
            ListFilmView = Film.GetFilms();
            RaisePropertyChanged("ListFilmView");
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
