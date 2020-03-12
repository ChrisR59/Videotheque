using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videothèque2.Tools
{
    public class DataBase
    {
        private static DataBase instance = null;
        private static object _lock = new object();

        public SqlConnection connection { get; set; }
        public SqlCommand command { get; set; }
        public SqlDataReader reader { get; set; }

        private string connectionString = @"Data Source=(localdb)\videotheque;Integrated Security=True";

        public static DataBase Instance
        {
            get
            {
                lock(_lock)
                {
                    if (instance == null)
                        instance = new DataBase();
                    return instance;
                }
            }
        }
        private DataBase()
        {
            connection = new SqlConnection(connectionString);
        }
    }
}
