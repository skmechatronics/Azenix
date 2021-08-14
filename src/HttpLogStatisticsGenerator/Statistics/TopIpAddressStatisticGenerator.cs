using HttpLogStatisticsGenerator.Model;
using System.Collections.Generic;
using System.Linq;

namespace HttpLogStatisticsGenerator.Statistics
{
    public class TopIpAddressStatisticGenerator : AbstractTopStatisticGenerator
    {
        private const int IpAddressToShow = 3;

        private const string Separator = ", ";

        public override StatisticResult Process(IEnumerable<HttpLogEntryDto> httpLogs)
        {
            var topIpAddresses = this.GetTopNEntities(httpLogs, log => log.IPAddress, IpAddressToShow);
            var hits = topIpAddresses.Count();
            var message = $"The top {hits} most active IP Addresses are : {string.Join(Separator, topIpAddresses)}";
            return new StatisticResult { Message = message };
        }
    }
}
