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
    public class CycleContent : INotifyPropertyChanged
    {
        private int id;
        private string title;
        private string status;
        private int rank;
        private string type;
        private int nbElt;
        private int idElt;
        private int idCycle;
        private Boolean toWatch;
        private string comment;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Status { get => status; set => status = value; }//?
        public int Rank { get => rank; set => rank = value; }
        public string Type { get => type; set => type = value; }
        public int NbElt { get => nbElt; set => nbElt = value; }
        public int IdElt { get => idElt; set => idElt = value; }
        public int IdCycle { get => idCycle; set => idCycle = value; }
        public bool ToWatch
        {
            get => toWatch;
            set
            {
                toWatch = value;
                NotifyPropertyChange("ToWatch");
            }
        }
        public string Comment { get => comment; set => comment = value; }

        public event PropertyChangedEventHandler PropertyChanged;

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
                CycleContent c = new CycleContent()
                {
                    Id = DataBase.Instance.reader.GetInt32(0),
                    Title = DataBase.Instance.reader.GetString(1),
                    Status = DataBase.Instance.reader.GetString(2),
                    Rank = DataBase.Instance.reader.GetInt32(3),
                    Type = DataBase.Instance.reader.GetString(4),
                    NbElt = DataBase.Instance.reader.GetInt32(5),
                    IdElt = DataBase.Instance.reader.GetInt32(6),
                    IdCycle = idCycleS,
                };

                if (!DataBase.Instance.reader.IsDBNull(8))
                    c.Comment = DataBase.Instance.reader.GetString(8);

                int w = DataBase.Instance.reader.GetInt32(7);
                c.ToWatch = false;
                if (w == 1)
                {
                    c.ToWatch = true;
                }

                listC.Add(c);
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return listC;
        }

        /*
         * Resume :
         *      Get one Film
         * Return a object of the film type
         */
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

        /*
         * Resume :
         *      Get one serie
         * Return a object of the serie type
         */
        public Serie GetOneSerie()
        {
            Serie s = new Serie();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,LastView,ToWatch FROM Series WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", IdElt));
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

        /*
         * Resume :
         *      Get one serie
         * Return a object of the serie type
         */
        public Discover GetOneDiscover()
        {
            Discover d = new Discover();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Discover WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", IdElt));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                d.Id = DataBase.Instance.reader.GetInt32(0);
                d.Title = DataBase.Instance.reader.GetString(1);
                int w = DataBase.Instance.reader.GetInt32(2);
                if (w == 1)
                    d.ToWatch = true;
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return d;
        }

        /*
         * Resume : 
         *      get the rank of an element of a cycle
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
            {
                res = true;
            }

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
            {
                res = true;
            }

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
            {
                res = true;
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }

        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
