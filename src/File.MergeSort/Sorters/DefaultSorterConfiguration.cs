namespace File.MergeSort.Sorters
{
    public sealed class ChunkedMergeSorterConfiguration
    {
        public ChunkedMergeSorterConfiguration(string outputFile, string chunkFilesBasePath, int linesBuffer)
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
