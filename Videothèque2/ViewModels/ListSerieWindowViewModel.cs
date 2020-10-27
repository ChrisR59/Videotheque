using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Videothèque2.Models;
using Videothèque2.Tools;

namespace Videothèque2.ViewModels
{
    public class ListSerieWindowViewModel : ViewModelBase
    {

        private ObservableCollection<Serie> listSerieView;
        private List<Rating> listRating;
        private List<SerieStatus> listStatus;
        private string searchTitleSerie;
        private Serie serie;

        public ObservableCollection<Serie> ListSerieView 
        { 
            get => listSerieView;
            set
            { 
                listSerieView = value;
                RaisePropertyChanged();
            }
        }
        public List<Rating> ListRating { get => listRating; set => listRating = value; }
        public List<SerieStatus> ListStatus { get => listStatus; set => listStatus = value; }
        public Serie Serie
        {
            get => serie;
            set
            {
                serie = value;
                if (serie == null)
                    serie = new Serie();
                RaisePropertyChanged("Id");
                RaisePropertyChanged("Title");
                RaisePropertyChanged("Genre");
                RaisePropertyChanged("RunTime");
                RaisePropertyChanged("ReleaseDate");
                RaisePropertyChanged("NbSeason");
                RaisePropertyChanged("NbEpisode");
                RaisePropertyChanged("Content");
                RaisePropertyChanged("Director");
                RaisePropertyChanged("Stars");
                RaisePropertyChanged("Poster");
                RaisePropertyChanged("LastView");
                RaisePropertyChanged("NbView");
                RaisePropertyChanged("Comment");
                RaisePropertyChanged("Status");
                RaisePropertyChanged("Rating");
            }
        }

        public int Id
        {
            get => Serie.Id;
            set
            {
                Serie.Id = value;
                RaisePropertyChanged("Id");
            }
        }
        public string Title
        {
            get => Serie.Title;
            set
            {
                Serie.Title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string Genre
        {
            get => Serie.Genre;
            set
            {
                Serie.Genre = value;
                RaisePropertyChanged("Genre");
            }
        }
        public string RunTime
        {
            get => Serie.RunTime;
            set
            {
                Serie.RunTime = value;
                RaisePropertyChanged("RunTime");
            }
        }
        public string ReleaseDate
        {
            get => Serie.ReleaseDate;
            set
            {
                Serie.ReleaseDate = value;
                RaisePropertyChanged("ReleaseDate");
            }
        }
        public int NbSeason
        {
            get => Serie.NbSeason;
            set
            {
                Serie.NbSeason = value;
                RaisePropertyChanged("NbSeason");
            }
        }
        public int NbEpisode
        {
            get => Serie.NbEpisode;
            set
            {
                Serie.NbEpisode = value;
                RaisePropertyChanged("NbEpisode");
            }
        }
        public string Content
        {
            get => Serie.Content;
            set
            {
                Serie.Content = value;
                RaisePropertyChanged("Content");
            }
        }
        public string Director
        {
            get => Serie.Director;
            set
            {
                Serie.Director = value;
                RaisePropertyChanged("Director");
            }
        }
        public string Stars
        {
            get => Serie.Stars;
            set
            {
                Serie.Stars = value;
                RaisePropertyChanged("Stars");
            }
        }
        public string Poster
        {
            get => Serie.Poster;
            set
            {
                Serie.Poster = value;
                RaisePropertyChanged("Poster");
            }
        }
        public DateTime LastView
        {
            get => Serie.LastView;
            set
            {
                Serie.LastView = value;
                RaisePropertyChanged("LastView");
            }
        }
        public int NbView
        {
            get => Serie.NbView;
            set
            {
                Serie.NbView = value;
                RaisePropertyChanged("NbView");
            }
        }
        public string ToWatchString
        {
            get => Serie.ToWatchString;
            set
            {
                Serie.ToWatchString = value;
                RaisePropertyChanged("ToWatchString");
            }
        }
        public string Comment
        {
            get => Serie.Comment;
            set
            {
                Serie.Comment = value;
                RaisePropertyChanged("Comment");
            }
        }
        public SerieStatus Status
        {
            get => Serie.Status;
            set
            {
                Serie.Status = value;
                RaisePropertyChanged("Status");
            }
        }
        public Rating Rating
        {
            get => Serie.Rating;
            set
            {
                Serie.Rating = value;
                RaisePropertyChanged("Rating");
            }
        }
        public string SearchTitleSerie { 
            get => searchTitleSerie;
            set 
            { 
                searchTitleSerie = value;
                RaisePropertyChanged();
            }
        }

        //Command
        public ICommand EditSerieCommand { get; set; }
        public ICommand DeleteSerieCommand { get; set; }
        public ICommand ProgramEltCommand { get; set; }
        public ICommand EditPosterCommand { get; set; }
        public ICommand SearchSerieCommand { get; set; }

        /*
         * Resume : 
         *      Initialize a serie
         *      Initialize a commands with a methode as a parameter
         *      For the EditSerieCommand added EditCanExecute in parameter
         */
        public ListSerieWindowViewModel()
        {
            Serie = new Serie();
            ListSerieView = Serie.GetAll();
            ListRating = Enum.GetValues(typeof(Rating)).Cast<Rating>().ToList();
            ListStatus = Enum.GetValues(typeof(SerieStatus)).Cast<SerieStatus>().ToList();
            EditSerieCommand = new RelayCommand(EditSerie, EditCanExecute);
            DeleteSerieCommand = new RelayCommand(DeleteSerie);
            ProgramEltCommand = new RelayCommand(ProgramSerie);
            EditPosterCommand = new RelayCommand(EditPoster);
            SearchSerieCommand = new RelayCommand(SearchSerie);
        }

        /*
         * Resume :
         *      opens a dialog box to choose a file.
         */
        private void EditPoster()
        {
            OpenFileDialog open = new OpenFileDialog();

            if (open.ShowDialog() == true)
                Poster = MoveImageToImageFolder(open.FileName);
        }

        /**
         * Resume :
         *      Check if the button "Modifié" can be use
         */
        private Boolean EditCanExecute()
        {
            Boolean res = true;

            if (Id == 0)
                res = false;

            return res;
        }

        /**
         * Resume :
         *      Confirm a editing of a serie 
         *      Update a serie with a MessageBoxResult for confirmation
         *      Edit in the Bdd with the UpdateSerie method
         *      Call EditList Method for Maj a serie list
         */
        private void EditSerie()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment modifier " + Title + "?", "Confirmation modification", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Title != null && Genre != null && Content != null && Director != null && Stars != null && Poster != null)
                {
                    if (Serie.UpdateOne())
                    {
                        MessageBox.Show(Title + " a bien été modifié.");
                        UpList();
                    }
                }
            }
        }

