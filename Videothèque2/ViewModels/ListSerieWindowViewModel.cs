using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Videothèque2.Models;

namespace Videothèque2.ViewModels
{
    public class ListSerieWindowViewModel : ViewModelBase
    {

        private ObservableCollection<Serie> listSerieView;
        public ObservableCollection<Serie> ListSerieView { get => listSerieView; set => listSerieView = value; }
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
                RaisePropertyChanged("NbSeason");
                RaisePropertyChanged("Content");
                RaisePropertyChanged("LastView");
                RaisePropertyChanged("NbView");
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
        public string NbSeason
        {
            get => Serie.NbSeason;
            set
            {
                Serie.NbSeason = value;
                RaisePropertyChanged("NbSeason");
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

        //Command
        public ICommand EditSerieCommand { get; set; }
        public ICommand DeleteSerieCommand { get; set; }
        public ICommand ProgramEltCommand { get; set; }

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
            EditSerieCommand = new RelayCommand(EditSerie, EditCanExecute);
            DeleteSerieCommand = new RelayCommand(DeleteSerie);
            ProgramEltCommand = new RelayCommand(EditElt);
        }

        /**
         * Resume :
         *      Check if the button "Modifieré can be use
         */
        private Boolean EditCanExecute()
        {
            Boolean res = true;

            if (Title == null && Content == null)
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
                if (Serie.Title != null && Serie.Content != null)
                {
                    if (Serie.UpdateSerie())
                    {
                        MessageBox.Show("La serie a bien été modifié.");
                        EditList();
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
                        EditList();
                    }
                }
            }
        }

        /**
         * Resume : 
         *      Maj of the serie list
         *      Call GetSeries method for initialize a list
         */
        private void EditList()
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
        private void EditElt()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment programmer cette série?", "Confirmation programmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Serie != null)
                {
                    Serie.ToWatch = true;
                    Serie.UpdateElement();
                    MessageBox.Show("La serie a bien été programmé.");
                    EditList();
                }
            }
        }
    }
}
