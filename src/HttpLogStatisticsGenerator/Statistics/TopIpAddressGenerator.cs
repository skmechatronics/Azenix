using HttpLogStatisticsGenerator.Model;
using System.Collections.Generic;

namespace HttpLogStatisticsGenerator.Statistics
{
    public class TopIpAddressGenerator : AbstractTopStatisticGenerator
    {
        private const int IpAddressToShow = 3;

        private const string Separator = ", ";

        public override StatisticResult Generate(IEnumerable<HttpLogEntryDto> httpLogs)
        {
            var topIpAddresses = this.GetTopNEntities(httpLogs, log => log.IPAddress, IpAddressToShow);
            var message = $"The top {IpAddressToShow} most active Ip Addresses are : {string.Join(Separator, topIpAddresses)}";
            return new StatisticResult { Message = message };
        }
    }
}
