using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videothèque2.Tools;

namespace Videothèque2.Models
{
    public class CycleStatus
    {
        private int id;
        private Status statusC;

        public int Id { get => id; set => id = value; }
        public Status StatusC { get => statusC; set => statusC = value; }

        public Boolean AddCycle()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO CycleStatus(Status) OUTPUT INSERTED.ID VALUES('@StatusC')", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("StatusC", Status.EnCours));
            DataBase.Instance.connection.Open();
            if ((int)DataBase.Instance.command.ExecuteScalar() > 0)
                res = true;
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }

        public void EditCycle()
        {

        }
    }

    public enum Status
    {
        EnCours,
        Prevu,
        Termine
    }
}
