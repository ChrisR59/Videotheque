using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videothèque2.Tools;

namespace Videothèque2.Models
{
    /*
     * Resume
     *      a serie
     */
    public class Serie
    {
        private int id;
        private string title;
        private string genre;
        private string nbSeason;
        private string content;
        private string director;
        private string stars;
        private string poster;
        private DateTime dateAdd;
        private string dateAddFormated;
        private DateTime lastView;
        private string lastViewFormated;
        private int nbView;
        private Boolean toWatch;
        private string toWatchString;
        private string comment;
        private int rating;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Genre { get => genre; set => genre = value; }
        public string NbSeason { get => nbSeason; set => nbSeason = value; }
        public string Content { get => content; set => content = value; }
        public string Director { get => director; set => director = value; }
        public string Stars { get => stars; set => stars = value; }
        public string Poster { get => poster; set => poster = value; }
        public DateTime DateAdd { get => dateAdd; set => dateAdd = value; }
        public string DateAddFormated { get => dateAddFormated; set => dateAddFormated = value; }
        public DateTime LastView { get => lastView; set => lastView = value; }
        public string LastViewFormated { get => lastViewFormated; set => lastViewFormated = value; }
        public int NbView { get => nbView; set => nbView = value; }
        public bool ToWatch { get => toWatch; set => toWatch = value; }
        public string ToWatchString { get => toWatchString; set => toWatchString = value; }
        public string Comment { get => comment; set => comment = value; }
        public int Rating { get => rating; set => rating = value; }

        /*
         * Resume :
         *      Add a new serie in the bdd
         * Return true if insert is successful
         */
        public Boolean Add()
        {
            bool res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO Series (Title,Genre,NbSeason,Content,Director,Stars,Poster,DateAdd,NbView,ToWatch)" +
                " OUTPUT INSERTED.ID VALUES(@title,@genre,@nbSeason,@content,@director,@stars,@poster,@dateAdd,'0','0')", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@title",Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@genre",Genre));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@nbSeason", NbSeason));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@content", Content));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@director", Director));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@stars", Stars));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@poster", Poster));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@dateAdd", DateAdd));
            DataBase.Instance.connection.Open();
            Id = (int)DataBase.Instance.command.ExecuteScalar();
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            if (Id > 0)
                res = true;

            return res;
        }

        /*
         * Resume :
         *      Get a serie list
         * Return an ObservableCollection of the Serie type order by Title
         */
        public ObservableCollection<Serie> GetSerie()
        {
            ObservableCollection<Serie> l = new ObservableCollection<Serie>();

            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,Genre,NbSeason,Content,Director,Stars,Poster,DateAdd,LastView,NbView,ToWatch,Comment,Rating FROM Series ORDER BY Title", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while(DataBase.Instance.reader.Read())
            {
                Serie s = new Serie();
                s.Id = DataBase.Instance.reader.GetInt32(0);
                s.Title = DataBase.Instance.reader.GetString(1);
                s.Genre = DataBase.Instance.reader.GetString(2);
                s.NbSeason = DataBase.Instance.reader.GetString(3);
                s.Content = DataBase.Instance.reader.GetString(4);
                s.Director = DataBase.Instance.reader.GetString(5);
                s.Stars = DataBase.Instance.reader.GetString(6);
                s.Poster = DataBase.Instance.reader.GetString(7);
                s.DateAdd = DataBase.Instance.reader.GetDateTime(8);
                s.DateAddFormated = s.DateAdd.ToString("dd/MM/yyyy");
                s.LastViewFormated = "Pas visionné";
                if (!DataBase.Instance.reader.IsDBNull(9))
                {
                    s.LastView = DataBase.Instance.reader.GetDateTime(9);
                    s.LastViewFormated = s.LastView.ToString("dd/MM/yyyy");
                }
                s.NbView = DataBase.Instance.reader.GetInt32(10);
                int w = DataBase.Instance.reader.GetInt32(11);
                s.ToWatchString = "Non programmé";
                if (w == 1)
                {
                    s.ToWatch = true;
                    s.ToWatchString = "Programmé";
                }
                if (!DataBase.Instance.reader.IsDBNull(12))
                    s.Comment = DataBase.Instance.reader.GetString(12);
                if (!DataBase.Instance.reader.IsDBNull(13))
                    s.Rating = DataBase.Instance.reader.GetInt32(13);
                
                l.Add(s);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }

        /*
         * Resume :
         *      Edit one serie after watching
         * Return true if update is successful
         */
        public Boolean UpdateLastView()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Series SET LastView = @LastView, NbView = @NbView, ToWatch = @ToWatch WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@LastView", LastView));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@NbView", NbView));
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
         *      Edit one serie for program or deprogram
         * Return true if update is successful
         */
        public Boolean UpSerieProgramm()
        {
            Boolean res = false;
            string req = "UPDATE Series SET ToWatch = @ToWatch WHERE id = @Id";
            DataBase.Instance.command = new SqlCommand(req, DataBase.Instance.connection);
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
         *      Edit title, nbSeason and content of the serie
         * Return true if update is successful
         */
        public Boolean UpdateSerie()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Series SET Title = @Title, NbSeason = @NbSeason, Content = @Content, Poster = @Poster WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Title", Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@NbSeason", NbSeason));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Content", Content));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Poster", Poster));
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
         *      Delete a serie
         * Return true if selete is successful
         */
        public Boolean DeleteSerie()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("DELETE FROM Series WHERE id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id",Id));
            DataBase.Instance.connection.Open();

            if(DataBase.Instance.command.ExecuteNonQuery() > 0)
            {
                res = true;
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }
    }
}
