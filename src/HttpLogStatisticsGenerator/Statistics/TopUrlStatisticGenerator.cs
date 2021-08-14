using HttpLogStatisticsGenerator.Model;
using System.Collections.Generic;
using System.Linq;

namespace HttpLogStatisticsGenerator.Statistics
{
    public class TopUrlStatisticGenerator : AbstractTopStatisticGenerator
    {
        private const int UrlsToShow = 3;

        private const string Separator = ", ";

        public override StatisticResult Process(IEnumerable<HttpLogEntryDto> httpLogs)
        {
            var topIpAddresses = this.GetTopNEntities(httpLogs, log => log.SubPath, UrlsToShow);
            var hits = topIpAddresses.Count();
            var message = $"The top {hits} most visited URLs are : {string.Join(Separator, topIpAddresses)}";
            return new StatisticResult { Message = message };
        }
    }
}
