using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class DataChecker
    {
        public static bool CheckPhoneRegistration(PhoneRegistrationModel model)
        {
            if (model == null)
            {
                return false;
            }
            if (model.IMEI != null && model.Manufacturer != null && model.Model != null && model.Username != null)
            {
                return true;
            }
            return false;
        }
        public static bool CheckUserCredentials(UserAuthenticationModel model)
        {
            if (model == null)
            {
                return false;
            }
            if (model.Password != null && model.Username != null)
            {
                return true;
            }
            return false;
        }
        public static bool CheckLocation(LocationModel location)
        {
            if (location == null)
            {
                return false;
            }

            if (location.Latitude != null && location.Longitude != null && location.Hash != null)
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

        public static bool CheckCalls(CallHistoryModel callHistory)
        {
            if(callHistory == null)
            {
                return false;
            }

            if (callHistory.Calls != null && callHistory.Hash != null)
            {
                return true;
            }

            return false;
        }

        public static bool CheckContacts(ContactsModel contacts)
        {
            if (contacts == null)
            {
                return false;
            }

            if (contacts.ContactList != null && contacts.Hash != null)
            {
                return true;
            }

            return false;
        }

        public static bool CheckMessages(MessagesModel messages)
        {
            if (messages == null)
            {
                return false;
            }

            if (messages.Messages != null && messages.Hash != null)
            {
                return true;
            }

            return false;
        }

        public static bool CheckTrafic(TraficModel trafic)
        {
            if (trafic == null)
            {
                return false;
            }

            if (trafic.Trafic != null && trafic.Hash != null)
            {
                return true;
            }

            return false;
        }

        public static bool CheckApplications(ApplicationsModel applications)
        {
            if (applications == null)
            {
                return false;
            }

            if (applications.Applications != null && applications.Hash != null)
            {
                return true;
            }

            return false;
        }

        public static bool CheckPhotos(PhotosModel photos)
        {
            if (photos == null)
            {
                return false;
            }

            if (photos.Photos != null && photos.Hash != null)
            {
                return true;
            }

            return false;
        }

        public static bool CheckMetadata(List<string> metadata)
        {
            if (metadata == null || metadata.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
