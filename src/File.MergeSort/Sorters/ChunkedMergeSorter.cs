using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace File.MergeSort.Sorters
{
    public sealed class ChunkedMergeSorter
    {
        private readonly ChunkedMergeSorterConfiguration _configuration;

        public ChunkedMergeSorter(ChunkedMergeSorterConfiguration configuration)
        {
            _configuration = configuration;
            if (Directory.Exists(_configuration.ChunkFilesBasePath))
                Directory.Delete(_configuration.ChunkFilesBasePath, true);

            Directory.CreateDirectory(_configuration.ChunkFilesBasePath);
        }

        public IEnumerable<ChunkFileProcess> Chunk(string fileName, CancellationToken token)
        {
            var results = System.IO.File.ReadLines(fileName).Select(int.Parse);

            int readLines = 0;
            while (token.IsCancellationRequested == false)
            {
                var takenResults =
                    results
                        .Skip(readLines)
                        .Take(_configuration.LinesBuffer)
                        .OrderBy(i => i)
                        .Select(i => i.ToString(CultureInfo.InvariantCulture))
                    .ToArray();

                if (takenResults.Any() == false) break;
                var from = readLines;
                var to = readLines += _configuration.LinesBuffer;

                var chunkPath = Path.Join(
                    _configuration.ChunkFilesBasePath,
                    $@"{fileName
                        .Replace(@"\", "_", System.StringComparison.InvariantCultureIgnoreCase)
                        .Replace("/", "_", System.StringComparison.InvariantCultureIgnoreCase)}-{from}-{to}.txt"
                );

                yield return new ChunkFileProcess
                {
                    ChunkFileName = chunkPath,
                    FromTo = $"{from}-{to}",
                    Task = System.IO.File.WriteAllLinesAsync(chunkPath, takenResults, token)
                };
            }
        }

        public async Task SortChunks(CancellationToken token)
        {
            var chunks = Directory.EnumerateFiles(_configuration.ChunkFilesBasePath).Select(chunk => System.IO.File.ReadLines(chunk));
            var numberOfRuns = chunks.Count();

            var oneFileBufferLines = _configuration.LinesBuffer / numberOfRuns;

            int readLines = 0;
            while (token.IsCancellationRequested == false)
            {
                var oneRunResult =
                    chunks
                        .SelectMany(chunkLines => chunkLines.Skip(readLines).Take(oneFileBufferLines))
                        .Select(int.Parse)
                        .OrderBy(i => i)
                        .Select(i => i.ToString(CultureInfo.InvariantCulture))
                    .ToArray();

                if (oneRunResult.Any() == false) break;

                await System.IO.File.AppendAllLinesAsync(_configuration.OutputFile, oneRunResult, token);
                readLines += oneFileBufferLines;
            }
        }
    }
}
