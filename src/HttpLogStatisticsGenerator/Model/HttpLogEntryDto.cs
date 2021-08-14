using System.Net;

namespace HttpLogStatisticsGenerator.Model
{
    /// <summary>
    /// Note that this doesn't contain the complete set of properties
    /// but they can be added as needed.
    /// </summary>
    public class HttpLogEntryDto
    {
        public IPAddress IPAddress { get; set; }

        public string SubPath { get; set; }
    }
}
