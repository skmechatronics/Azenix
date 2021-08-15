using HttpLogStatisticsGenerator.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HttpLogStatisticsGenerator.Input
{
    public class HttpLogTokenizer : IHttpLogTokenizer
    {
        private const int ExpectedTokens = 9;

        private const string SpacedToken = "([^\\s]+)";

        private const string BracketedToken = "\\[(.*)\\]";

        private const string QuotedToken = "\"([^\"]+)\"";

        // IP Address
        // Group
        // Username
        // DateTime
        // Request - Method, URI, HTTP
        // Status
        // Size
        // Referer
        // UserAgent
        private static readonly Regex HttpLogPattern
            = new Regex($"{SpacedToken} {SpacedToken} {SpacedToken} {BracketedToken} {QuotedToken} {SpacedToken} {SpacedToken} {QuotedToken} {QuotedToken}");
        
        private readonly ILogger<HttpLogTokenizer> logger;

        public HttpLogTokenizer(ILogger<HttpLogTokenizer> logger)
        {
            this.logger = logger;
        }

        public IEnumerable<RawHttpLogEntry> Tokenize(IEnumerable<string> textLogs)
        {
            var tokenizedLogs = new List<RawHttpLogEntry>();
            foreach(var log in textLogs)
            {
                var tokenized = HttpLogPattern.Match(log);

                // The first token is the match itself which is to be ignored
                if (tokenized.Groups.Count == ExpectedTokens + 1)
                {
                    tokenizedLogs.Add(ConvertTextToLogEntry(tokenized.Groups));
                }
                else
                {
                    this.logger.LogError("Unexpected number of tokens");
                }
            }

            return tokenizedLogs;
        }

        private RawHttpLogEntry ConvertTextToLogEntry(GroupCollection tokenGroups)
        {
            var enumerated = tokenGroups.Values.ToList();

            return new RawHttpLogEntry
            {
                // The first entry is the match itself which is not required
                IPAddress = enumerated[1].Value,
                GroupName = enumerated[2].Value,
                Username = enumerated[3].Value,
                DateTime = enumerated[4].Value,
                Request = enumerated[5].Value,
                Status = enumerated[6].Value,
                ResponseSize = enumerated[7].Value,
                Referrer = enumerated[8].Value,
                UserAgentString = enumerated[9].Value
            };
        }
    }
}
