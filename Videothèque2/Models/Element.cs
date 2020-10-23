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
     *      An element can be a Film or a Serie or a Discover
     */
    public class Element : INotifyPropertyChanged
    {
        private int id;
        private string title;
        private int place;
        private Boolean toWatch;
        private string toWatchString;
        private string comment;
        private string type;
        private int nbElt;
        private int idCycle;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value;  }
        public int Place { get => place; set => place = value; }
        public bool ToWatch { get => toWatch; set => toWatch = value; } 
        public string ToWatchString { get => toWatchString; set => toWatchString = value; }
        public string Comment { get => comment; set => comment = value; }
        public string Type { get => type; set => type = value; }
        public int NbElt { get => nbElt; set => nbElt = value; }
        public int IdCycle { get => idCycle; set => idCycle = value; }
        private ObservableCollection<Element> ListElt { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        /*
         * Resume :
         *      Get a element list which "ToWatch" = 1 in the base of the Film, Serie and Discover
         * Return an ObservableCollection of the Element type Which "ToWatch" = 1
         */
        public ObservableCollection<Element> GetEltProgram()
        {
            ListElt = new ObservableCollection<Element>();
            GetFilmToWatch();
            GetSerieToWatch();
            GetDiscoverToWatch();
            return ListElt;
        }

        /**
         * Resume
         *      Get items Film ToWatch = 1 and add ListElt
         */
        private void GetFilmToWatch()
        {
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,NbFilm,ToWatch,Comment FROM Films WHERE ToWatch = 1", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Element e = new Element();
                e.Id = DataBase.Instance.reader.GetInt32(0);
                e.Title = DataBase.Instance.reader.GetString(1);
                e.Type = "Film";

                if (!DataBase.Instance.reader.IsDBNull(2))
                    e.nbElt = DataBase.Instance.reader.GetInt32(2);

                int w = DataBase.Instance.reader.GetInt32(3);

                if (w == 1)
                    e.ToWatch = true;

                if (!DataBase.Instance.reader.IsDBNull(4))
                    e.Comment = DataBase.Instance.reader.GetString(4);

                ListElt.Add(e);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
        }

        /**
         * Resume
         *      Get items Serie ToWatch = 1 and add ListElt
         */
        private void GetSerieToWatch()
        {
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,NbSeason,ToWatch,Comment FROM Series WHERE ToWatch = 1", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Element e = new Element();
                e.Id = DataBase.Instance.reader.GetInt32(0);
                e.Title = DataBase.Instance.reader.GetString(1);
                e.Type = "Serie";
                e.nbElt = DataBase.Instance.reader.GetInt32(2);
                int w = DataBase.Instance.reader.GetInt32(3);

                if (w == 1)
                    e.ToWatch = true;

                if (!DataBase.Instance.reader.IsDBNull(4))
                    e.Comment = DataBase.Instance.reader.GetString(4);

                ListElt.Add(e);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
        }

        /**
         * Resume
         *      Get items Discover ToWatch = 1 and add ListElt
         */
        private void GetDiscoverToWatch()
        {
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch,Comment FROM Discover WHERE ToWatch = 1", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Element e = new Element();
                e.Id = DataBase.Instance.reader.GetInt32(0);
                e.Title = DataBase.Instance.reader.GetString(1);
                e.Type = "Découverte";
                e.NbElt = 0;
                int w = DataBase.Instance.reader.GetInt32(2);

                if (w == 1)
                    e.ToWatch = true;

                if (!DataBase.Instance.reader.IsDBNull(3))
                    e.Comment = DataBase.Instance.reader.GetString(3);

                ListElt.Add(e);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
        }

        /*
         * Resume :
         *      Removes an element FILM from a cycle
         * Return True is successfull
         */
        public Boolean UpdateToWatchFilm()
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
        public Boolean UpdateToWatchSerie()
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
        public Boolean UpadateToWatchDiscover()
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
    }
}