        /**
         * Resume :
         *      Confirm a deleting of a serie
         *      Delete a serie with a MessageBoxResult for confirmation
         *      Delete in the Bdd with the DeleteSerie method
         C      call EditList Method for Maj a serie list
         */
        private void DeleteSerie()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer " + Serie.Title + "?", "Confirmation Suppression", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Serie.Id != 0)
                {
                    if (Serie.DeleteOne())
                    {
                        MessageBox.Show(Serie.Title + " a bien été supprimé.");
                        UpList();
                    }
                }
            }
        }

        /**
         * Resume : 
         *      Maj of the serie list
         *      Call GetSeries method for initialize a list
         */
        private void UpList()
        {
            ListSerieView = Serie.GetAll();
            RaisePropertyChanged("ListSerieView");
        }

        /**
         * Resume :
         *      Confirm a programming of an elements in the list. Attribute ToWatch
         *      Update a serie with a MessageBoxResult for confirmation
         *      Edit in the Bdd with the UpdateElement method
         *      Call EditList Method for Maj a list of the serie
         */
        private void ProgramSerie()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment programmer " + Serie.Title + "?", "Confirmation programmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Serie != null)
                {
                    Serie.ToWatch = true;
                    Serie.UpdateToWatchSerie();
                    MessageBox.Show(Serie.Title + " a bien été programmé.");
                    UpList();
                }
            }
        }

        /**
         * Resume : 
         *      Search Serie in the list or Get complete list
         */
        private void SearchSerie()
        {
            ListSerieView = Serie.GetSearch(SearchTitleSerie);

            if (SearchTitleSerie == "" || SearchTitleSerie == null)
                ListSerieView = Serie.GetAll();

            SearchTitleSerie = null;
        }

        /*
         * Resume : 
         *      Copy the file to its new location
         */
        private string MoveImageToImageFolder(string urlToMove)
        {
            if (!Directory.Exists("posters"))
                Directory.CreateDirectory("posters");

            string urlAfterMove = Path.Combine(Directory.GetCurrentDirectory(), "posters", Path.GetFileName(urlToMove));
            File.Copy(urlToMove, urlAfterMove, true);
            return urlAfterMove;
        }
    }
}
