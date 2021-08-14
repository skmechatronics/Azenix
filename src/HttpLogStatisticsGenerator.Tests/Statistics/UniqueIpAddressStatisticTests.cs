using FluentAssertions;
using HttpLogStatisticsGenerator.Model;
using HttpLogStatisticsGenerator.Statistics;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;

namespace HttpLogStatisticsGenerator.Tests.Statistics
{
    [TestFixture]
    public class UniqueIpAddressStatisticTests
    {
        private UniqueIpAddressStatisticGenerator uniqueIpAddressStatisticGenerator;

        [SetUp]
        public void SetUp()
        {
            this.uniqueIpAddressStatisticGenerator = new UniqueIpAddressStatisticGenerator();
        }

        [Test]
        public void When_there_are_duplicate_ips_the_unique_count_should_be_returned()
        {
            var duplicates = new List<HttpLogEntryDto>
            {
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.1")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.1")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.1")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.2")},
            };

            var uniqueCount = this.uniqueIpAddressStatisticGenerator.Process(duplicates);
            uniqueCount.Message.Should().Be("The number of unique IP Addresses are: 2");
        }
    }
}
