using System;
using System.Collections.Generic;
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
        private DateTime lastView;
        private Boolean toWatch;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string NbSeason { get => nbSeason; set => nbSeason = value; }
        public DateTime LastView { get => lastView; set => lastView = value; }
        public bool ToWatch { get => toWatch; set => toWatch = value; }

        public Boolean Add()
        {
            bool res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO Series (Title, NbSeason,LastView,ToWatch) OUTPUT INSERTED.ID VALUES(@title,@nbSeason,@lastView,'0')",DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@title",Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@nbSeason",NbSeason));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@lastView",LastView));
            DataBase.Instance.connection.Open();
            Id = (int)DataBase.Instance.command.ExecuteScalar();
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            if (Id > 0)
                res = true;

            return res;
        }

        public List<Serie> GetSerie()
        {
            List<Serie> l = new List<Serie>();

            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,NbSeason,LastView,ToWatch FROM Series",DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while(DataBase.Instance.reader.Read())
            {
                Serie s = new Serie();
                s.Id = DataBase.Instance.reader.GetInt32(0);
                s.Title = DataBase.Instance.reader.GetString(1);
                s.NbSeason = DataBase.Instance.reader.GetString(2);
                s.LastView = DataBase.Instance.reader.GetDateTime(3);
                int w = DataBase.Instance.reader.GetInt32(4);
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
            DataBase.Instance.command = new SqlCommand("UPDATE Series SET LastView = @LastView, ToWatch = @ToWatch WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@LastView", LastView));
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
