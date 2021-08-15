using FluentAssertions;
using HttpLogStatisticsGenerator.Input;
using HttpLogStatisticsGenerator.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HttpLogStatisticsGenerator.Tests.Input
{
    [TestFixture]
    public class HttpLogParserTests
    {
        private HttpLogParser httpLogParser;

        [SetUp]
        public void SetUp()
        {
            this.httpLogParser = new HttpLogParser();
        }

        [Test]
        public void When_the_ip_address_cannot_be_parsed_expect_empty_result()
        {
            var rawHttpLogs = new List<RawHttpLogEntry>{
                new RawHttpLogEntry()
                {
                    IPAddress = "192.",
                    Request = "GET http://example.net/blog/category/meta/ HTTP/1.1"
                } 
            };

            var result = this.httpLogParser.Parse(rawHttpLogs);
            result.Should().BeEmpty();
        }

        [Test]
        public void When_the_ip_address_is_valid_expect_it_to_be_parsed()
        {
            var rawHttpLogs = new List<RawHttpLogEntry>{
                new RawHttpLogEntry()
                {
                    IPAddress = "127.0.0.1",
                    Request = "GET http://example.net/blog/category/meta/ HTTP/1.1"
                }
            };

            var result = this.httpLogParser.Parse(rawHttpLogs);
            result.First().IPAddress.ToString().Should().Be("127.0.0.1");
        }

        [Test]
        public void When_the_ip_address_is_invalid_expect_empty_result()
        {
            var rawHttpLogs = new List<RawHttpLogEntry>{
                new RawHttpLogEntry()
                {
                    IPAddress = "500.0.0.1",
                    Request = "GET http://example.net/blog/category/meta/ HTTP/1.1"
                }
            };

            var result = this.httpLogParser.Parse(rawHttpLogs);
            result.Should().BeEmpty();
        }

        [Test]
        public void When_the_url_starts_with_http_expect_subpath_retrieved_without_extra_slash()
        {
            var rawHttpLogs = new List<RawHttpLogEntry>{
                new RawHttpLogEntry()
                {
                    IPAddress = "127.0.0.1",
                    Request = "GET http://example.net/blog/category/meta/ HTTP/1.1"
                }
            };

            var result = this.httpLogParser.Parse(rawHttpLogs);
            result.First().SubPath.Should().BeEquivalentTo("/blog/category/meta");
        }

        [Test]
        public void When_the_url_is_just_the_root_then_expect_that_to_be_retained()
        {
            var rawHttpLogs = new List<RawHttpLogEntry>{
                new RawHttpLogEntry()
                {
                    IPAddress = "127.0.0.1",
                    Request = "GET / HTTP/1.1"
                }
            };

            var result = this.httpLogParser.Parse(rawHttpLogs);
            result.First().SubPath.Should().BeEquivalentTo("/");
        }

        [Test]
        public void When_the_url_is_a_sub_path_expect_subpath_to_be_retrieved()
        {
            var rawHttpLogs = new List<RawHttpLogEntry>{
                new RawHttpLogEntry()
                {
                    IPAddress = "127.0.0.1",
                    Request = "GET /foo HTTP/1.1"
                }
            };

            var result = this.httpLogParser.Parse(rawHttpLogs);
            result.First().SubPath.Should().BeEquivalentTo("/foo");
        }

        [Test]
        public void When_the_url_is_a_sub_path_with_slash_expect_subpath_to_be_retrieved_without_slash()
        {
            var rawHttpLogs = new List<RawHttpLogEntry>{
                new RawHttpLogEntry()
                {
                    IPAddress = "127.0.0.1",
                    Request = "GET /foo/ HTTP/1.1"
                }
            };

            var result = this.httpLogParser.Parse(rawHttpLogs);
            result.First().SubPath.Should().BeEquivalentTo("/foo");
        }
    }
}
