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
using Videothèque2.Views;

namespace Videothèque2.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
        private ObservableCollection<CycleContent> listCycleContent;
        private Element element;
        private CycleContent cycleC;
        private CycleStatus cycleS;
        public ObservableCollection<Element> ListElements { get; set; }
        public ObservableCollection<CycleContent> ListCycleContent { get => listCycleContent; set => listCycleContent = value; }
        public Element Element { get => element; set => element = value; }
        public CycleContent CycleC { get => cycleC; set => cycleC = value; }
        public CycleStatus CycleS { get => cycleS; set => cycleS = value; }


        public ICommand ProgramEltCommand { get; set; }//Button
        public ICommand UpdateListCommand { get; set; }//Button
        public ICommand LFilmCommand { get; set; }
        public ICommand LSerieCommand { get; set; }
        public ICommand AEltCommand { get; set; }
        public ICommand CycleCommand { get; set; }

        public MainWindowViewModel()
        {
            //Recuperer l'id du cycle en cours
            Element = new Element();
            CycleS = new CycleStatus();
            CycleC = new CycleContent();
            CycleS.GetIdCycle();
            ListElements = Element.GetProgram();
            ListCycleContent = CycleC.GetCycleActually(CycleS.Id);

            LFilmCommand = new RelayCommand(() =>
            {
                ListFilmWindow lFw = new ListFilmWindow();
                lFw.Show();
            });

            LSerieCommand = new RelayCommand(() =>
            {
                ListSerieWindow lSw = new ListSerieWindow();
                lSw.Show();
            });

            AEltCommand = new RelayCommand(() =>
            {
                AddElementWindow aEw = new AddElementWindow();
                aEw.Show();
            });

            CycleCommand = new RelayCommand(() =>
           {
               GestionCycleWindow gc = new GestionCycleWindow();
               gc.Show();
           });

            ProgramEltCommand = new RelayCommand(EditElt);
            UpdateListCommand = new RelayCommand(UpList);
        }

        private void EditElt()
        {
            if (CycleC != null)
            {
                if (CycleC.Type == "Film")
                {
                    Film f = CycleC.GetOneFilm();
                    f.LastView = DateTime.Now;
                    f.NbView++;
                    f.ToWatch = false;
                    Boolean res = f.UpdateLastView();
                    if (res)
                        MessageBox.Show("Le date du film a bien été modifié");
                }
                else if (CycleC.Type == "Serie")
                {
                    Serie s = CycleC.GetOneSerie();
                    s.LastView = DateTime.Now;
                    s.NbView++;
                    s.ToWatch = false;
                    Boolean res = s.UpdateLastView();
                    if (res)
                        MessageBox.Show("La date de la série a bien été modifié");
                }
                Element.ToWatch = false;//ligne à supprimer?
                UpList();
            }
        }

        private void UpList()
        {
            CycleC = new CycleContent();
            ListCycleContent = CycleC.GetCycleActually(CycleS.Id);
            RaisePropertyChanged("ListCycleContent");
        }
    }
}
