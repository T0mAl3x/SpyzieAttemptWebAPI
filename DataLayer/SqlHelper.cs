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

        public static string GetMask(string connectionString, string IMEI, string secToken)
        {
            ConnectionManagement connection = ConnectionManagement.getInstance(connectionString);
            DataTable table = new DataTable();
            using (SqlCommand command = new SqlCommand("GetMask", connection.GetConnection()))
            {
                string response = "";
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
                        ParameterName = "@SecToken",
                        Value = secToken,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter()
                    {
                        ParameterName = "@Status",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    connection.openConnection();
                    SqlDataReader reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();

                    response = (string)command.Parameters["@Status"].Value;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    connection.closeConnection();
                }

                if (response == "Authentication failed")
                {
                    return response;
                }
                else
                {
                    DataRow row = table.Rows[0];
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.ColumnName == "Mask")
                        {
                            response += row[column.ColumnName].ToString() + ";";
                        }
                        else
                        {
                            response += row[column.ColumnName].ToString() + ":";
                        }
                    }
                    response.Remove(response.Length - 1);
                    return response;
                }
            }
        }
    }
}
