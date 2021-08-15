using FluentAssertions;
using HttpLogStatisticsGenerator.Input;
using HttpLogStatisticsGenerator.Model;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace HttpLogStatisticsGenerator.Tests.Input
{
    [TestFixture]
    public class HttpLogTokenizerTests
    {
        private HttpLogTokenizer httpLogTokenizer;

        [SetUp]
        public void SetUp()
        {
            var logger = Mock.Of<ILogger<HttpLogTokenizer>>();
            this.httpLogTokenizer = new HttpLogTokenizer(logger);
        }

        [Test]
        public void When_the_line_doesnt_match_the_pattern_expect_nothing_returned()
        {
            var text = new List<string>
            {
                "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200]"
            };

            var result = this.httpLogTokenizer.Tokenize(text);
            result.Should().BeEmpty();
        }

        [Test]
        public void When_the_line_matches_expected_pattern_expect_raw_log()
        {
            var text = new List<string>
            {
                "168.41.191.40 - - [09/Jul/2018:10:10:38 +0200] \"GET http://example.net/blog/category/meta/ HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_7) AppleWebKit/534.24 (KHTML, like Gecko) RockMelt/0.9.58.494 Chrome/11.0.696.71 Safari/534.24\""
            };

            var result = this.httpLogTokenizer.Tokenize(text);
            result.First().Should().BeEquivalentTo(
                new RawHttpLogEntry {
                     IPAddress = "168.41.191.40",
                     DateTime = "09/Jul/2018:10:10:38 +0200",
                     Username = "-",
                     GroupName = "-",
                     Request = "GET http://example.net/blog/category/meta/ HTTP/1.1",
                     Status = "200",
                     ResponseSize  = "3574",
                     Referrer = "-",
                     UserAgentString = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_7) AppleWebKit/534.24 (KHTML, like Gecko) RockMelt/0.9.58.494 Chrome/11.0.696.71 Safari/534.24"
                });
        }
    }
}
