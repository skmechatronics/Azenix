using HttpLogStatisticsGenerator.Input;
using HttpLogStatisticsGenerator.Model;
using HttpLogStatisticsGenerator.Statistics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpLogStatisticsGenerator
{
    public class Orchestrator
    {
        private const string OutputHeader = "* HTTP Log Statistics *";

        private static readonly string HeaderDecoration = new string('*', OutputHeader.Length);

        private readonly IHttpLogReader httpLogReader;
        private readonly IHttpLogTokenizer httpLogTokenizer;
        private readonly IEnumerable<IStatisticGenerator> statisticGenerators;
        private readonly IHttpLogParser httpLogParser;
        private readonly ILogger<Orchestrator> logger;

        public Orchestrator(IHttpLogReader httpLogReader,
                        IHttpLogTokenizer httpLogTokenizer,
                        IHttpLogParser httpLogParser,
                        IEnumerable<IStatisticGenerator> statisticGenerators,
                        ILogger<Orchestrator> logger)
        {
            this.httpLogReader = httpLogReader;
            this.httpLogTokenizer = httpLogTokenizer;
            this.httpLogParser = httpLogParser;
            this.statisticGenerators = statisticGenerators;
            this.logger = logger;
        }

        public async Task Run()
        {
            var httpLogs = await this.RetrieveHttpLogs();
            if (!httpLogs.Any())
            {
                throw new AzenixException("Did not parse any valid HTTP logs. Please check the log source and try again.");
            }

            var statistics = await this.GetStatistics(httpLogs);
            this.OutputStatistics(statistics);
        }

        private void OutputStatistics(IEnumerable<StatisticResult> statistics)
        {

            var outputBuilder = new StringBuilder();
            outputBuilder.Append($"\n{ HeaderDecoration}\n{ OutputHeader}\n{ HeaderDecoration}\n\n");

            foreach(var statistic in statistics)
            {
                outputBuilder.AppendLine(statistic.Message);
            }
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(outputBuilder.ToString());
            Console.ForegroundColor = ConsoleColor.White;

            this.logger.LogInformation("Completed Run");
        }


        // This is overkill for a simple assignment but is to demonstrate 
        // the setup and structure for a more complex scenario where
        // each statistic might be generated in e.g. a separate microservice.
        private async Task<IEnumerable<StatisticResult>> GetStatistics(IEnumerable<HttpLogEntryDto> httpLogs)
        {
            this.logger.LogInformation("Generating statistics.");
            var tasks =  this.statisticGenerators.Select(
                statistic => Task.Run(() => statistic.Process(httpLogs)));

            var result = await Task.WhenAll(tasks);
            return result;
        }

        private async Task<IEnumerable<HttpLogEntryDto>> RetrieveHttpLogs()
        {
            this.logger.LogInformation("Reading logs...");
            var rawLogs = await httpLogReader.ReadHttpLogs();

            this.logger.LogInformation("Tokenizing logs...");
            var tokenizedLogs = httpLogTokenizer.Tokenize(rawLogs);

            this.logger.LogInformation("Parsing logs...");
            var parsedLogs = httpLogParser.Parse(tokenizedLogs);

            return parsedLogs;
        }
    }
}
