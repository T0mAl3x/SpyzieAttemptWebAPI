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
                    bulkData.Authentication.IMEI = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.Authentication.IMEI)));
                    bulkData.Authentication.SecToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.Authentication.SecToken)));
                }

                if (DataChecker.CheckLocation(bulkData.Location))
                {
                    bulkData.Location.Latitude = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.Location.Latitude)));
                    bulkData.Location.Longitude = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.Location.Longitude)));
                }

                if (DataChecker.CheckCalls(bulkData.CallHistory))
                {
                    for (int i=0; i<bulkData.CallHistory.Calls.Count; i++)
                    {
                        bulkData.CallHistory.Calls[i].Date = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.CallHistory.Calls[i].Date)));
                        bulkData.CallHistory.Calls[i].Direction = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.CallHistory.Calls[i].Direction)));
                        bulkData.CallHistory.Calls[i].Duration = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.CallHistory.Calls[i].Duration)));
                        bulkData.CallHistory.Calls[i].Number = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.CallHistory.Calls[i].Number)));
                    }
                    bulkData.CallHistory.Hash = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(MakeUrlSafe(bulkData.CallHistory.Hash)));
                }

                if(DataChecker.CheckContacts(bulkData.Contacts))
                {

                }
                return bulkData;
            }
            return null;
        }

        private static string MakeUrlSafe(string value)
        {
            return value.Replace('+', '-').Replace('/', '_');
        }

        private static string MakeUrlUnsafe(string value)
        {
            return value.Replace('-', '+').Replace('_', '/');
        }
    }
}