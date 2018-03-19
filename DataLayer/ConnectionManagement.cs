using System.Data.SqlClient;
using System;

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
            Connection.Open();
            if (Connection.State == System.Data.ConnectionState.Broken || Connection.State == System.Data.ConnectionState.Closed)
            {
                throw new Exception("Connection could not be opened");
            }
        }

        public void closeConnection()
        {
            Connection.Close();
            if (Connection.State != System.Data.ConnectionState.Closed)
            {
                throw new Exception("Connection could not be closed");
            }
        }

        public SqlConnection GetConnection()
        {
            return Connection;
        }
    }
}
