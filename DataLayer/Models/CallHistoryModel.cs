using System.Collections.Generic;

namespace DataLayer.Models
{
    public class CallHistoryModel
    {
        public List<CallInformation> Calls { get; set; }
        public string Hash { get; set; }
    }

    public class CallInformation
    {
        public string Number { get; set; }
        public string Date { get; set; }
        public string Duration { get; set; }
        public string Direction { get; set; }
    }
}
