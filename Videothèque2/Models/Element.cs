using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videothèque2.Tools;

namespace Videothèque2.Models
{
    /*
     * Resume : 
     *      An element can be a Film or a Serie
     */
    public class Element : INotifyPropertyChanged
    {
        private int id;
        private string title;
        private int place;
        private Boolean toWatch;
        private string toWatchString;
        private string type;

        public int Id { get => id; set => id = value; }
        public string Title { 
            get => title; 
            set  { 
                title = value;
                NotifyPropertyChange("Title");
            }
        }
        public int Place { get => place; set => place = value; }
        public bool ToWatch { get => toWatch; set => toWatch = value; }
        public string ToWatchString { get => toWatchString; set => toWatchString = value; }
        public string Type { get => type; set => type = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        /*
         * Resume :
         *      Get a element list which "ToWatch" = 1 in the base of the Film and Serie
         * Return an ObservableCollection of the Element type Which "ToWatch" = 1
         */
        public ObservableCollection<Element> GetProgram()
        {
            ObservableCollection<Element> l = new ObservableCollection<Element>();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Films WHERE ToWatch = 1", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Element e = new Element();
                e.Id = DataBase.Instance.reader.GetInt32(0);
                e.Title = DataBase.Instance.reader.GetString(1);
                e.Type = "Film";
                int w = DataBase.Instance.reader.GetInt32(2);
                if (w == 1)
                    e.ToWatch = true;

                l.Add(e);
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Series WHERE ToWatch = 1", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Element s = new Element();
                s.Id = DataBase.Instance.reader.GetInt32(0);
                s.Title = DataBase.Instance.reader.GetString(1);
                s.Type = "Serie";
                int w = DataBase.Instance.reader.GetInt32(2);
                if (w == 1)
                    s.ToWatch = true;
                l.Add(s);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
            return l;
        }

        /*
         * Resume :
         *      Get one film with his Id
         * Return an Film object 
         */
        public Film GetOneFilm()
        {
            Film f = new Film();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,LastView,ToWatch FROM Films WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", Id));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                f.Id = DataBase.Instance.reader.GetInt32(0);
                f.Title = DataBase.Instance.reader.GetString(1);
                if(!DataBase.Instance.reader.IsDBNull(2))
                    f.LastView = DataBase.Instance.reader.GetDateTime(2);
                int w = DataBase.Instance.reader.GetInt32(3);
                if (w == 1)
                    f.ToWatch = true;
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return f;
        }

        /*
         * Resume :
         *      Get one serie with his Id
         * Return an Serie object 
         */
        public Serie GetOneSerie()
        {
            Serie s = new Serie();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,LastView,ToWatch FROM Series WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", Id));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                s.Id = DataBase.Instance.reader.GetInt32(0);
                s.Title = DataBase.Instance.reader.GetString(1);
                if(!DataBase.Instance.reader.IsDBNull(2))
                    s.LastView = DataBase.Instance.reader.GetDateTime(2);
                int w = DataBase.Instance.reader.GetInt32(3);
                if (w == 1)
                    s.ToWatch = true;
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return s;
        }

        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
        }
    }
}
