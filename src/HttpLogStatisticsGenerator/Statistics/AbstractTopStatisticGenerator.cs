using HttpLogStatisticsGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HttpLogStatisticsGenerator.Statistics
{
    public abstract class AbstractTopStatisticGenerator : IStatisticsGenerator
    {
        public abstract StatisticResult Generate(IEnumerable<HttpLogEntryDto> httpLogs);

        public IEnumerable<T> GetTopNEntities<T>(
            IEnumerable<HttpLogEntryDto> httpLogs,
            Func<HttpLogEntryDto, T> propertySelector,
            int entitiesToShow)
        {
            var topIpAddresses = httpLogs.GroupBy(i => propertySelector(i))
                                        .Select(g => new { Entity = g.Key, Count = g.Count() })
                                        .OrderByDescending(g => g.Count)
                                        .Take(entitiesToShow)
                                        .Select(result => result.Entity);

            return topIpAddresses;
        }
    }
}
