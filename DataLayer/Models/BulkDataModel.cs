using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class BulkDataModel
    {
        public PhoneAuthenticationModel Authentication { get; set; }
        public LocationModel Location { get; set; }
        public CallHistoryModel CallHistory { get; set; }
        public ContactsModel Contacts { get; set; }
        public MessagesModel Messages { get; set; }
    }
}
