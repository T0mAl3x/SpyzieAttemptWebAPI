using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DataLayer
{
    public class SqlHelper
    {
        public static string RegisterPhone(string connectionString, string IMEI, string manufacturer, string model, string username)
        {
            ConnectionManagement connection = ConnectionManagement.getInstance(connectionString);
            using (SqlCommand command = new SqlCommand("PhoneRegistration", connection.GetConnection()))
            {
                bool response = true;
                string secToken = TokenGenerator.RandomString();
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameter = new SqlParameter()
                    {
                        ParameterName = "@IMEI",
                        Value = IMEI,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter()
                    {
                        ParameterName = "@Manufacturer",
                        Value = manufacturer,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter()
                    {
                        ParameterName = "@Model",
                        Value = model,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);

                    //Generating security token
                    parameter = new SqlParameter()
                    {
                        ParameterName = "@SecToken",
                        Value = secToken,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter()
                    {
                        ParameterName = "@Mask",
                        Value = "00000000",
                        SqlDbType = SqlDbType.Char,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter()
                    {
                        ParameterName = "@Username",
                        Value = username,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter()
                    {
                        ParameterName = "@Response",
                        SqlDbType = SqlDbType.Bit,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                    connection.openConnection();
                    command.ExecuteNonQuery();

                    response = (bool)command.Parameters["@Response"].Value;
                }
                catch (SqlException ex)
                {

                }
                finally
                {
                    connection.closeConnection();
                }

                if (response)
                {
                    return "Already registered";
                }
                else
                {
                    return secToken;
                }
            }
        }
    }
}
