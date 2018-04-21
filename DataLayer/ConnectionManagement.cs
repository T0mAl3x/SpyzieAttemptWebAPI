using System.Data.SqlClient;
using System;
using System.Data;

namespace DataLayer
{
    public class ConnectionManagement
    {
        private static ConnectionManagement myInstance = null;

        private SqlConnection Connection = null;

        private ConnectionManagement(string connectionString)
        {
            Connection = new SqlConnection { ConnectionString = connectionString };
        }

        public static ConnectionManagement getInstance(string connectionString)
        {
            if (myInstance == null)
            {
                myInstance = new ConnectionManagement(connectionString);
            }

            return myInstance;
        }

        public void openConnection()
        {
            if (Connection != null && Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        public void closeConnection()
        {
            if (Connection != null && Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            return Connection;
        }
    }
}
