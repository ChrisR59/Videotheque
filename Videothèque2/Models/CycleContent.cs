using System;
using System.Collections.Generic;
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
        private int idCycle;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Status { get => status; set => status = value; }
        public int Rank { get => rank; set => rank = value; }
        public int IdCycle { get => idCycle; set => idCycle = value; }

        public Boolean AddElement()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO CycleContent(Title,Status,Rank,IdCycle) OUTPUT INSERTED.ID VALUES(@title,@status,@rank,@idCycle)", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@title",Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@status", Status));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@rank", Rank));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@idCycle", IdCycle));
            DataBase.Instance.connection.Open();

            if ((int)DataBase.Instance.command.ExecuteNonQuery() > 0)
                res = true;
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
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
