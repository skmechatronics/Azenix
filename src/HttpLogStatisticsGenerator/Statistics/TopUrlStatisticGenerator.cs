using HttpLogStatisticsGenerator.Model;
using System.Collections.Generic;

namespace HttpLogStatisticsGenerator.Statistics
{
    public class TopUrlStatisticGenerator : AbstractTopStatisticGenerator
    {
        private const int UrlsToShow = 3;

        private const string Separator = ", ";

        public override StatisticResult Generate(IEnumerable<HttpLogEntryDto> httpLogs)
        {
            var topIpAddresses = this.GetTopNEntities(httpLogs, log => log.SubPath, UrlsToShow);
            var message = $"The top {UrlsToShow} most visited URLs are : {string.Join(Separator, topIpAddresses)}";
            return new StatisticResult { Message = message };
        }
    }
}
