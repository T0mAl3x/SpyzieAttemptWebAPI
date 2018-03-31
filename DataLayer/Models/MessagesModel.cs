using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class MessagesModel
    {
        public List<Message> Messages { get; set; }
        public string Hash { get; set; }
    }

    public class Message
    {
        public string Address { get; set; }
        public string Body { get; set; }
        public string State { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
    }
}
