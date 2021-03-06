﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Videothèque2.Tools;

namespace Videothèque2.Models
{
    public class Discover : Element
    {
        private string releaseDate;

        public string ReleaseDate { get => releaseDate; set => releaseDate = value; }

        /**
         * Resume :
         *      Add a new Discover in the bdd
         * Return true if insert is successful
         */
        public Boolean Add()
        {
            bool res = false;
            DataBase.Instance.command = new SqlCommand("INSERT INTO Discover (Title, ReleaseDate, ToWatch) OUTPUT INSERTED.ID VALUES (@Title,@ReleaseDate, '0')",
                DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Title", Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@ReleaseDate", ReleaseDate));
            DataBase.Instance.connection.Open();
            Id = (int)DataBase.Instance.command.ExecuteScalar();

            if (Id > 0)
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }

        /**
         * Resume :
         *      Get a Discover list
         * Return an ObservableCollection of the Discover type order by Title
         */
        public ObservableCollection<Discover> GetAll()
        {
            ObservableCollection<Discover> list = new ObservableCollection<Discover>();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ReleaseDate,ToWatch,Comment FROM Discover ORDER BY Title", DataBase.Instance.connection);
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                Discover d = new Discover();
                d.Id = DataBase.Instance.reader.GetInt32(0);
                d.Title = DataBase.Instance.reader.GetString(1);
                d.ReleaseDate = DataBase.Instance.reader.GetString(2);
                d.ToWatch = false;
                int watch = DataBase.Instance.reader.GetInt32(3);

                if (watch == 1)
                    d.ToWatch = true;

                if (!DataBase.Instance.reader.IsDBNull(4))
                    d.Comment = DataBase.Instance.reader.GetString(4);

                list.Add(d);
            }

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return list;
        }

        /*
         * Resume :
         *      Get one serie with his Id
         * Return an Discover object 
         */
        public Discover GetOneWithId(int idFilm)
        {
            Discover d = new Discover();
            DataBase.Instance.command = new SqlCommand("SELECT Id,Title,ToWatch FROM Discover WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("@Id", idFilm));
            DataBase.Instance.connection.Open();
            DataBase.Instance.reader = DataBase.Instance.command.ExecuteReader();

            while (DataBase.Instance.reader.Read())
            {
                d.Id = DataBase.Instance.reader.GetInt32(0);
                d.Title = DataBase.Instance.reader.GetString(1);
                int watch = DataBase.Instance.reader.GetInt32(2);

                if (watch == 1)
                    d.ToWatch = true;
            }
            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return d;
        }

        /**
         * Resume :
         *      Edit fields of the Discover
         * Return true if update is successful
         */
        public Boolean UpdateOne()
        {
            bool res = false;
            DataBase.Instance.command = new SqlCommand("UPDATE Discover SET Title = @Title, ReleaseDate = @ReleaseDate, ToWatch = @ToWatch, Comment = @Comment WHERE Id = @Id",
                DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("Id", Id));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("Title", Title));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("ReleaseDate", ReleaseDate));
            DataBase.Instance.command.Parameters.Add(new SqlParameter("ToWatch", ToWatch));

            if (Comment == null)
                DataBase.Instance.command.Parameters.Add(new SqlParameter("Comment", DBNull.Value));
            else
                DataBase.Instance.command.Parameters.Add(new SqlParameter("Comment", Comment));

            DataBase.Instance.connection.Open();

            if (DataBase.Instance.command.ExecuteNonQuery() > 0)
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }

        /**
         * Resume :
         *      Delete a Discover
         * Return true if delete is successful
         */
        public Boolean DeleteOne()
        {
            bool res = false;

            DataBase.Instance.command = new SqlCommand("DELETE FROM Discover WHERE Id = @Id", DataBase.Instance.connection);
            DataBase.Instance.command.Parameters.Add(new SqlParameter("Id", Id));
            DataBase.Instance.connection.Open();
            if (DataBase.Instance.command.ExecuteNonQuery() > 0)
                res = true;

            DataBase.Instance.command.Dispose();
            DataBase.Instance.connection.Close();

            return res;
        }
    }
}
