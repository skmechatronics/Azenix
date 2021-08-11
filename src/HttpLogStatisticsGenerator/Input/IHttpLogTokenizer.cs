using HttpLogStatisticsGenerator.Model;
using System.Collections.Generic;

namespace HttpLogStatisticsGenerator.Input
{
    public interface IHttpLogTokenizer
    {
        IEnumerable<RawHttpLogEntry> Tokenize(IEnumerable<string> rawLogs);
    }
}
