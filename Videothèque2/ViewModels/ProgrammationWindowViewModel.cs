using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Videothèque2.Models;

namespace Videothèque2.ViewModels
{
    class ProgrammationWindowViewModel:ViewModelBase
    {
        public ObservableCollection<Element> ListElements { get; set; }
        private Element element = new Element();

        public int Id
        {
            get => Element.Id;
            set
            {
                Element.Id = value;
                RaisePropertyChanged("Id");
            }
        }

        public string Title
        {
            get => Element.Title;
            set
            {
                Element.Title = value;
                RaisePropertyChanged("Title");
            }
        }
        
        public string ToWatchString
        {
            get => Element.ToWatchString;
            set
            {
                Element.ToWatchString = value;
                RaisePropertyChanged("ToWatchString");
            }
        }

        public Element Element { get => element; set => element = value; }
        public ICommand ProgramEltCommand { get; set; }

        public ProgrammationWindowViewModel()
        {
            ListElements = Element.GetElements();
            ProgramEltCommand = new RelayCommand(EditElt);
        }

        private void EditElt()
        {
            if (Element != null)
            {
                Element.ToWatch = true;
                Element.UpdateElement();
                ListElements = Element.GetElements();
                RaisePropertyChanged("ListElements");
            }
        }
    }
}
