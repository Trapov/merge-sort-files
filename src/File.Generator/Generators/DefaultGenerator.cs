using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace File.Generator.Generators
{
    public sealed class DefaultGenerator
    {
        private readonly GeneratorConfiguration _configuration;

        public DefaultGenerator(GeneratorConfiguration configuration)
        {
            _configuration = configuration;

            if(Directory.Exists(_configuration.FilePathBase))
                Directory.Delete(_configuration.FilePathBase, true);

            Directory.CreateDirectory(_configuration.FilePathBase);
        }

        public IEnumerable<GenerationFileProcess> GenerateFiles(ushort fileCount, string filePostfix, CancellationToken cancellationToken)
        {
            return Enumerable
                    .Range(0, fileCount)
                    .Select(n => GenerateFile(n, filePostfix, cancellationToken));
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
        private IEnumerable<string> RandomizeLines(int maxNumberOfLines) => Enumerable.Range(0, maxNumberOfLines).Select(_ => new Random().Next().ToString(CultureInfo.InvariantCulture));
        private int RandomizeMaxNumberOfLines() => new Random().Next(_configuration.NumberOfLinesRange.Start.Value, _configuration.NumberOfLinesRange.End.Value);

    }
}
