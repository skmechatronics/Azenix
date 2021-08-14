﻿using HttpLogStatisticsGenerator.Model;
using System.Collections.Generic;

using System.Linq;

namespace HttpLogStatisticsGenerator.Statistics
{
    public class UniqueIpAddressStatistic : IStatisticsGenerator
    {
        public StatisticResult Generate(IEnumerable<HttpLogEntryDto> httpLogs)
        {
            var uniqueIPAddresses = httpLogs.Select(i => i.IPAddress).Distinct().Count();
            var message = $"The number of unique IP Addresses are: {uniqueIPAddresses}";
            return new StatisticResult { Message = message };
        }
    }
}
