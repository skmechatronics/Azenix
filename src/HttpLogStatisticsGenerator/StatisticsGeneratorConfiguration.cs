namespace HttpLogStatisticsGenerator
{
    public class StatisticsGeneratorConfiguration
    {
        public string InputFileLogPath { get; set; }

        public int MostVisitedIpsToShow { get; set; } = 3;

        public int MostVisitedUrlsToShow { get; set; } = 3;
    }
}
