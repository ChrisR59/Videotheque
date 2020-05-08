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
    public class CycleStatus
    {
        private int id;
        private Status statusC;

        public int Id { get => id; set => id = value; }
        public Status StatusC { get => statusC; set => statusC = value; }

        public Boolean AddCycle()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO CycleStatus(Status) OUTPUT INSERTED.ID VALUES(@StatusC)", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@StatusC", StatusC));
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

        public ObservableCollection<CycleStatus> GetCycles()
        {
            ObservableCollection<CycleStatus> l = new ObservableCollection<CycleStatus>();

            DataBase.Instance.command = new SqlCommand("SELECT Id,Status FROM CycleStatus", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                CycleStatus c = new CycleStatus();
                c.Id = DataBase.Instance.reader.GetInt32(0);
                c.StatusC = (Status)DataBase.Instance.reader.GetInt32(1);
                l.Add(c);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }

        public void GetIdCycle()
        {
            DataBase.Instance.command = new SqlCommand("SELECT Id FROM CycleStatus WHERE Status = '0'", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();
            if (DataBase.Instance.reader.Read())
            {
                Id = DataBase.Instance.reader.GetInt32(0);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
        }

        public void CheckCycleExist()
        {
            DataBase.Instance.command = new SqlCommand("SELECT Id FROM CycleStatus WHERE Status = '0'",DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            if ((int)DataBase.Instance.command.ExecuteScalar() > 0)
                StatusC = Status.Prevu;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
        }

        public ObservableCollection<int> GetListCycle()
        {
            ObservableCollection<int> l = new ObservableCollection<int>();

            DataBase.Instance.command = new SqlCommand("SELECT Id FROM CycleStatus WHERE Status != '2'",DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while(DataBase.Instance.reader.Read())
                l.Add(DataBase.Instance.reader.GetInt32(0));

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }

        public Boolean DeleteOne()
        {
            bool res = false;
            DataBase.Instance.command = new SqlCommand("DELETE FROM CycleStatus WHERE id = @id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@id", Id));
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

    public enum Status
    {
        EnCours,
        Prevu,
        Termine
    }
}
