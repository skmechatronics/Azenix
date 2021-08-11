using HttpLogStatisticsGenerator.Model;
using System.Collections.Generic;

namespace HttpLogStatisticsGenerator.Input
{
    public interface IHttpInputParser
    {
        IEnumerable<HttpLogEntryDto> Parse(IEnumerable<RawHttpLogEntry> tokenizedLogs);
    }
}