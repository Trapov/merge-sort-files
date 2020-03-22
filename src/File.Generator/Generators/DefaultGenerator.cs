using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace File.Generator.Generators
{
    public sealed class DefaultGenerator
    {
        private readonly GeneratorConfiguration _configuration;

        public DefaultGenerator(GeneratorConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<GenerationFileProcess> GenerateFiles(ushort fileCount, string filePostfix, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(_configuration.FilePathBase);
            return Enumerable
                    .Range(0, fileCount)
                    .Select(n => GenerateFile(n, filePostfix, cancellationToken));
        }

        public sealed class GenerationFileProcess
        {
            public GenerationFileProcess(string fileName, int numberOfLines, Task task)
            {
                FileName = fileName;
                NumberOfLines = numberOfLines;
                Task = task;
            }

            public string FileName { get; }
            public int NumberOfLines { get; }
            public Task Task { get; }
        }
        public sealed class GeneratorConfiguration
        {
            public GeneratorConfiguration(Range numberOfLines, string filePathBase)
            {
                NumberOfLinesRange = numberOfLines;
                FilePathBase = filePathBase;
            }

            public string FilePathBase { get; }
            public Range NumberOfLinesRange { get; }
        }

        private GenerationFileProcess GenerateFile(int fileNumber, string filePostfix, CancellationToken cancellationToken)
        {
            var numberOfLines = RandomizeMaxNumberOfLines();
            var lines = RandomizeLines(numberOfLines);
            var fileName = string.Concat(fileNumber, filePostfix);
            var path = Path.Join(_configuration.FilePathBase, fileName);
            var task = System.IO.File.WriteAllLinesAsync(path, lines, cancellationToken);
            return new GenerationFileProcess(fileName, numberOfLines, task);
        }
        private IEnumerable<string> RandomizeLines(int maxNumberOfLines) => Enumerable.Range(0, maxNumberOfLines).Select(_ => new Random().Next().ToString());
        private int RandomizeMaxNumberOfLines() => new Random().Next(_configuration.NumberOfLinesRange.Start.Value, _configuration.NumberOfLinesRange.End.Value);

    }
}
