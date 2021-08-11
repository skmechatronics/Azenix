namespace HttpLogStatisticsGenerator.Model
{
    /// <summary>
    /// This represents the 
    /// Not all fields are used at this time but it can be extended.
    /// </summary>
    public class RawHttpLogEntry
    {
        public string IPAddress { get; set; }

        public string GroupName { get; set; }

        public string Username { get; set; }

        public string DateTime { get; set; }

        public string Request { get; set; }

        public string Status { get; set; }

        public string ResponseSize { get; set; }

        public string Referrer { get; set; }

        public string UserAgentString { get; set; }
    }
}
