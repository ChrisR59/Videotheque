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
    public class Serie
    {
        private int id;
        private string title;
        private string nbSeason;
        private string content;
        private DateTime dateAdd;
        private string dateAddFormated;
        private DateTime lastView;
        private string lastViewFormated;
        private int nbView;
        private Boolean toWatch;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string NbSeason { get => nbSeason; set => nbSeason = value; }
        public string Content { get => content; set => content = value; }
        public DateTime DateAdd { get => dateAdd; set => dateAdd = value; }
        public string DateAddFormated { get => dateAddFormated; set => dateAddFormated = value; }
        public DateTime LastView { get => lastView; set => lastView = value; }
        public string LastViewFormated { get => lastViewFormated; set => lastViewFormated = value; }
        public int NbView { get => nbView; set => nbView = value; }
        public bool ToWatch { get => toWatch; set => toWatch = value; }

        public Boolean Add()
        {
            bool res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO Series (Title, NbSeason,Content,DateAdd,NbView,ToWatch) OUTPUT INSERTED.ID VALUES(@title,@nbSeason,@content,@dateAdd,'0','0')", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@title",Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@nbSeason", NbSeason));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@content", Content));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@dateAdd", DateAdd));
            DataBase.Instance.connection.Open();
            Id = (int)DataBase.Instance.command.ExecuteScalar();
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            if (Id > 0)
                res = true;

            return res;
        }

        public ObservableCollection<Serie> GetSerie()
        {
            ObservableCollection<Serie> l = new ObservableCollection<Serie>();

            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,NbSeason,Content,DateAdd,LastView,NbView,ToWatch FROM Series", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while(DataBase.Instance.reader.Read())
            {
                Serie s = new Serie();
                s.Id = DataBase.Instance.reader.GetInt32(0);
                s.Title = DataBase.Instance.reader.GetString(1);
                s.NbSeason = DataBase.Instance.reader.GetString(2);
                s.Content = DataBase.Instance.reader.GetString(3);
                s.DateAdd = DataBase.Instance.reader.GetDateTime(4);
                s.DateAddFormated = s.DateAdd.ToString("dd/MM/yyyy");
                s.LastViewFormated = "Pas visionné";
                if (!DataBase.Instance.reader.IsDBNull(5))
                {
                    s.LastView = DataBase.Instance.reader.GetDateTime(5);
                    s.LastViewFormated = s.LastView.ToString("dd/MM/yyyy");
                }
                s.NbView = DataBase.Instance.reader.GetInt32(6);
                int w = DataBase.Instance.reader.GetInt32(7);
                if (w == 1)
                    s.ToWatch = true;
                l.Add(s);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }
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

        public Boolean UpdateSerie()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Series SET Title = @Title, NbSeason = @NbSeason, Content = @Content WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Title", Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@NbSeason", NbSeason));
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
