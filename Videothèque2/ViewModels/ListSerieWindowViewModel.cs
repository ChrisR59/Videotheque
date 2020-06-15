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
        private Serie serie;

        public ObservableCollection<Serie> ListSerieView { get => listSerieView; set => listSerieView = value; }
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


        public ICommand EditSerieCommand { get; set; }
        public ICommand DeleteSerieCommand { get; set; }
        public ICommand ProgramEltCommand { get; set; }

        public ListSerieWindowViewModel()
        {
            Serie = new Serie();
            ListSerieView = Serie.GetSerie();
            EditSerieCommand = new RelayCommand(EditSerie, EditCanExecute);
            DeleteSerieCommand = new RelayCommand(DeleteSerie);
            ProgramEltCommand = new RelayCommand(EditElt);
        }

        /**
         * Verifie si le bouton peux être utiliser
         */
        private Boolean EditCanExecute()
        {
            Boolean res = true;

            if (Title == null && Content == null)
                res = false;

            return res;
        }

        /**
         * Modifie la série selectionné
         */
        private void EditSerie()
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

        /**
         * Supprime la série selectionné
         */
        private void DeleteSerie()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer cette série?", "Confirmation Suppression", System.Windows.MessageBoxButton.YesNo);
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
         * Maj de la liste affiché
         */
        private void EditList()
        {
            ListSerieView = Serie.GetSerie();
            RaisePropertyChanged("ListSerieView");
        }

        /**
         * Confirme la programmation d'un élément de la liste
         */
        private void EditElt()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment programmer cette série?", "Confirmation programmation", System.Windows.MessageBoxButton.YesNo);
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
