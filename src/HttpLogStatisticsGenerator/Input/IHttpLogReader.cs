using System.Collections.Generic;
using System.Threading.Tasks;

namespace HttpLogStatisticsGenerator.Input
{
    public interface IHttpLogReader
    {
        Task<IEnumerable<string>> ReadHttpLogs();
    }
}
