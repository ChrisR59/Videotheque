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
     *      A film
     */
    public class Film
    {
        private int id;
        private string title;
        private string content;
        private string poster;
        private DateTime dateAdd;
        private string dateAddFormated;
        private DateTime lastView;
        private string lastViewFormated;
        private int nbView;
        private Boolean toWatch;
        private string toWatchString;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Content { get => content; set => content = value; }
        public string Poster { get => poster; set => poster = value; }
        public DateTime DateAdd { get => dateAdd; set => dateAdd = value; }
        public string DateAddFormated { get => dateAddFormated; set => dateAddFormated = value; }
        public DateTime LastView { get => lastView; set => lastView = value; }
        public string LastViewFormated { get => lastViewFormated; set => lastViewFormated = value; }
        public int NbView { get => nbView; set => nbView = value; }
        public bool ToWatch { get => toWatch; set => toWatch = value; }
        public string ToWatchString { get => toWatchString; set => toWatchString = value; }

        /*
         * Resume :
         *      Add a new film in the bdd
         * Return true if insert is successful
         */
        public Boolean Add()
        {
            bool res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO Films(Title,Content,Poster,DateAdd,NbView,ToWatch) OUTPUT INSERTED.ID VALUES(@title,@content,@poster,@dateAdd,'0','0')",DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("title",Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("content",Content));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("poster",Poster));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("dateAdd",DateAdd));
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
         *      Get a film list
         * Return an ObservableCollection of the Film type order by Title
         */
        public ObservableCollection<Film> GetFilms()
        {
            ObservableCollection<Film> l = new ObservableCollection<Film>();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,Content,Poster,DateAdd,LastView,NbView,ToWatch FROM Films ORDER BY Title",DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Film f = new Film();
                f.Id = DataBase.Instance.reader.GetInt32(0);
                f.Title = DataBase.Instance.reader.GetString(1);
                f.Content = DataBase.Instance.reader.GetString(2);
                f.Poster = DataBase.Instance.reader.GetString(3);
                f.DateAdd = DataBase.Instance.reader.GetDateTime(4);
                f.DateAddFormated = f.DateAdd.ToString("dd/MM/yyyy");
                f.LastViewFormated = "Pas visionné";
                if (!DataBase.Instance.reader.IsDBNull(5))
                {
                    f.LastView = DataBase.Instance.reader.GetDateTime(5);
                    f.LastViewFormated = f.LastView.ToString("dd/MM/yyyy");
                }
                f.NbView = DataBase.Instance.reader.GetInt32(6);
                int w = DataBase.Instance.reader.GetInt32(7);
                f.ToWatchString = "Non programmé";
                if (w == 1)
                {
                    f.ToWatch = true;
                    f.ToWatchString = "Programmé";
                }

                l.Add(f);
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }

        /*
         * Resume :
         *      Edit one film after watching the movie
         * Return true if update is successful
         */
        public Boolean UpdateLastView()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Films SET LastView = @LastView, NbView = @NbView, ToWatch = @ToWatch WHERE Id = @Id", DataBase.Instance.connection);
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
         *      Edit one film for program or deprogram
         * Return true if update is successful
         */
        public Boolean UpFilmProgramm()
        {
            Boolean res = false;
            string req = "UPDATE Films SET ToWatch = @ToWatch WHERE id = @Id";
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
         *      Edit title and content of the film
         * Return true if update is successful
         */
        public Boolean UpdateFilm()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Films SET Title = @Title, Content = @Content WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Title", Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Content", Content));
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
         *      Delete a film
         * Return true if delete is successful
         */
        public Boolean DeleteFilm()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("DELETE FROM Films WHERE id = @Id", DataBase.Instance.connection);
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
