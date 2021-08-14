using FluentAssertions;
using HttpLogStatisticsGenerator.Model;
using HttpLogStatisticsGenerator.Statistics;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;

namespace HttpLogStatisticsGenerator.Tests.Statistics
{
    [TestFixture]
    public class TopIpAddressStatisticGeneratorTests
    {

        private TopIpAddressStatisticGenerator topIpAddressStatisticGenerator;

        [SetUp]
        public void SetUp()
        {
            this.topIpAddressStatisticGenerator = new TopIpAddressStatisticGenerator();
        }

        [Test]
        public void When_there_are_multiple_ip_addresses_hit_expect_top_ips_to_be_correct()
        {
            var duplicates = new List<HttpLogEntryDto>
            {
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.1")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.1")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.2")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.2")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.1")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.1")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.4")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.4")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.3")},
            };

            var uniqueCount = this.topIpAddressStatisticGenerator.Process(duplicates);
            uniqueCount.Message.Should().Be("The top 3 most active IP Addresses are : 127.0.0.1, 127.0.0.2, 127.0.0.4");
        }

        [Test]
        public void When_there_are_fewer_ip_addresses_than_expected_then_expect_top_ips_to_be_correct()
        {
            var duplicates = new List<HttpLogEntryDto>
            {
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.1")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.1")},
                new HttpLogEntryDto {IPAddress = IPAddress.Parse("127.0.0.2")}
            };

            var uniqueCount = this.topIpAddressStatisticGenerator.Process(duplicates);
            uniqueCount.Message.Should().Be("The top 2 most active IP Addresses are : 127.0.0.1, 127.0.0.2");
        }
    }
}
