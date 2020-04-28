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
    public class CycleContent
    {
        private int id;
        private string title;
        private string status;
        private int rank;
        private string type;
        private int idElt;
        private int idCycle;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Status { get => status; set => status = value; }
        public int Rank { get => rank; set => rank = value; }
        public string Type { get => type; set => type = value; }
        public int IdElt { get => idElt; set => idElt = value; }
        public int IdCycle { get => idCycle; set => idCycle = value; }

        public Boolean AddElement()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO CycleContent(Title,Status,Rank,Type,IdElt,IdCycle) OUTPUT INSERTED.ID VALUES(@title,@status,@rank,@type,@idElt,@idCycle)", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@title",Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@status", Status));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@rank", Rank));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@type", Type));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@idElt", IdElt));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@idCycle", IdCycle));
            DataBase.Instance.connection.Open();

            if ((int)DataBase.Instance.command.ExecuteNonQuery() > 0)
                res = true;
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }

        public ObservableCollection<CycleContent> GetCycleActually(int idCycleS)
        {
            ObservableCollection<CycleContent> listC = new ObservableCollection<CycleContent>();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,Status,Rank,Type,IdElt FROM CycleContent WHERE IdCycle = @idCycle", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@idCycle", idCycleS));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while(DataBase.Instance.reader.Read())
            {
                CycleContent c = new CycleContent()
                {
                    Id = DataBase.Instance.reader.GetInt32(0),
                    Title = DataBase.Instance.reader.GetString(1),
                    Status = DataBase.Instance.reader.GetString(2),
                    Rank = DataBase.Instance.reader.GetInt32(3),
                    Type = DataBase.Instance.reader.GetString(4),
                    IdElt = DataBase.Instance.reader.GetInt32(5),
                    IdCycle = idCycleS
                };
                listC.Add(c);
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return listC;
        }
        public Film GetOneFilm()
        {
            Film f = new Film();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,LastView,ToWatch FROM Films WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", IdElt));
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

        public void GetRank()
        {
            DataBase.Instance.command = new SqlCommand("SELECT Rank FROM CycleContent WHERE IdCycle = @idCycle",DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@idCycle", IdCycle));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while(DataBase.Instance.reader.Read()){
                if (DataBase.Instance.reader.GetInt32(0) > Rank)
                    Rank = DataBase.Instance.reader.GetInt32(0);
            }
            Rank++;
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

        }
    }
}
