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
    public class Film : Element
    {
        private string genre;
        private string runTime;
        private string releaseDate;
        private int nbFilm;
        private string content;
        private string director;
        private string stars;
        private string poster;
        private DateTime dateAdd;
        private string dateAddFormated;
        private DateTime lastView;
        private string lastViewFormated;
        private int nbView;
        private Rating rating;

        public string Genre { get => genre; set => genre = value; }
        public string RunTime { get => runTime; set => runTime = value; }
        public string ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public int NbFilm { get => nbFilm; set => nbFilm = value; }
        public string Content { get => content; set => content = value; }
        public string Director { get => director; set => director = value; }
        public string Stars { get => stars; set => stars = value; }
        public string Poster { get => poster; set => poster = value; }
        public DateTime DateAdd { get => dateAdd; set => dateAdd = value; }
        public string DateAddFormated { get => dateAddFormated; set => dateAddFormated = value; }
        public DateTime LastView { get => lastView; set => lastView = value; }
        public string LastViewFormated { get => lastViewFormated; set => lastViewFormated = value; }
        public int NbView { get => nbView; set => nbView = value; }
        public Rating Rating { get => rating; set => rating = value; }

        /*
         * Resume :
         *      Add a new film in the bdd
         * Return true if insert is successful
         */
        public Boolean Add()
        {
            bool res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO Films(Title,Genre,Content,Director,Stars,Poster,DateAdd,NbView,ToWatch,Rating)" +
                " OUTPUT INSERTED.ID VALUES(@title,@genre,@content,@director,@stars,@poster,@dateAdd,'0','0',@rating)", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("title", Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("genre", Genre));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("content", Content));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("director", Director));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("stars", Stars));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("poster", Poster));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("dateAdd", DateTime.Now));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("rating", Rating.Aucune));
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
        public ObservableCollection<Film> GetAll()
        {
            ObservableCollection<Film> l = new ObservableCollection<Film>();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,Genre,RunTime,ReleaseDate,NbFilm,Content,Director,Stars,Poster,DateAdd,LastView,NbView,ToWatch,Comment," +
                "Rating FROM Films ORDER BY Title", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
                l.Add(SaveFilm());

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }

        /*
         * Resume :
         *      Get one film with his Id
         *      Just get title and ToWatch
         * Return an Film object 
         */
        public Film GetOneWithId(int idFilm)
        {
            Film f = new Film();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Films WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", idFilm));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                f.Id = DataBase.Instance.reader.GetInt32(0);
                f.Title = DataBase.Instance.reader.GetString(1);
                int w = DataBase.Instance.reader.GetInt32(2);
                if (w == 1)
                    f.ToWatch = true;
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return f;
        }

        /**
         * Resume : 
         *      Get List Films Search by user
         * Parameter :
         *      a string  : search
         * Return an ObservableCollection of the Films type order by Title Where Title == search
         */
        public ObservableCollection<Film> GetSearch(string search)
        {
            ObservableCollection<Film> l = new ObservableCollection<Film>();

            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,Genre,RunTime,ReleaseDate,NbFilm,Content,Director,Stars,Poster,DateAdd,LastView,NbView,ToWatch,Comment," +
                "Rating FROM Films WHERE Title LIKE '%' + @title + '%' ORDER BY Title", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@title", search));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
                l.Add(SaveFilm());

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
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }

        /*
         * Resume :
         *      Edit title and content of the film
         * Return true if update is successful
         */
        public Boolean UpdateOne()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Films SET Title = @Title, Genre = @Genre, RunTime = @RunTime, ReleaseDate = @ReleaseDate, " +
                "NbFilm = @NbFilm, Content = @Content, Director = @Director, Stars = @Stars, Poster = @Poster, Comment = @Comment, Rating = @Rating WHERE Id = @Id",
                DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", Id));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Title", Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Genre", Genre));

            if (RunTime == null)
                DataBase.Instance.command.Parameters.Add(new SqlParameter("@RunTime", DBNull.Value));
            else
                DataBase.Instance.command.Parameters.Add(new SqlParameter("@RunTime", RunTime));

            if (ReleaseDate == null)
                DataBase.Instance.command.Parameters.Add(new SqlParameter("@ReleaseDate", DBNull.Value));
            else
                DataBase.Instance.command.Parameters.Add(new SqlParameter("@ReleaseDate", ReleaseDate));

            DataBase.Instance.command.Parameters.Add(new SqlParameter("@NbFilm", NbFilm));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Content", Content));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Director", Director));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Stars", Stars));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Poster", Poster));

            if (Comment == null)
                DataBase.Instance.command.Parameters.Add(new SqlParameter("@Comment", DBNull.Value));
            else
                DataBase.Instance.command.Parameters.Add(new SqlParameter("@Comment", Comment));

            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Rating", Rating));
            DataBase.Instance.connection.Open();

            if (DataBase.Instance.command.ExecuteNonQuery() > 0)
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
            return res;
        }

        /*
         * Resume :
         *      Delete a film
         * Return true if delete is successful
         */
        public Boolean DeleteOne()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("DELETE FROM Films WHERE id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", Id));
            DataBase.Instance.connection.Open();

            if (DataBase.Instance.command.ExecuteNonQuery() > 0)
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
            return res;
        }

        /**
         * Resume :
         *      Save a film in the object film
         * Return :
         *      a Film for be add a list
         */
        private Film SaveFilm()
        {
            Film f = new Film();
            f.Id = DataBase.Instance.reader.GetInt32(0);
            f.Title = DataBase.Instance.reader.GetString(1);
            f.Genre = DataBase.Instance.reader.GetString(2);

            if (!DataBase.Instance.reader.IsDBNull(3))
                f.RunTime = DataBase.Instance.reader.GetString(3);

            if (!DataBase.Instance.reader.IsDBNull(4))
                f.ReleaseDate = DataBase.Instance.reader.GetString(4);

            if (!DataBase.Instance.reader.IsDBNull(5))
                f.NbFilm = DataBase.Instance.reader.GetInt32(5);

            f.Content = DataBase.Instance.reader.GetString(6);
            f.Director = DataBase.Instance.reader.GetString(7);
            f.Stars = DataBase.Instance.reader.GetString(8);
            f.Poster = DataBase.Instance.reader.GetString(9);
            f.DateAdd = DataBase.Instance.reader.GetDateTime(10);
            f.DateAddFormated = f.DateAdd.ToString("dd/MM/yyyy");
            f.LastViewFormated = "Pas visionné";

            if (!DataBase.Instance.reader.IsDBNull(11))
            {
                f.LastView = DataBase.Instance.reader.GetDateTime(11);
                f.LastViewFormated = f.LastView.ToString("dd/MM/yyyy");
            }

            f.NbView = DataBase.Instance.reader.GetInt32(12);
            int w = DataBase.Instance.reader.GetInt32(13);
            f.ToWatchString = "Non programmé";

            if (w == 1)
            {
                f.ToWatch = true;
                f.ToWatchString = "Programmé";
            }

            if (!DataBase.Instance.reader.IsDBNull(14))
                f.Comment = DataBase.Instance.reader.GetString(14);

            f.Rating = (Rating)DataBase.Instance.reader.GetInt32(15);

            return f;
        }
    }
}
