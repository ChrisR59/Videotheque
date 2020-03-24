using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Videothèque2.Models;

namespace Videothèque2.ViewModels
{
    public class ListFilmWindowViewModel:ViewModelBase
    {
        private Film film = new Film();
        private ObservableCollection<Film> listFilmView;

        public Film Film { get => film; set => film = value; }
        public ObservableCollection<Film> ListFilmView { get => listFilmView; set => listFilmView = value; }
        /*
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

        public string Content
        {
            get => Film.Content;
            set
            {
                Film.Content = value;
                RaisePropertyChanged("Content");
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
        }*/


        public ICommand EditFilmCommand { get; set; }
        public ICommand DeleteFilmCommand { get; set; }
        public ICommand SelectFilmCommand { get; set; }

        public ListFilmWindowViewModel()
        {
            ListFilmView = Film.GetFilms();
            EditFilmCommand = new RelayCommand(EditFilm,EditFilmCanExecute);
            DeleteFilmCommand = new RelayCommand(DeleteFilm);
            SelectFilmCommand = new RelayCommand(GetFilm);
        }

        private void GetFilm()
        {
            /*Id = Film.Id;
            Title = Film.Title;
            Content = Film.Content;
            NbView = Film.NbView;
            LastView = Film.LastView;*/
        }

        private Boolean EditFilmCanExecute()
        {
            Boolean res = true;
            if(Film.Title == null && Film.Content == null)
            {
                res = false;
            }
            return res;
        }

        private void EditFilm()
        {
            if (Film.UpdateFilm())
            {
                MessageBox.Show("Le Film a bien été modifié.");
                EditList();
            }
        }

        private void DeleteFilm()
        {
            if (Film.DeleteFilm())
            {
                MessageBox.Show("Le Film a bien été modifié.");
                EditList();
            }
        }

        private void EditList()
        {
            ListFilmView = Film.GetFilms();
            RaisePropertyChanged("ListFilmView");
        }
    }
}
