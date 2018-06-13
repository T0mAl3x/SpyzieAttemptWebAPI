using System;
using System.Data;
using System.Data.SqlClient;
using DataLayer.Models;
using System.Text;

namespace DataLayer
{
    public class SqlHelper
    {
        public static string RegisterPhone(string connectionString, string IMEI, string manufacturer, string model, string username, string number)
        {
            using (SqlCommand command = new SqlCommand("PhoneRegistration"))
            {
                bool response = true;
                string secToken = TokenGenerator.RandomString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        command.Connection = connection;
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
                            ParameterName = "@Username",
                            Value = username,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter()
                        {
                            ParameterName = "@Number",
                            Value = number,
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
                        command.ExecuteNonQuery();

                        response = (bool)command.Parameters["@Response"].Value;
                    }
                    catch (SqlException ex)
                    {
                        secToken = null;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                if (response)
                {
                    return "Already registered";
                }
                else
                {
                    if (secToken != null)
                    {
                        return secToken;
                    }
                    else
                    {
                        return "fail";
                    }
                }
            }
        }

        private static bool AuthenticatePhone(string connectionString, PhoneAuthenticationModel credentials)
        {
            if (DataChecker.CheckAuthentication(credentials))
            {
                bool response = false;
                using (SqlCommand command = new SqlCommand("AuthenticatePhone"))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            command.Connection = connection;
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

                            command.ExecuteNonQuery();
                            response = (bool)command.Parameters["@Status"].Value;
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            connection.Close();
                        }
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
            if (AuthenticatePhone(connectionString, new PhoneAuthenticationModel() { IMEI = IMEI, SecToken = secToken }))
            {
                DataTable table = new DataTable();
                using (SqlCommand command = new SqlCommand("GetMask"))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string response = "";
                        try
                        {
                            connection.Open();
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            SqlParameter parameter = new SqlParameter()
                            {
                                ParameterName = "@IMEI",
                                Value = IMEI,
                                SqlDbType = SqlDbType.NVarChar,
                                Direction = ParameterDirection.Input
                            };
                            command.Parameters.Add(parameter);
                            
                            SqlDataReader reader = command.ExecuteReader();
                            table.Load(reader);
                            reader.Close();

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
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            connection.Close();
                        }
                        return response;
                    }
                }
            }
            else
            {
                return "Authentication failed";
            }
            
        }

        public static string GetUserMask(string connectionString)
        {
            string mask = "";
            using (SqlCommand command = new SqlCommand("GetUserMask"))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter parameter = new SqlParameter()
                        {
                            ParameterName = "@UserMask",
                            Size = 9,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                        
                        command.ExecuteNonQuery();
                        mask = (string)command.Parameters["@UserMask"].Value;
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return mask;
            }
        }

