namespace HeadCount.Core.Models
{
    public enum Status
    {
        NoResponse,
        Yes,
        No,
        Maybe
    }
    public class Guest
    {
        public Status Status { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public string Response { get; set; }
    }
}