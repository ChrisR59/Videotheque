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
        private int idCycle;

        public int Id { get => id; set => id = value; }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                NotifyPropertyChange("Title");
            }
        }
        public int Place { get => place; set => place = value; }
        public bool ToWatch { get => toWatch; set => toWatch = value; }
        public string ToWatchString { get => toWatchString; set => toWatchString = value; }
        public string Type { get => type; set => type = value; }
        public int IdCycle { get => idCycle; set => idCycle = value; }

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

            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Discover WHERE ToWatch = 1", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Element s = new Element();
                s.Id = DataBase.Instance.reader.GetInt32(0);
                s.Title = DataBase.Instance.reader.GetString(1);
                s.Type = "Découverte";
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
         * Resume 
         *      Get an items list form the cycle
         * Parameter
         *      An Integer corresponding to the id of the current cycle
         * Return an ObservableCollection of the CycleContent type
         */
        public ObservableCollection<Element> GetCycle(int idCycleS)
        {
            ObservableCollection<Element> listC = new ObservableCollection<Element>();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,Rank,Type,IdElt,ToWatch FROM CycleContent WHERE IdCycle = @idCycle", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@idCycle", idCycleS));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Element c = new Element()
                {
                    Id = DataBase.Instance.reader.GetInt32(4),
                    Title = DataBase.Instance.reader.GetString(1),
                    Place = DataBase.Instance.reader.GetInt32(2),
                    Type = DataBase.Instance.reader.GetString(3),
                    IdCycle = DataBase.Instance.reader.GetInt32(0)
                };

                int w = DataBase.Instance.reader.GetInt32(4);
                c.ToWatch = false;
                if (w == 1)
                {
                    c.ToWatch = true;
                }
                listC.Add(c);
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return listC;
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
                if (!DataBase.Instance.reader.IsDBNull(2))
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
                if (!DataBase.Instance.reader.IsDBNull(2))
                    s.LastView = DataBase.Instance.reader.GetDateTime(2);
                int w = DataBase.Instance.reader.GetInt32(3);
                if (w == 1)
                    s.ToWatch = true;
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return s;
        }

        /*
         * Resume :
         *      Get one serie with his Id
         * Return an Discover object 
         */
        public Discover GetOneDiscover()
        {
            Discover d = new Discover();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Discover WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", Id));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                d.Id = DataBase.Instance.reader.GetInt32(0);
                d.Title = DataBase.Instance.reader.GetString(1);

                int w = DataBase.Instance.reader.GetInt32(2);
                if (w == 1)
                    d.ToWatch = true;
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return d;
        }

        /*
         * Resume :
         *      Removes an element FILM from a cycle
         * Return True is successfull
         */
        public Boolean UnWatchFilm()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Films SET ToWatch = @ToWatch WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@ToWatch", ToWatch));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", Id));
            DataBase.Instance.connection.Open();

            if (DataBase.Instance.command.ExecuteNonQuery() > 0)
            {
                res = true;
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }

        /*
         * Resume :
         *      Removes an element SERIE from a cycle
         * Return True is successfull
         */
        public Boolean UnWatchSerie()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Series SET ToWatch = @ToWatch WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@ToWatch", ToWatch));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", Id));
            DataBase.Instance.connection.Open();

            if (DataBase.Instance.command.ExecuteNonQuery() > 0)
            {
                res = true;
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
            return res;
        }

        /*
         * Resume :
         *      Removes an element Discover from a cycle
         * Return True is successfull
         */
        public Boolean UnWatchDiscover()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Discover SET ToWatch = @ToWatch WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@ToWatch", ToWatch));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", Id));
            DataBase.Instance.connection.Open();

            if (DataBase.Instance.command.ExecuteNonQuery() > 0)
            {
                res = true;
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
            return res;
        }

        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
