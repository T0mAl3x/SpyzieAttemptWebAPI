using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class DataChecker
    {
        public static bool CheckLocation(LocationModel location)
        {
            if (location == null)
            {
                return false;
            }

            if (location.Latitude != null && location.Longitude != null && location.Hash != 0)
            {
                return true;
            }

            return false;
        }

        public static bool CheckAuthentication(PhoneAuthenticationModel credentials)
        {
            if (credentials == null)
            {
                return false;
            }

            if (credentials.IMEI != null && credentials.SecToken != null)
            {
                return true;
            }

            return false;
        }
    }
}
