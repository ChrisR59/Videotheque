using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videothèque2.Tools;

namespace Videothèque2.Models
{
    public class Element : INotifyPropertyChanged
    {
        private int id;
        private string title;
        private Boolean toWatch;
        private string type;

        public int Id { get => id; set => id = value; }
        public string Title { 
            get => title; 
            set  { 
                title = value;
                NotifyPropertyChange("Title");
            } 
        }
        public bool ToWatch { get => toWatch; set => toWatch = value; }
        public string Type { get => type; set => type = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Element> GetProgram()
        {
            ObservableCollection<Element> l = new ObservableCollection<Element>();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Films WHERE ToWatch = 1", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Element e = new Element();
                e.Id = DataBase.Instance.reader.GetInt32(0);
                e.Title = DataBase.Instance.reader.GetString(1);
                int w = DataBase.Instance.reader.GetInt32(2);
                if (w == 1)
                    e.ToWatch = true;

                l.Add(e);
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Series WHERE ToWatch = 1", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Element s = new Element();
                s.Id = DataBase.Instance.reader.GetInt32(0);
                s.Title = DataBase.Instance.reader.GetString(1);
                int w = DataBase.Instance.reader.GetInt32(2);
                if (w == 1)
                    s.ToWatch = true;
                l.Add(s);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();
            return l;
        }
        public ObservableCollection<Element> GetElements()
        {
            ObservableCollection<Element> l = new ObservableCollection<Element>();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Films", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Element e = new Element();
                e.Id = DataBase.Instance.reader.GetInt32(0);
                e.Title = DataBase.Instance.reader.GetString(1);
                int w = DataBase.Instance.reader.GetInt32(2);
                if (w == 1)
                    e.ToWatch = true;
                e.Type = "Films";
                l.Add(e);
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Series", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Element s = new Element();
                s.Id = DataBase.Instance.reader.GetInt32(0);
                s.Title = DataBase.Instance.reader.GetString(1);
                int w = DataBase.Instance.reader.GetInt32(2);
                if (w == 1)
                    s.ToWatch = true;
                s.Type = "Series";
                l.Add(s);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return l;
        }

        public Boolean UpdateElement()
        {
            Boolean res = false;
            string req = "UPDATE " + Type + " SET ToWatch = @ToWatch WHERE id = @Id";
            DataBase.Instance.command = new SqlCommand(req, DataBase.Instance.connection);
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

        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
        }
    }
}
