using HttpLogStatisticsGenerator.Input;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HttpLogStatisticsGenerator
{
    public class Runner
    {
        private readonly IHttpLogReader httpLogReader;
        private readonly IHttpLogTokenizer httpLogTokenizer;
        private readonly IHttpInputParser httpLogParser;
        private ILogger<Runner> logger;

        public Runner(IHttpLogReader reader,
                        IHttpLogTokenizer tokenizer, 
                        ILogger<Runner> logger)
        {
            this.httpLogReader = reader;
            this.httpLogTokenizer = tokenizer;
            this.logger = logger;
        }

        public async Task Run()
        {
            this.logger.LogInformation("Reading logs...");
            var rawLogs = await httpLogReader.ReadHttpLogs();

            this.logger.LogInformation("Tokenizing logs...");
            var tokenizedLogs = httpLogTokenizer.Tokenize(rawLogs);

            this.logger.LogInformation("Parsing logs...");
            var parsedLogs = httpLogParser.Parse(tokenizedLogs);



            this.logger.LogInformation("Completed");
        }
    }
}
