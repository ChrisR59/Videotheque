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
    /**
     * Resume
     *      Status of a cycle
     */
    public class CycleStatus
    {
        private int id;
        private int number;
        private StatusOfCycle statusC;

        public int Id { get => id; set => id = value; }
        public int Number { get => number; set => number = value; }
        public StatusOfCycle StatusC { get => statusC; set => statusC = value; }

        /*
         * Resume
         *      Add one cycle in the bdd
         * Return true if insert is successful
         */
        public Boolean Add()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO CycleStatus(Number, Status) OUTPUT INSERTED.ID VALUES(@Number, @StatusC)", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Number", Number));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@StatusC", StatusC));
            DataBase.Instance.connection.Open();

            if ((int)DataBase.Instance.command.ExecuteScalar() > 0)
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }

        /*
         * Resume :
         *      Get a cycle list
         * Return an ObservableCollection of the CycleStatus type
         */
        public ObservableCollection<CycleStatus> GetAll()
        {
            ObservableCollection<CycleStatus> l = new ObservableCollection<CycleStatus>();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Number,Status FROM CycleStatus", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                CycleStatus c = new CycleStatus();
                c.Id = DataBase.Instance.reader.GetInt32(0);
                c.Number = DataBase.Instance.reader.GetInt32(1);
                c.StatusC = (StatusOfCycle)DataBase.Instance.reader.GetInt32(2);
                l.Add(c);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }

        /*
         * Resume :
         *      Get a Number of a Cycle 'Encours'
         * Return
         *      a attribut Number
         */
        public int GetIdCycle()
        {
            DataBase.Instance.command = new SqlCommand("SELECT Number FROM CycleStatus WHERE Status = '0'", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            if (DataBase.Instance.reader.Read())
                Number = DataBase.Instance.reader.GetInt32(0);

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return Number;
        }

        /*
         * Resume :
         *      Get one cycle 'Prevue'
         * Return
         *      a attribut Number
         */
        public int GetNewCycle()
        {
            DataBase.Instance.command = new SqlCommand("SELECT Number FROM CycleStatus WHERE Status = '1'", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            int i = 0;// partie à ameliorer
            while (DataBase.Instance.reader.Read())
            {
                if(i == 0)
                {
                    Number = DataBase.Instance.reader.GetInt32(0);
                    StatusC = StatusOfCycle.EnCours;
                    i++;
                }
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return Number;
        }

        /*
         * Resume : 
         *      Check and get if a current cycle
         */
        public void CheckCycleExist()
        {
            StatusC = StatusOfCycle.Prevu;
            DataBase.Instance.command = new SqlCommand("SELECT Id FROM CycleStatus WHERE Status = '0'",DataBase.Instance.connection);
            DataBase.Instance.connection.Open();

            if (DataBase.Instance.command.ExecuteScalar() == null)
                StatusC = StatusOfCycle.EnCours;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
        }

        /*
         * Resume :
         *      Get total number of cycles
         */
        public void GetOneNumberCycle()
        {
            DataBase.Instance.command = new SqlCommand("SELECT Number FROM CycleStatus ORDER BY Number DESC",DataBase.Instance.connection);
            DataBase.Instance.connection.Open();

            Number = (int) DataBase.Instance.command.ExecuteScalar();
            Number++;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
        }

        /*
         * Resume :
         *      Get a cycle list which are not finished
         * Return an ObservableCollection of the CycleStatus type which are not finished
         */
        public ObservableCollection<int> GetCycleListNotFinish()
        {
            ObservableCollection<int> l = new ObservableCollection<int>();

            DataBase.Instance.command = new SqlCommand("SELECT Number FROM CycleStatus WHERE Status != '2'",DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while(DataBase.Instance.reader.Read())
                l.Add(DataBase.Instance.reader.GetInt32(0));

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }

        /*
         * Resume :
         *      Get all cycle list 
         * Return an ObservableCollection of the CycleStatus type which are not finished
         */
        public ObservableCollection<int> GetAllCycle()
        {
            ObservableCollection<int> l = new ObservableCollection<int>();

            DataBase.Instance.command = new SqlCommand("SELECT Number FROM CycleStatus", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
                l.Add(DataBase.Instance.reader.GetInt32(0));

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }

        /*
         * Resume :
         *      Edit "Status" of a cycle
         * Return true if update is successful
         */
        public Boolean UpdateStatusCycle()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE CycleStatus SET Status = @Status WHERE Number = @Number", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Status", StatusC));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Number", Number));
            DataBase.Instance.connection.Open();

            if (DataBase.Instance.command.ExecuteNonQuery() > 0)
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }

        /*
         * Resume :
         *      Delete one cycle
         * Return true if delete is successful
         */
        public Boolean DeleteOne()
        {
            bool res = false;
            DataBase.Instance.command = new SqlCommand("DELETE FROM CycleStatus WHERE id = @id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@id", Id));
            DataBase.Instance.connection.Open();

            if (DataBase.Instance.command.ExecuteNonQuery() > 0)
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
            return res;
        }
    }
}
