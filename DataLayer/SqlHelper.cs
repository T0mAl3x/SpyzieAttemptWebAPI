using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DataLayer.Models;

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

        private static bool AuthenticatePhone(string connectionString, PhoneAuthenticationModel credentials)
        {
            if (DataChecker.CheckAuthentication(credentials))
            {
                ConnectionManagement connection = ConnectionManagement.getInstance(connectionString);
                bool response = false;
                using (SqlCommand command = new SqlCommand("AuthenticatePhone", connection.GetConnection()))
                {

                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter parameter = new SqlParameter()
                        {
                            ParameterName = "@IMEI",
                            Value = credentials.IMEI,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter()
                        {
                            ParameterName = "@SecToken",
                            Value = credentials.SecToken,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter()
                        {
                            ParameterName = "@Status",
                            SqlDbType = SqlDbType.Bit,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);

                        connection.openConnection();
                        command.ExecuteNonQuery();

                        response = (bool)command.Parameters["@Status"].Value;
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.closeConnection();
                    }
                }
                return response;
            }
            else
            {
                return false;
            }
        }

        public static string GetMask(string connectionString, string IMEI, string secToken)
        {
            ConnectionManagement connection = ConnectionManagement.getInstance(connectionString);

            if (AuthenticatePhone(connectionString, new PhoneAuthenticationModel() { IMEI = IMEI, SecToken = secToken }))
            {
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

                        connection.openConnection();
                        SqlDataReader reader = command.ExecuteReader();
                        table.Load(reader);
                        reader.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.closeConnection();
                    }
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
            else
            {
                return "Authentication failed";
            }
            
        }

        public static string GatherData(string connectionString, BulkDataModel bulkData)
        {
            ConnectionManagement connection = ConnectionManagement.getInstance(connectionString);

            if (AuthenticatePhone(connectionString, bulkData.Authentication))
            {
                if (DataChecker.CheckLocation(bulkData.Location))
                {
                    using (SqlCommand command = new SqlCommand("InsertLocation", connection.GetConnection()))
                    {
                        try
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            SqlParameter parameter = new SqlParameter()
                            {
                                ParameterName = "@IMEI",
                                Value = bulkData.Authentication.IMEI,
                                SqlDbType = SqlDbType.NVarChar,
                                Direction = ParameterDirection.Input
                            };
                            command.Parameters.Add(parameter);

                            parameter = new SqlParameter()
                            {
                                ParameterName = "@Latitude",
                                Value = Double.Parse(bulkData.Location.Latitude),
                                SqlDbType = SqlDbType.Float,
                                Direction = ParameterDirection.Input
                            };
                            command.Parameters.Add(parameter);

                            parameter = new SqlParameter()
                            {
                                ParameterName = "@Longitude",
                                Value = Double.Parse(bulkData.Location.Longitude),
                                SqlDbType = SqlDbType.Float,
                                Direction = ParameterDirection.Input
                            };
                            command.Parameters.Add(parameter);

                            parameter = new SqlParameter()
                            {
                                ParameterName = "@DateTime",
                                Value = DateTime.Now.ToString("yyyy-MM-ddTHH':'mm':'sszzz"),
                                SqlDbType = SqlDbType.DateTime,
                                Direction = ParameterDirection.Input
                            };
                            command.Parameters.Add(parameter);

                            parameter = new SqlParameter()
                            {
                                ParameterName = "@Hash",
                                Value = bulkData.Location.Hash,
                                SqlDbType = SqlDbType.Int,
                                Direction = ParameterDirection.Input
                            };
                            command.Parameters.Add(parameter);

                            connection.openConnection();
                            command.ExecuteNonQuery();
                        }
                        catch(Exception ex)
                        {

                        }
                        finally
                        {
                            connection.closeConnection();
                        }
                    }
                }
                if (DataChecker.CheckCalls(bulkData.CallHistory))
                {
                    using(SqlCommand command = new SqlCommand("InsertCallHistory", connection.GetConnection()))
                    {
                        try
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            SqlParameter parameter = new SqlParameter()
                            {
                                ParameterName = "@IMEI",
                                Value = bulkData.Authentication.IMEI,
                                SqlDbType = SqlDbType.NVarChar,
                                Direction = ParameterDirection.Input
                            };
                            command.Parameters.Add(parameter);

                            //TODO: continua!
                        }
                        catch(Exception ex)
                        {

                        }
                        finally
                        {
                            connection.closeConnection();
                        }
                    }
                }
                return "";
            }
            else
            {
                return "Authentication failed";
            }
        }
    }
}
