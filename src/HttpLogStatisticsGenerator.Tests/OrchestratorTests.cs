using FluentAssertions;
using HttpLogStatisticsGenerator.Input;
using HttpLogStatisticsGenerator.Model;
using HttpLogStatisticsGenerator.Statistics;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HttpLogStatisticsGenerator.Tests
{
    [TestFixture]
    public class OrchestratorTests
    {
        private Orchestrator orchestrator;

        public OrchestratorTests()
        {
            var mockReader = Mock.Of<IHttpLogReader>();
            var mockTokenizer = Mock.Of<IHttpLogTokenizer>();
            var mockParser = Mock.Of<IHttpLogParser>();
            var generators = Mock.Of<IEnumerable<IStatisticGenerator>>();
            var logger = Mock.Of<ILogger<Orchestrator>>();

            Mock.Get(mockParser).Setup(i => i.Parse(It.IsAny<IEnumerable<RawHttpLogEntry>>()))
                .Returns(new List<HttpLogEntryDto>());

            this.orchestrator = new Orchestrator(
                                        mockReader,
                                        mockTokenizer,
                                        mockParser,
                                        generators,
                                        logger);
        }

        [Test]
        public async Task When_the_parser_returns_empty_logs_then_an_exception_should_be_thrown()
        {
            Func<Task> orchestratorAction = async () => await this.orchestrator.Run();

            await orchestratorAction.Should().ThrowAsync<AzenixException>()
                        .WithMessage("Did not parse any valid HTTP logs. Please check the log source and try again.");
        }
    }
}
