using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videothèque2.Models
{
    public class CycleContent
    {
        private int id;
        private string title;
        private string status;
        private int rank;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Status { get => status; set => status = value; }
        public int Rank { get => rank; set => rank = value; }

        public void AddElement()
        {

        }
    }
}
