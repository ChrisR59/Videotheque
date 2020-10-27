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
    public class Serie : Element
    {
        private string genre;
        private string runTime;
        private string releaseDate;
        private int nbSeason;
        private int nbEpisode;
        private string content;
        private string director;
        private string stars;
        private string poster;
        private DateTime dateAdd;
        private string dateAddFormated;
        private DateTime lastView;
        private string lastViewFormated;
        private int nbView;
        private SerieStatus status;
        private Rating rating;

        public string Genre { get => genre; set => genre = value; }
        public string RunTime { get => runTime; set => runTime = value; }
        public string ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public int NbSeason { get => nbSeason; set => nbSeason = value; }
        public int NbEpisode { get => nbEpisode; set => nbEpisode = value; }
        public string Content { get => content; set => content = value; }
        public string Director { get => director; set => director = value; }
        public string Stars { get => stars; set => stars = value; }
        public string Poster { get => poster; set => poster = value; }
        public DateTime DateAdd { get => dateAdd; set => dateAdd = value; }
        public string DateAddFormated { get => dateAddFormated; set => dateAddFormated = value; }
        public DateTime LastView { get => lastView; set => lastView = value; }
        public string LastViewFormated { get => lastViewFormated; set => lastViewFormated = value; }
        public int NbView { get => nbView; set => nbView = value; }
        public SerieStatus Status { get => status; set => status = value; }
        public Rating Rating { get => rating; set => rating = value; }

        /*
         * Resume :
         *      Add a new serie in the bdd
         * Return true if insert is successful
         */
        public Boolean Add()
        {
            bool res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO Series (Title,Genre,Content,Director,Stars,Poster,DateAdd,NbView,ToWatch,Status,Rating)" +
                " OUTPUT INSERTED.ID VALUES(@title,@genre,@content,@director,@stars,@poster,@dateAdd,'0','0',@status,@rating)", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@title", Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@genre", Genre));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@content", Content));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@director", Director));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@stars", Stars));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@poster", Poster));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@dateAdd", DateTime.Now));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@status", SerieStatus.Inconnu));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@rating", Rating.Aucune));
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
        public ObservableCollection<Serie> GetAll()
        {
            ObservableCollection<Serie> l = new ObservableCollection<Serie>();

            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,Genre,RunTime,ReleaseDate,NbSeason,NbEpisode,Content,Director,Stars,Poster,DateAdd,LastView,NbView,ToWatch," +
                "Comment,Status,Rating FROM Series ORDER BY Title", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
                l.Add(SaveSerie());

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }

        /*
         * Resume :
         *      Get one serie with his Id
         * Return an Serie object 
         */
        public Serie GetOneWithId(int idSerie)
        {
            Serie s = new Serie();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Series WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", idSerie));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                s.Id = DataBase.Instance.reader.GetInt32(0);
                s.Title = DataBase.Instance.reader.GetString(1);

                int watch = DataBase.Instance.reader.GetInt32(2);
                if (watch == 1)
                    s.ToWatch = true;
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return s;
        }

        /**
         * Resume : 
         *      Get List Series Search by user
         * Parameter :
         *      a string  : search
         * Return an ObservableCollection of the Serie type order by Title Where Title == search
         */
        public ObservableCollection<Serie> GetSearch(string search)
        {
            ObservableCollection<Serie> l = new ObservableCollection<Serie>();

            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,Genre,RunTime,ReleaseDate,NbSeason,NbEpisode,Content,Director,Stars,Poster,DateAdd,LastView,NbView,ToWatch," +
                "Comment,Status,Rating FROM Series WHERE Title LIKE '%' + @title + '%' ORDER BY Title", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@title", search));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
                l.Add(SaveSerie());

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
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }

        /*
         * Resume :
         *      Edit title, nbSeason and content of the serie
         * Return true if update is successful
         */
        public Boolean UpdateOne()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Series SET Title = @Title, Genre = @Genre, RunTime = @RunTime, ReleaseDate = @ReleaseDate," +
                " NbSeason = @NbSeason, NbEpisode = @NbEpisode, Content = @Content, Director = @Director, Stars = @Stars, Poster = @Poster, Comment = @Comment, " +
                "Status = @Status, Rating = @Rating WHERE Id = @Id",
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

            DataBase.Instance.command.Parameters.Add(new SqlParameter("@NbSeason", NbSeason));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@NbEpisode", NbEpisode));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Content", Content));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Director", Director));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Stars", Stars));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Poster", Poster));

            if (Comment == null)
                DataBase.Instance.command.Parameters.Add(new SqlParameter("@Comment", DBNull.Value));
            else
                DataBase.Instance.command.Parameters.Add(new SqlParameter("@Comment", Comment));

            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Status", Status));
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
         *      Delete a serie
         * Return true if selete is successful
         */
        public Boolean DeleteOne()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("DELETE FROM Series WHERE id = @Id", DataBase.Instance.connection);
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
         *      Save a Serie in the object serie
         * Return :
         *      a Serie for be add a list
         */
        private Serie SaveSerie()
        {
            Serie s = new Serie();
            s.Id = DataBase.Instance.reader.GetInt32(0);
            s.Title = DataBase.Instance.reader.GetString(1);
            s.Genre = DataBase.Instance.reader.GetString(2);

            if (!DataBase.Instance.reader.IsDBNull(3))
                s.RunTime = DataBase.Instance.reader.GetString(3);

            if (!DataBase.Instance.reader.IsDBNull(4))
                s.ReleaseDate = DataBase.Instance.reader.GetString(4);

            if (!DataBase.Instance.reader.IsDBNull(5))
                s.NbSeason = DataBase.Instance.reader.GetInt32(5);

            if (!DataBase.Instance.reader.IsDBNull(6))
                s.NbEpisode = DataBase.Instance.reader.GetInt32(6);

            s.Content = DataBase.Instance.reader.GetString(7);
            s.Director = DataBase.Instance.reader.GetString(8);
            s.Stars = DataBase.Instance.reader.GetString(9);
            s.Poster = DataBase.Instance.reader.GetString(10);
            s.DateAdd = DataBase.Instance.reader.GetDateTime(11);
            s.DateAddFormated = s.DateAdd.ToString("dd/MM/yyyy");
            s.LastViewFormated = "Pas visionné";

            if (!DataBase.Instance.reader.IsDBNull(12))
            {
                s.LastView = DataBase.Instance.reader.GetDateTime(12);
                s.LastViewFormated = s.LastView.ToString("dd/MM/yyyy");
            }

            s.NbView = DataBase.Instance.reader.GetInt32(13);
            int watch = DataBase.Instance.reader.GetInt32(14);
            s.ToWatchString = "Non programmé";

            if (watch == 1)
            {
                s.ToWatch = true;
                s.ToWatchString = "Programmé";
            }

            if (!DataBase.Instance.reader.IsDBNull(15))
                s.Comment = DataBase.Instance.reader.GetString(15);

            if (!DataBase.Instance.reader.IsDBNull(16))
                s.Status = (SerieStatus)DataBase.Instance.reader.GetInt32(16);

            s.Rating = (Rating)DataBase.Instance.reader.GetInt32(17);

            return s;
        }
    }
}
