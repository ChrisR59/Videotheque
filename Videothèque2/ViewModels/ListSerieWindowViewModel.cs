using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class ListSerieWindowViewModel:ViewModelBase
    {
        public ObservableCollection<Serie> ListSerieView { get; set; }
        private Serie serie = new Serie();

        public int Id
        {
            get => Serie.Id;
            set
            {
                Serie.Id = value;
                RaisePropertyChanged();
            }
        }
        public string Title
        {
            get => Serie.Title;
            set
            {
                Serie.Title = value;
                RaisePropertyChanged();
            }
        }

        public string NbSeason
        {
            get => Serie.NbSeason;
            set
            {
                Serie.NbSeason = value;
                RaisePropertyChanged();
            }
        }

        public string Content
        {
            get => Serie.Content;
            set
            {
                Serie.Content = value;
                RaisePropertyChanged();
            }
        }

        public DateTime LastView
        {
            get => Serie.LastView;
            set
            {
                Serie.LastView = value;
                RaisePropertyChanged();
            }
        }

        public int NbView
        {
            get => Serie.NbView;
            set
            {
                Serie.NbView = value;
                RaisePropertyChanged();
            }
        }

        public ICommand EditSerieCommand { get; set; }
        public ICommand SelectListCommand { get; set; }
        public ICommand DeleteSerieCommand { get; set; }
        public Serie Serie { get => serie; set => serie = value; }

        public ListSerieWindowViewModel()
        {
            ListSerieView = Serie.GetSerie();
            EditSerieCommand = new RelayCommand(EditSerie);
            SelectListCommand = new RelayCommand(SelectSerie);
            DeleteSerieCommand = new RelayCommand(DeleteSerie);
        }

        private void SelectSerie()
        {
            Id = Serie.Id;
            Title = Serie.Title;
            NbSeason = Serie.NbSeason;
            NbView = Serie.NbView;
            Content = Serie.Content;
            LastView = Serie.LastView;
        }

        private void EditSerie()
        {
            if (Serie.UpdateSerie())
            {
                MessageBox.Show("La serie a bien été modifié.");
            }
        }

        private void DeleteSerie()
        {
            if (Serie.DeleteSerie())
            {
                MessageBox.Show("La serie a bien été supprimé.");
            }
        }
    }
}
