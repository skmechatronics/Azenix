using FluentAssertions;
using HttpLogStatisticsGenerator.Input;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HttpLogStatisticsGenerator.Tests.Input
{
    [TestFixture]
    public class FileHttpLogReaderTests
    {
        private FileHttpLogReader fileHttpLogReader;

        [SetUp]
        public void SetUp()
        {
            var configOptions = Mock.Of<IOptions<StatisticsGeneratorConfiguration>>();
            Mock.Get(configOptions)
                .Setup(i => i.Value)
                .Returns(new StatisticsGeneratorConfiguration
                {
                    InputFileLogPath = @"C:\filethatdoesnotexist.log"
                });


            this.fileHttpLogReader = new FileHttpLogReader(configOptions);
        }

        [Test]
        public void When_the_file_doesnt_exist_expect_exception_to_be_thrown()
        {
            Func<Task> readTask = async () => await this.fileHttpLogReader.ReadHttpLogs();
            readTask.Should()
                    .ThrowAsync<AzenixException>()
                    .WithMessage("The file does not exist.");
        }
    }
}
