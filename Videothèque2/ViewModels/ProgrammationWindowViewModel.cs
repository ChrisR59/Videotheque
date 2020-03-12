using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videothèque2.Models;

namespace Videothèque2.ViewModels
{
    class ProgrammationWindowViewModel
    {
        private ObservableCollection<Element> listElements = new ObservableCollection<Element>();
        private Element e = new Element();

        public ObservableCollection<Element> ListElements { get => listElements; set => listElements = value; }

        public ProgrammationWindowViewModel()
        {
            listElements = e.GetElements();
        }
    }
}
