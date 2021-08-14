using HttpLogStatisticsGenerator.Model;
using System.Collections.Generic;

namespace HttpLogStatisticsGenerator.Input
{
    public interface IHttpLogParser
    {
        IEnumerable<HttpLogEntryDto> Parse(IEnumerable<RawHttpLogEntry> tokenizedLogs);
    }
}