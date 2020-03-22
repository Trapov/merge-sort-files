using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace File.MergeSort.Sorters
{
    public sealed class DefaultSorter
    {
        private readonly DefaultSorterConfiguration _configuration;

        public DefaultSorter(DefaultSorterConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<ChunkFileProcess> Chunk(string fileName, CancellationToken token)
        {
            var results = System.IO
                .File.ReadLines(fileName)
                .Select(int.Parse)
                .OrderBy(i => i)
                .Select(i => i.ToString());

            Directory.CreateDirectory(_configuration.ChunkFilesBasePath);
            int readLines = 0;
            while (token.IsCancellationRequested == false)
            {
                var takenResults = results.Skip(readLines).Take(_configuration.LinesBuffer).ToArray();
                if (takenResults.Any() == false) break;
                var from = readLines;
                var to = readLines += _configuration.LinesBuffer;
                var chunkPath = Path.Join(_configuration.ChunkFilesBasePath, string.Format("{0}-{1}-{2}.txt", fileName.Replace(@"\", "_").Replace("/", "_"), from, to));

                yield return new ChunkFileProcess
                {
                    ChunkFileName = chunkPath,
                    FromTo = $"{from}-{to}",
                    Task = System.IO.File.WriteAllLinesAsync(chunkPath, takenResults, token)
                };
            }
        }

        public sealed class ChunkFileProcess
        {
            public string ChunkFileName { get; set; }
            public string FromTo { get; set; }
            public Task Task { get; set; }
        }

        public async Task SortChunks(CancellationToken token)
        {
            var chunks = Directory.EnumerateFiles(_configuration.ChunkFilesBasePath).Select(chunk => System.IO.File.ReadLines(chunk));
            var numberOfRuns = chunks.Count();

            var oneFileBufferLines = _configuration.LinesBuffer / numberOfRuns;

            int readLines = 0;
            while (token.IsCancellationRequested == false)
            {
                var oneRunResult = chunks
                    .SelectMany(chunkLines => chunkLines.Skip(readLines).Take(oneFileBufferLines))
                    .Select(int.Parse)
                    .OrderBy(i => i)
                    .Select(i => i.ToString());

                if (oneRunResult.Any() == false) break;

                await System.IO.File.AppendAllLinesAsync(_configuration.OutputFile, oneRunResult, token);
                readLines += oneFileBufferLines;
            }
        }

        public sealed class DefaultSorterConfiguration
        {
            public DefaultSorterConfiguration(string outputFile, string chunkFilesBasePath, int linesBuffer)
            {
                OutputFile = outputFile;
                ChunkFilesBasePath = chunkFilesBasePath;
                LinesBuffer = linesBuffer;
            }

            public string OutputFile { get; }
            public string ChunkFilesBasePath { get; }
            public int LinesBuffer { get; }
        }
    }
}
