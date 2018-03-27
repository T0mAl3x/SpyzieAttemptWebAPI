namespace DataLayer.Models
{
    public class CallHistoryModel
    {
        public CallInformation[] Calls { get; set; }
        public int Hash { get; set; }
    }

    public class CallInformation
    {
        public string Number { get; set; }
        public string Date { get; set; }
        public string Duration { get; set; }
    }
}
