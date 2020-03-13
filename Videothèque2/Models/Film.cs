using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videothèque2.Tools;

namespace Videothèque2.Models
{
    public class Film
    {
        private int id;
        private string title;
        private DateTime lastView;
        private Boolean toWatch;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public DateTime LastView { get => lastView; set => lastView = value; }
        public bool ToWatch { get => toWatch; set => toWatch = value; }

        public Boolean Add()
        {
            bool res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO Films(Title,LastView,ToWatch) OUTPUT INSERTED.ID VALUES(@title,@lastView,'0')",DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("title",Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("lastView",LastView));
            DataBase.Instance.connection.Open();
            Id = (int)DataBase.Instance.command.ExecuteScalar();
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            if (Id > 0)
                res = true;

            return res;
        }

        public List<Film> GetFilms()
        {
            List<Film> l = new List<Film>();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,LastView,ToWatch FROM Films",DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Film f = new Film();
                f.Id = DataBase.Instance.reader.GetInt32(0);
                f.Title = DataBase.Instance.reader.GetString(1);
                f.LastView = DataBase.Instance.reader.GetDateTime(2);
                int w = DataBase.Instance.reader.GetInt32(3);
                if (w == 1)
                    f.ToWatch = true;
                l.Add(f);
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }

        public Boolean UpdateLastView()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Films SET LastView = @LastView, ToWatch = @ToWatch WHERE Id = @Id", DataBase.Instance.connection);
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
