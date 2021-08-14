using HttpLogStatisticsGenerator.Input;
using HttpLogStatisticsGenerator.Statistics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace HttpLogStatisticsGenerator
{
    public class Startup
    {
        private readonly ServiceCollection services;
        private readonly IConfigurationRoot configuration;
        private ServiceProvider serviceProvider;
        private Orchestrator orchestrator;
        private ILogger<Startup> logger;

        public Startup()
        {
            this.services = new ServiceCollection();
            this.configuration = new ConfigurationBuilder()
                                .AddJsonFile("appSettings.json")
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
                .FinalizeSetup();
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
                .AddSingleton<Orchestrator>();

            return this;
        }

        private Startup FinalizeSetup()
        {
            this.serviceProvider = this.services.BuildServiceProvider();
            this.orchestrator = this.serviceProvider.GetService<Orchestrator>();
            this.logger = this.serviceProvider.GetService<ILogger<Startup>>();
            return this;
        }

        private async Task Run()
        {
            try
            {
                await this.orchestrator.Run();
            }
            catch (AzenixException azenixException)
            {
                this.logger.LogError(azenixException, "An application error occurred: {exceptionDetails}");
            }
            catch (Exception unexpectedException)
            {
                this.logger.LogError(unexpectedException, "An unexpected error occurred");
            }
        }

    }
}
