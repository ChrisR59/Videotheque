using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videothèque2.Tools;

namespace Videothèque2.Models
{
    /*
     * Resume :
     *      Content of the cycle
     */
    public class CycleContent : Element
    {
        private string status;
        private int rank;
        private int idElt;

        public string Status { get => status; set => status = value; }//?
        public int Rank { get => rank; set => rank = value; }
        public int IdElt { get => idElt; set => idElt = value; }

        /*
         * Resume :
         *      Add a Cycle in the Bdd
         *      
         * Return True if insert is successful
         */
        public Boolean AddElement()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO CycleContent(Title,Status,Rank,Type,NbElt,IdElt,IdCycle,ToWatch,Comment) " +
                "OUTPUT INSERTED.ID VALUES(@title,@status,@rank,@type,@NbElt,@idElt,@idCycle,'0',@Comment)", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@title", Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@status", Status));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@rank", Rank));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@type", Type));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@NbElt", NbElt));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@idElt", IdElt));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@idCycle", IdCycle));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Comment", Comment));
            DataBase.Instance.connection.Open();

            if ((int)DataBase.Instance.command.ExecuteNonQuery() > 0)
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }

        /*
         * Resume 
         *      Get an items list form the current cycle
         * Parameter
         *      An Integer corresponding to the id of the current cycle
         * Return an ObservableCollection of the CycleContent type
         */
        public ObservableCollection<CycleContent> GetCurrentCycle(int idCycleS)
        {
            ObservableCollection<CycleContent> listC = new ObservableCollection<CycleContent>();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,Status,Rank,Type,NbElt,IdElt,ToWatch,Comment FROM CycleContent WHERE IdCycle = @idCycle",
                DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@idCycle", idCycleS));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                CycleContent c = new CycleContent();
                c.Id = DataBase.Instance.reader.GetInt32(0);
                c.Title = DataBase.Instance.reader.GetString(1);
                c.Status = DataBase.Instance.reader.GetString(2);
                c.Rank = DataBase.Instance.reader.GetInt32(3);
                c.Type = DataBase.Instance.reader.GetString(4);
                c.NbElt = DataBase.Instance.reader.GetInt32(5);
                c.IdElt = DataBase.Instance.reader.GetInt32(6);
                c.IdCycle = idCycleS;

                if (!DataBase.Instance.reader.IsDBNull(8))
                    c.Comment = DataBase.Instance.reader.GetString(8);

                int w = DataBase.Instance.reader.GetInt32(7);
                c.ToWatch = false;

                if (w == 1)
                    c.ToWatch = true;

                listC.Add(c);
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return listC;
        }

        /*
         * Resume : 
         *      get the rank of an element of a cycle and increment for a new elt
         */
        public void GetRankElt()
        {
            DataBase.Instance.command = new SqlCommand("SELECT Rank FROM CycleContent WHERE IdCycle = @idCycle", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@idCycle", IdCycle));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                if (DataBase.Instance.reader.GetInt32(0) > Rank)
                    Rank = DataBase.Instance.reader.GetInt32(0);
            }
            Rank++;
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

        }

        /*
         * Resume
         *      Edit "ToWatch" of an element of a cycle
         * Return True if update is successful
         */
        public Boolean UpdateToWatch()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE CycleContent SET ToWatch = @ToWatch WHERE Id = @Id", DataBase.Instance.connection);
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
         * Resume
         *      Edit "Rank" of an element of a cycle
         * Return True if update is successful
         */
        public Boolean UpdateRank()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE CycleContent SET Rank = @Rank WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Rank", Rank));
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
         *      Remove a Cycle 
         *  Return True is successful
         */
        public Boolean DelElement()
        {
            Boolean res = false;
            DataBase.Instance.command = new SqlCommand("DELETE FROM CycleContent WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", Id));
            DataBase.Instance.connection.Open();

            if (DataBase.Instance.command.ExecuteNonQuery() > 0)
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }
    }
}
