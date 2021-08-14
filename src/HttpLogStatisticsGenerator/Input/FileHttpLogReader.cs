using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HttpLogStatisticsGenerator.Input
{
    public class FileHttpLogReader : IHttpLogReader
    {
        private readonly StatisticsGeneratorConfiguration configuration;

        public FileHttpLogReader(IOptions<StatisticsGeneratorConfiguration> configuration)
        {
            this.configuration = configuration.Value;
        }

        public async Task<IEnumerable<string>> ReadHttpLogs()
        {
            if (!File.Exists(this.configuration.InputFileLogPath))
            {
                throw new AzenixException("The file does not exist.");
            }

            var lines = await File.ReadAllLinesAsync(this.configuration.InputFileLogPath);
            return lines.AsEnumerable();
        }
    }
}
