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
        private Film film;
        private ObservableCollection<Film> listFilmView;

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
                RaisePropertyChanged("Content");
                RaisePropertyChanged("LastView");
                RaisePropertyChanged("NbView");
            }
        }
        public ObservableCollection<Film> ListFilmView { get => listFilmView; set => listFilmView = value; }
        
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
        }


        public ICommand EditFilmCommand { get; set; }
        public ICommand DeleteFilmCommand { get; set; }

        public ListFilmWindowViewModel()
        {
            Film = new Film();
            ListFilmView = Film.GetFilms();
            EditFilmCommand = new RelayCommand(EditFilm, EditCanExecute);
            DeleteFilmCommand = new RelayCommand(DeleteFilm);
        }

        private Boolean EditCanExecute()
        {
            Boolean res = true;

            if(Title == null && Content == null)
                res = false;

            return res;
        }

        private void EditFilm()
        {
            if (Film.Title != null && Film.Content != null)
            {
                if (Film.UpdateFilm())
                {
                    MessageBox.Show("Le Film a bien été modifié.");
                    EditList();
                }
            }
        }

        private void DeleteFilm()
        {
            if(Film.Id != 0)
            {
                if (Film.DeleteFilm())
                {
                    MessageBox.Show("Le Film a bien été supprimé.");
                    EditList();
                }
            }
        }

        private void EditList()
        {
            ListFilmView = Film.GetFilms();
            RaisePropertyChanged("ListFilmView");
        }
    }
}
