using HttpLogStatisticsGenerator.Input;
using HttpLogStatisticsGenerator.Statistics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Threading.Tasks;

namespace HttpLogStatisticsGenerator
{
    public class Startup
    {
        private readonly ServiceCollection services;
        private readonly IConfigurationRoot configuration;
        private ServiceProvider serviceProvider;

        public Startup()
        {
            this.services = new ServiceCollection();
            this.configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .Build();
        }

        public static async Task Main()
        {
            var program = new Startup();
            program.Setup();
            await program.Run();            
        }

        private void Setup()
        {
            this.AddConfiguration()
                .AddLogging()
                .AddInputProcessors()
                .AddGenerators()
                .BuildServiceProvider();
        }

        private Startup AddConfiguration()
        {
            var statisticsConfiguration = this.configuration.GetSection(nameof(StatisticsGeneratorConfiguration));
            this.services.Configure<StatisticsGeneratorConfiguration>(statisticsConfiguration);
            return this;
        }

        private Startup AddLogging()
        {
            var loggingConfiguration = new LoggerConfiguration()
                                        .WriteTo.Console();
            services
               .AddLogging(logging => logging.AddSerilog(loggingConfiguration.CreateLogger()));

            return this;
        }

        private Startup AddInputProcessors()
        {
            this.services
                .AddSingleton<IHttpLogReader, FileHttpLogReader>()
                .AddSingleton<IHttpLogTokenizer, HttpLogTokenizer>()
                .AddSingleton<IHttpLogParser, HttpInputParser>();

            return this;
        }

        private Startup AddGenerators()
        {
            this.services
                .AddSingleton<IStatisticsGenerator, UniqueIpAddressStatistic>()
                .AddSingleton<IStatisticsGenerator, TopIpAddressGenerator>()
                .AddSingleton<IStatisticsGenerator, TopUrlStatisticGenerator>()
                .AddSingleton<Runner>();

            return this;
        }

        private Startup BuildServiceProvider()
        {
            this.serviceProvider = this.services.BuildServiceProvider();
            return this;
        }

        private async Task Run()
        {
            var runner = this.serviceProvider.GetService<Runner>();
            await runner.Run();
        }

    }
}
