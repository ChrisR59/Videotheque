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
        public ObservableCollection<Serie> ListSerieView { get => listSerieView; set => listSerieView = value; }
        public List<Rating> ListRating { get => listRating; set => listRating = value; }

        private Serie serie;
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
                RaisePropertyChanged("RunningTime");
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
                RaisePropertyChanged("Rating");
            }
        }

        //List a attributes a serie
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
        public string RunningTime
        {
            get => Serie.RunningTime;
            set
            {
                Serie.RunningTime = value;
                RaisePropertyChanged("RunningTime");
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
        public Rating Rating
        {
            get => Serie.Rating;
            set
            {
                Serie.Rating = value;
                RaisePropertyChanged("Rating");
            }
        }

        //Command
        public ICommand EditSerieCommand { get; set; }
        public ICommand DeleteSerieCommand { get; set; }
        public ICommand ProgramEltCommand { get; set; }
        public ICommand EditPosterCommand { get; set; }

        /*
         * Resume : 
         *      Initialize a serie
         *      Initialize a commands with a methode as a parameter
         *      For the EditSerieCommand added EditCanExecute in parameter
         */
        public ListSerieWindowViewModel()
        {
            Serie = new Serie();
            ListSerieView = Serie.GetSerie();
            ListRating = Enum.GetValues(typeof(Rating)).Cast<Rating>().ToList();
            EditSerieCommand = new RelayCommand(EditSerie, EditCanExecute);
            DeleteSerieCommand = new RelayCommand(DeleteSerie);
            ProgramEltCommand = new RelayCommand(ProgramSerie);
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
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment modifier cette série?", "Confirmation modification", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Title != null && Genre != null && Content != null && Director != null && Stars != null && Poster != null)
                {
                    if (Serie.UpdateSerie())
                    {
                        MessageBox.Show("La serie a bien été modifié.");
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
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer cette série?", "Confirmation Suppression", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Serie.Id != 0)
                {
                    if (Serie.DeleteSerie())
                    {
                        MessageBox.Show("La serie a bien été supprimé.");
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
            ListSerieView = Serie.GetSerie();
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
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment programmer cette série?", "Confirmation programmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Serie != null)
                {
                    Serie.ToWatch = true;
                    Serie.UpSerieProgramm();
                    MessageBox.Show("La serie a bien été programmé.");
                    UpList();
                }
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
