using HttpLogStatisticsGenerator.Input;
using HttpLogStatisticsGenerator.Statistics;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Threading.Tasks;

namespace HttpLogStatisticsGenerator
{
    public class Startup
    {
        public static async Task Main()
        {
            var program = new Startup();
            var services = program.Setup();
            var runner = services.GetService<Runner>();

            await runner.Run();
        }

        private ServiceProvider Setup()
        {
            var loggingConfiguration = new LoggerConfiguration()
                                        .WriteTo.Console();

            var config = new StatisticsGeneratorConfiguration
            {
                InputFileLogPath = @"D:\_codingtest\azenix\programming-task\programming-task-example-data.log"
            };

            var serviceProvider = new ServiceCollection()
                .AddLogging(logging => logging.AddSerilog(loggingConfiguration.CreateLogger()))
                .AddSingleton<StatisticsGeneratorConfiguration>(config)
                .AddSingleton<IHttpLogReader, FileHttpLogReader>()
                .AddSingleton<IHttpLogTokenizer, HttpLogTokenizer>()
                .AddSingleton<IHttpLogParser, HttpInputParser>()
                .AddSingleton<IStatisticsGenerator, UniqueIpAddressStatistic>()
                .AddSingleton<IStatisticsGenerator, TopIpAddressGenerator>()
                .AddSingleton<IStatisticsGenerator, TopUrlStatisticGenerator>()
                .AddSingleton<Runner>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
