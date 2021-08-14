using FluentAssertions;
using HttpLogStatisticsGenerator.Model;
using HttpLogStatisticsGenerator.Statistics;
using NUnit.Framework;
using System.Collections.Generic;

namespace HttpLogStatisticsGenerator.Tests.Statistics
{
    [TestFixture]
    public class TopUrlStatisticGeneratorTests
    {
        private TopUrlStatisticGenerator topUrlStatisticGenerator;

        [SetUp]
        public void SetUp()
        {
            this.topUrlStatisticGenerator = new TopUrlStatisticGenerator();
        }

        [Test]
        public void When_there_are_multiple_urls_hit_expect_top_urls_to_be_correct()
        {
            var httpLogs = new List<HttpLogEntryDto>
            {
                new HttpLogEntryDto {SubPath = "/foo"},
                new HttpLogEntryDto {SubPath = "/bar"},
                new HttpLogEntryDto {SubPath = "/baz"},
                new HttpLogEntryDto {SubPath = "/foo/morefoo"},
                new HttpLogEntryDto {SubPath = "/foo/morefoo"},
                new HttpLogEntryDto {SubPath = "/foo/morefoo"},
                new HttpLogEntryDto {SubPath = "/foo"},
                new HttpLogEntryDto {SubPath = "/foo"},
                new HttpLogEntryDto {SubPath = "/bar"},
                new HttpLogEntryDto {SubPath = "/foo"},
                new HttpLogEntryDto {SubPath = "/foo"},
            };

            var result = this.topUrlStatisticGenerator.Process(httpLogs);
            result.Message.Should().Be("The top 3 most visited URLs are : /foo, /foo/morefoo, /bar");
        }

        [Test]
        public void When_there_are_less_than_expected_urls_expect_top_urls_to_be_formatted_correctly()
        {
            var httpLogs = new List<HttpLogEntryDto>
            {
                new HttpLogEntryDto {SubPath = "/foo"},
                new HttpLogEntryDto {SubPath = "/bar"},
                new HttpLogEntryDto {SubPath = "/bar"}
            };

            var result = this.topUrlStatisticGenerator.Process(httpLogs);
            result.Message.Should().Be("The top 2 most visited URLs are : /bar, /foo");
        }
    }
}
