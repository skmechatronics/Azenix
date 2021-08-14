using HttpLogStatisticsGenerator.Model;
using System.Collections.Generic;

namespace HttpLogStatisticsGenerator.Statistics
{
    public interface IStatisticGenerator
    {
        StatisticResult Process(IEnumerable<HttpLogEntryDto> httpLogs);
    }
}