        public static void SetUserMask(string connectionString, StringBuilder userMask)
        {
            DataTable table = new DataTable();
            using (SqlCommand command = new SqlCommand("SetUserMask"))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter parameter = new SqlParameter()
                        {
                            ParameterName = "@UserMask",
                            Value = userMask.ToString(),
                            SqlDbType = SqlDbType.Char,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }
                }    
            }
        }

        public static DataTable GetConversation(string connectionString, string IMEI, string number)
        {
            DataTable table = new DataTable();
            using (SqlCommand command = new SqlCommand("GetConversation"))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        command.Connection = connection;
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
                            ParameterName = "@Number",
                            Value = number,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);

                        SqlDataReader reader = command.ExecuteReader();
                        table.Load(reader);
                        reader.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return table;
        }

        public static DataTable GetSpecificInformation(string connectionString, string IMEI, string storedProcedure)
        {
            DataTable table = new DataTable();
            using (SqlCommand command = new SqlCommand(storedProcedure))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter parameter = new SqlParameter()
                        {
                            ParameterName = "@IMEI",
                            Value = IMEI,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);
                        
                        SqlDataReader reader = command.ExecuteReader();
                        table.Load(reader);
                        reader.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return table;
        }

        public static DataTable GetSmartphones(string connectionString, string username)
        {
            DataTable table = new DataTable();
            using (SqlCommand command = new SqlCommand("GetSmartphones"))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter parameter = new SqlParameter()
                        {
                            ParameterName = "@Username",
                            Value = username,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);
                        
                        SqlDataReader reader = command.ExecuteReader();
                        table.Load(reader);
                        reader.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return table;
        }

        public static string GatherData(string connectionString, BulkDataModel bulkData)
        {

            if (AuthenticatePhone(connectionString, bulkData.Authentication))
            {
                if (DataChecker.CheckKeylogger(bulkData.Keylogger))
                {
                    using (SqlCommand command = new SqlCommand("InsertKeystrokes"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                command.Connection = connection;
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
                                    ParameterName = "@Info",
                                    Value = bulkData.Keylogger.Info,
                                    SqlDbType = SqlDbType.NText,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Hash",
                                    Value = bulkData.Keylogger.Hash,
                                    SqlDbType = SqlDbType.NText,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }
                if (DataChecker.CheckLocation(bulkData.Location))
                {
                    using (SqlCommand command = new SqlCommand("InsertLocation"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                command.Connection = connection;
                                command.CommandType = CommandType.StoredProcedure;
                                SqlParameter parameter = new SqlParameter()
                                {
                                    ParameterName = "@IMEI",
                                    Value = bulkData.Authentication.IMEI,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                DataTable table = new DataTable();
                                for (int i = 0; i < 3; i++)
                                {
                                    table.Columns.Add();
                                }

                                for (int i = 0; i < bulkData.Location.Locations.Count; i++)
                                {
                                    DataRow row = table.NewRow();

                                    row[0] = bulkData.Location.Locations[i].Latitude;
                                    row[1] = bulkData.Location.Locations[i].Longitude;
                                    row[2] = bulkData.Location.Locations[i].Date;

                                    table.Rows.Add(row);
                                }
                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Locations",
                                    Value = table,
                                    SqlDbType = SqlDbType.Structured,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                string hashValue;
                                if (bulkData.Location.Hash != null)
                                {
                                    hashValue = bulkData.Location.Hash;
                                }
                                else
                                {
                                    hashValue = "DB";
                                }
                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Hash",
                                    Value = hashValue,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);
                                
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                connection.Close() ;
                            }
                        }
                    }
                }
                if (DataChecker.CheckCalls(bulkData.CallHistory))
                {
                    using (SqlCommand command = new SqlCommand("InsertCallHistory"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                command.Connection = connection;
                                command.CommandType = CommandType.StoredProcedure;
                                SqlParameter parameter = new SqlParameter()
                                {
                                    ParameterName = "@IMEI",
                                    Value = bulkData.Authentication.IMEI,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                DataTable table = new DataTable();
                                for (int i = 0; i < 4; i++)
                                {
                                    table.Columns.Add();
                                }

                                for (int i = 0; i < bulkData.CallHistory.Calls.Count; i++)
                                {
                                    DataRow row = table.NewRow();

                                    row[0] = bulkData.CallHistory.Calls[i].Number;
                                    row[1] = bulkData.CallHistory.Calls[i].Date;
                                    row[2] = bulkData.CallHistory.Calls[i].Direction;
                                    row[3] = bulkData.CallHistory.Calls[i].Duration;

                                    table.Rows.Add(row);
                                }
                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Calls",
                                    Value = table,
                                    SqlDbType = SqlDbType.Structured,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Hash",
                                    Value = bulkData.CallHistory.Hash,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);
                                
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }
                if (DataChecker.CheckContacts(bulkData.Contacts))
                {
                    using (SqlCommand command = new SqlCommand("InsertContacts"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                command.Connection = connection;
                                command.CommandType = CommandType.StoredProcedure;
                                SqlParameter parameter = new SqlParameter()
                                {
                                    ParameterName = "@IMEI",
                                    Value = bulkData.Authentication.IMEI,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                DataTable table = new DataTable();
                                for (int i = 0; i < 3; i++)
                                {
                                    table.Columns.Add();
                                }

                                for (int i = 0; i < bulkData.Contacts.ContactList.Count; i++)
                                {
                                    DataRow row = table.NewRow();

                                    row[0] = bulkData.Contacts.ContactList[i].Name;
                                    row[1] = bulkData.Contacts.ContactList[i].Number;
                                    row[2] = bulkData.Contacts.ContactList[i].Picture;

                                    table.Rows.Add(row);
                                }
                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Contacts",
                                    Value = table,
                                    SqlDbType = SqlDbType.Structured,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Hash",
                                    Value = bulkData.Contacts.Hash,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);
                                
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }

                if (DataChecker.CheckMessages(bulkData.Messages))
                {
                    using (SqlCommand command = new SqlCommand("InsertMessages"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                command.Connection = connection;
                                command.CommandType = CommandType.StoredProcedure;
                                SqlParameter parameter = new SqlParameter()
                                {
                                    ParameterName = "@IMEI",
                                    Value = bulkData.Authentication.IMEI,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                DataTable table = new DataTable();
                                for (int i = 0; i < 5; i++)
                                {
                                    table.Columns.Add();
                                }

                                for (int i = 0; i < bulkData.Messages.Messages.Count; i++)
                                {
                                    DataRow row = table.NewRow();

                                    row[0] = bulkData.Messages.Messages[i].Address;
                                    row[1] = bulkData.Messages.Messages[i].Body;
                                    row[2] = bulkData.Messages.Messages[i].State;
                                    row[3] = bulkData.Messages.Messages[i].Date;
                                    row[4] = bulkData.Messages.Messages[i].Type;

                                    table.Rows.Add(row);
                                }
                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Messages",
                                    Value = table,
                                    SqlDbType = SqlDbType.Structured,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Hash",
                                    Value = bulkData.Messages.Hash,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }

                if (DataChecker.CheckTrafic(bulkData.Trafic))
                {
                    using (SqlCommand command = new SqlCommand("InsertTrafic"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                command.Connection = connection;
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
                                    ParameterName = "@Trafic",
                                    Value = bulkData.Trafic.Trafic,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Date",
                                    Value = DateTime.Now.ToString("yyyy-MM-ddTHH':'mm':'sszzz"),
                                    SqlDbType = SqlDbType.DateTime,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Hash",
                                    Value = bulkData.Trafic.Hash,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);
                                
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                connection.Close();
                            }
                        } 
                    }
                }

                if (DataChecker.CheckApplications(bulkData.Applications))
                {
                    using (SqlCommand command = new SqlCommand("InsertApplications"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                command.Connection = connection;
                                command.CommandType = CommandType.StoredProcedure;
                                SqlParameter parameter = new SqlParameter()
                                {
                                    ParameterName = "@IMEI",
                                    Value = bulkData.Authentication.IMEI,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                DataTable table = new DataTable();
                                for (int i = 0; i < 2; i++)
                                {
                                    table.Columns.Add();
                                }

                                for (int i = 0; i < bulkData.Applications.Applications.Count; i++)
                                {
                                    DataRow row = table.NewRow();

                                    row[0] = bulkData.Applications.Applications[i].Name;
                                    row[1] = bulkData.Applications.Applications[i].Icon;

                                    table.Rows.Add(row);
                                }
                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Applications",
                                    Value = table,
                                    SqlDbType = SqlDbType.Structured,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Hash",
                                    Value = bulkData.Applications.Hash,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);
                                
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }

                if (bulkData.BatteryLevel != 0)
                {
                    using (SqlCommand command = new SqlCommand("InsertBatteryLevel"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                command.Connection = connection;
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
                                    ParameterName = "@Level",
                                    Value = bulkData.BatteryLevel,
                                    SqlDbType = SqlDbType.Float,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);
                                
                                command.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }

                if (DataChecker.CheckPhotos(bulkData.Photos))
                {
                    using (SqlCommand command = new SqlCommand("InsertPhotos"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                command.Connection = connection;
                                command.CommandType = CommandType.StoredProcedure;
                                SqlParameter parameter = new SqlParameter()
                                {
                                    ParameterName = "@IMEI",
                                    Value = bulkData.Authentication.IMEI,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                DataTable table = new DataTable();
                                for (int i = 0; i < 4; i++)
                                {
                                    table.Columns.Add();
                                }

                                for (int i = 0; i < bulkData.Photos.Photos.Count; i++)
                                {
                                    DataRow row = table.NewRow();

                                    row[0] = bulkData.Photos.Photos[i].Image;
                                    row[1] = bulkData.Photos.Photos[i].Date;
                                    row[2] = bulkData.Photos.Photos[i].Latitude;
                                    row[3] = bulkData.Photos.Photos[i].Longitude;

                                    table.Rows.Add(row);
                                }
                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Photos",
                                    Value = table,
                                    SqlDbType = SqlDbType.Structured,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Hash",
                                    Value = bulkData.Photos.Hash,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);
                                
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                connection.Close();
                            }
                        }   
                    }
                }

                if (DataChecker.CheckMetadata(bulkData.Metadata))
                {
                    using (SqlCommand command = new SqlCommand("InsertMetadata"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                command.Connection = connection;
                                command.CommandType = CommandType.StoredProcedure;
                                SqlParameter parameter = new SqlParameter()
                                {
                                    ParameterName = "@IMEI",
                                    Value = bulkData.Authentication.IMEI,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                DataTable table = new DataTable();
                                for (int i = 0; i < 3; i++)
                                {
                                    table.Columns.Add();
                                }

                                for (int i = 0; i < bulkData.Metadata.Metadata.Count; i++)
                                {
                                    string[] metadata = bulkData.Metadata.Metadata[i].Split(';');

                                    DataRow row = table.NewRow();

                                    row[0] = metadata[0];
                                    row[1] = metadata[1];
                                    row[2] = metadata[2];

                                    table.Rows.Add(row);
                                }
                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Metadata",
                                    Value = table,
                                    SqlDbType = SqlDbType.Structured,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);

                                parameter = new SqlParameter()
                                {
                                    ParameterName = "@Hash",
                                    Value = bulkData.Metadata.Hash,
                                    SqlDbType = SqlDbType.NVarChar,
                                    Direction = ParameterDirection.Input
                                };
                                command.Parameters.Add(parameter);
                                
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                connection.Close();
                            }
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

        public static string RegisterUser(string connectionString, string firstname, string lastname, string username, string password, string email)
        {
            bool sqlResponse = true;
            using (SqlCommand command = new SqlCommand("InsertUser"))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter parameter = new SqlParameter()
                        {
                            ParameterName = "@Firstname",
                            Value = firstname,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter()
                        {
                            ParameterName = "@Lastname",
                            Value = lastname,
                            SqlDbType = SqlDbType.NVarChar,
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
                            ParameterName = "@Password",
                            Value = password,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter()
                        {
                            ParameterName = "@Email",
                            Value = email,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter()
                        {
                            ParameterName = "@Out",
                            SqlDbType = SqlDbType.Bit,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                        
                        command.ExecuteNonQuery();

                        sqlResponse = (bool)command.Parameters["@Out"].Value;
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                    
            }

            if (sqlResponse)
            {
                return "Ok";
            }
            else
            {
                return "Already registered";
            }
        }

        public static bool LogInUser(string connectionString, string username, string password)
        {
            bool sqlResponse = false;
            using (SqlCommand command = new SqlCommand("LogInUser"))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter parameter = new SqlParameter()
                        {
                            ParameterName = "@Username",
                            Value = username,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter()
                        {
                            ParameterName = "@Password",
                            Value = password,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter()
                        {
                            ParameterName = "@Out",
                            SqlDbType = SqlDbType.Bit,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                        
                        command.ExecuteNonQuery();

                        sqlResponse = (bool)command.Parameters["@Out"].Value;
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return sqlResponse;
            }
        }
    }
}
