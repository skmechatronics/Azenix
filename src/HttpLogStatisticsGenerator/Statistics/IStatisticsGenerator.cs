using HttpLogStatisticsGenerator.Model;
using System.Collections.Generic;

namespace HttpLogStatisticsGenerator.Statistics
{
    public interface IStatisticsGenerator
    {
        StatisticResult Generate(IEnumerable<HttpLogEntryDto> httpLogs);
    }
}
