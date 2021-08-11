using HttpLogStatisticsGenerator.Input;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Threading.Tasks;

namespace HttpLogStatisticsGenerator
{
    class Program
    {

        public static async Task Main()
        {
            var program = new Program();
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
                .AddSingleton<IHttpInputParser, HttpInputParser>()
                .AddSingleton<Runner>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
