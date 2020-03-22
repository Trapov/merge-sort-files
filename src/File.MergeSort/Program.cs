using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace File.MergeSort
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Singletones.Metrics.Stopwatch.Start();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            await Task.WhenAll(
                Directory.EnumerateFiles(Constants.DefaultInputFileBasePath)
                .Select(
                    fileName => Singletones.Sorter.Chunk(fileName, Singletones.CancellationTokenSource.Token)
                ).SelectMany(f => 
                {
                    return f.Select(t =>
                    {
                        var item = new FileMergeSortModel.ChunkedFile
                        {
                            ChunkName = t.ChunkFileName,
                            FromTo = t.FromTo,
                            IsDone = false
                        };
                        t.Task.ContinueWith(_ =>
                        {
                            item.IsDone = true;
                            Singletones.UI.UpdateElapsed();
                        });
                        Singletones.UI.Model.ChunkedFiles.Add(item);
                        return t.Task;
                    });
                })
            );

            await Singletones.Sorter.SortChunks(Singletones.CancellationTokenSource.Token);
            Singletones.UI.Model.IsDone = true;

            Singletones.Metrics.Stopwatch.Stop();
            Singletones.UI.UpdateElapsed();
            Singletones.UI.RenderingLoop.Block();
        }
    }
}
