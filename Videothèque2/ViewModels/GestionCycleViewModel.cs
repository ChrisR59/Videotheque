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
    class GestionCycleViewModel : ViewModelBase
    {
        private ObservableCollection<int> listCycle;
        private ObservableCollection<Element> listElements;
        private CycleStatus cycleS;
        private CycleContent cycleC;
        private Element element;
        private int idCycle;

        public ObservableCollection<int> ListCycle { get => listCycle; set => listCycle = value; }
        public ObservableCollection<Element> ListElements { get => listElements; set => listElements = value; }
        public CycleStatus CycleS { get => cycleS; set => cycleS = value; }
        public CycleContent CycleC { get => cycleC; set => cycleC = value; }
        public Element Element { get => element; set => element = value; }
        public int IdCycle { get => idCycle; set => idCycle = value; }

        public Status Status
        {
            get => CycleS.StatusC;
            set
            {
                CycleS.StatusC = value;
                RaisePropertyChanged("StatusC");
            }

        }


        public ICommand AddCycleCommand { get; set; }
        public ICommand AddEltCycleCommand { get; set; }

        public GestionCycleViewModel()
        {
            CycleS = new CycleStatus();
            CycleC = new CycleContent();
            Element = new Element();
            ListCycle = CycleS.GetListCycle();
            ListElements = Element.GetProgram();
            AddCycleCommand = new RelayCommand(AddCycle);
            AddEltCycleCommand = new RelayCommand(AddElt);
        }

        public void AddCycle()
        {
            CycleS.CheckCycleExist();
            if(CycleS.AddCycle())
            {
                MessageBox.Show("Un nouveau Cycle a été crée.");//Verifier d'abord si un cycle exsite et son status
            }
        }

        public void AddElt()
        {
            CycleC.IdCycle = IdCycle;
            CycleC.GetRank();
            CycleC.Title = Element.Title;
            CycleC.Status = "A voir";
            if(CycleC.AddElement())
            {
                if (Element != null)
                {
                    if (Element.Type == "Film")
                    {
                        Film f = Element.GetOneFilm();
                        f.ToWatch = false;
                        Boolean res = f.UpdateElement();
                    }
                    else if (Element.Type == "Serie")
                    {
                        Serie s = Element.GetOneSerie();
                        s.ToWatch = false;
                        Boolean res = s.UpdateElement();
                    }
                }
                ListElements.Remove(Element);
                RaisePropertyChanged("ListElements");
                MessageBox.Show("Element bien ajouté au cycle.");
                CycleC.Rank = 0;
            }
        }
    }
}
