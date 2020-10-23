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
    class DiscoverWindowViewModel : ViewModelBase
    {
        private ObservableCollection<Discover> listDiscover;
        private Discover discover;
        public ObservableCollection<Discover> ListDiscover { get => listDiscover; set => listDiscover = value; }
        public Discover Discover
        {
            get => discover;
            set
            {
                discover = value;
                if (discover == null)
                    discover = new Discover();

                RaisePropertyChanged("Title");
                RaisePropertyChanged("ReleaseDate");
                RaisePropertyChanged("Comment");
            }
        }
        public string Title
        {
            get => Discover.Title;
            set
            {
                Discover.Title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string ReleaseDate
        {
            get => Discover.ReleaseDate;
            set
            {
                Discover.ReleaseDate = value;
                RaisePropertyChanged("ReleaseDate");
            }
        }
        public string Comment
        {
            get => Discover.Comment;
            set
            {
                Discover.Comment = value;
                RaisePropertyChanged("Comment");
            }
        }

        public ICommand DeleteDiscoverCommand { get; set; }
        public ICommand EditDiscoverCommand { get; set; }
        public ICommand ProgramEltCommand { get; set; }

        /**
         * Resume :
         *      Initalize a Discover
         *      Call the GetAllDiscover methode to get a list of Discover
         *      Initialize a commands with a methode as a parameter
         */
        public DiscoverWindowViewModel()
        {
            Discover = new Discover();
            ListDiscover = Discover.GetAll();

            DeleteDiscoverCommand = new RelayCommand(DelDiscover);
            EditDiscoverCommand = new RelayCommand(EditDiscover);
            ProgramEltCommand = new RelayCommand(ProgramDiscover);
        }

        /**
         * Resume :
         *      Confirm a editing of a Discover 
         *      Update a Discover with a MessageBoxResult for confirmation
         *      edit in the Bdd with the EditOneDiscover method
         */
        private void EditDiscover()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment modifier " + Discover.Title + "?", "Confirmation modification", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Title != null && ReleaseDate != null)
                {
                    if (Discover.UpdateOne())
                    {
                        MessageBox.Show(Discover.Title + " a bien été modifié.");
                        ListDiscover = Discover.GetAll();
                        RaisePropertyChanged("ListDiscover");
                    }
                }
            }
        }

        /**
         * Resume :
         *      Confirm a programming of an elements in the list. Attribute ToWatch
         *      Update a Discover with a MessageBoxResult for confirmation
         *      edit in the Bdd with the UpProgrammDiscover method
         */
        private void ProgramDiscover()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment programmer " + Discover.Title + "?", "Confirmation programmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Discover != null)
                {
                    Discover.ToWatch = true;
                    Discover.UpadateToWatchDiscover();
                    MessageBox.Show(Discover.Title + " a bien été programmé.");
                    ListDiscover = Discover.GetAll();
                    RaisePropertyChanged("ListDiscover");
                }
            }
        }

        /**
         * Resume :
         *      Confirm a deleting of a Discover
         *      Delete a Discover with a MessageBoxResult for confirmation
         *      Delete in the Bdd with the DeleteOneDiscover method
         */
        private void DelDiscover()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous vraiment supprimer " + Discover.Title + "?", "Confirmation Suppression", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (Discover.Id != 0)
                {
                    if (Discover.DeleteOne())
                    {
                        MessageBox.Show(Discover.Title + " a bien été supprimé.");
                        ListDiscover = Discover.GetAll();
                        RaisePropertyChanged("ListDiscover");
                    }
                }
            }
        }

    }
}
