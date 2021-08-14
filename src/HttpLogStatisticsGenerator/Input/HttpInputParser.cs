using HttpLogStatisticsGenerator.Model;
using System;
using System.Collections.Generic;
using System.Net;

namespace HttpLogStatisticsGenerator.Input
{
    /// <summary>
    /// Note that only two fields are parsed as that's all that's required
    /// But the remainder can be accommodated easily and when needed
    /// </summary>
    public class HttpInputParser : IHttpLogParser
    {
        private const int ExpectedRequestTokens = 3;

        private const string Space = " ";

        private const string HttpPrefix = "http";

        private const string RootPath = "/";

        private const char TrailingSlash = '/';

        public IEnumerable<HttpLogEntryDto> Parse(IEnumerable<RawHttpLogEntry> tokenizedLogs)
        {
            foreach(var log in tokenizedLogs)
            {
                var transformed = this.TransformRawToActual(log);
                if (transformed != null)
                {
                    yield return transformed;
                }
            }
        }

        private HttpLogEntryDto TransformRawToActual(RawHttpLogEntry rawLog)
        {
            var httpLogDto = new HttpLogEntryDto();
            var valid = true;
            valid &= ParseSubPath(httpLogDto, rawLog);
            valid &= ParseIPAddress(httpLogDto, rawLog);

            return valid ? httpLogDto : null;
        }

        private bool ParseSubPath(HttpLogEntryDto httpLogDto, RawHttpLogEntry rawLog)
        {
            var requestTokens = rawLog.Request.Split(Space);
            if (requestTokens.Length != ExpectedRequestTokens)
            {
                return false;
            }

            var rawUri = requestTokens[1];
            if (rawUri.StartsWith(HttpPrefix))
            {
                var parsedUri = new Uri(rawUri);
                httpLogDto.SubPath = parsedUri.AbsolutePath.TrimEnd(TrailingSlash);
            }
            else if (rawUri == RootPath)
            {
                httpLogDto.SubPath = rawUri;
            }
            else
            {
                httpLogDto.SubPath = rawUri.TrimEnd(TrailingSlash);
            }

            return true;
        }

        private bool ParseIPAddress(HttpLogEntryDto httpLogDto, RawHttpLogEntry rawLog)
        {
            var didParse = IPAddress.TryParse(rawLog.IPAddress, out var parsedIpAddress);
            httpLogDto.IPAddress = parsedIpAddress;
            return didParse;
        }
    }
}
