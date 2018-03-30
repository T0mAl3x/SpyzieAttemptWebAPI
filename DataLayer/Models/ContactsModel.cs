using System.Collections.Generic;

namespace DataLayer.Models
{
    public class ContactsModel
    {
        public List<ContactList> ContactList { get; set; }
        public string Hash { get; set; }
    }

    public class ContactList
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string Picture { get; set; }
    }
}
