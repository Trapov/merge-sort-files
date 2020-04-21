using ConsoleLoop;
using File.MergeSort.Sorters;
using System.Diagnostics;
using System.Threading;

namespace File.MergeSort
{
    public static class Singletones
    {
        public static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public static ChunkedMergeSorter Sorter = new ChunkedMergeSorter(new ChunkedMergeSorterConfiguration(
            Constants.DefaultOutputFile,
            Constants.DefaultChunkFileBasePath,
            Constants.DefaultLinesBuffer
        ));

        public static class Metrics
        {
            public static Stopwatch Stopwatch = new Stopwatch();
        }

        public static class UI
        {
            public static void UpdateElapsed()
            {
                try
                {
                    //not atomic so have to lock
                    SemaphoreSlim.Wait();
                    Model.TimeElapsed = Metrics.Stopwatch.Elapsed;
                }
                finally
                {
                    SemaphoreSlim.Release();
                }
            }

            public static SemaphoreSlim SemaphoreSlim = default;

            public static FileMergeSortModel Model = new FileMergeSortModel();
            public static TaskBasedRenderingLoop<FileMergeSortModel, FileMergeSortView> RenderingLoop =
                new ConsoleLoopBuilder(cancellationToken: CancellationTokenSource.Token)
                    .WithInputEventHandler(keyInfo => keyInfo.Key == System.ConsoleKey.UpArrow, _ =>
                    {
                        Model.Skip -= 10;
                    })
                    .WithInputEventHandler(keyInfo => keyInfo.Key == System.ConsoleKey.DownArrow, _ =>
                    {
                        Model.Skip += 10;
                    })
                    .Model(Model, out SemaphoreSlim)
                    .ToView<FileMergeSortView>()
                    .WithLoop<TaskBasedRenderingLoop<FileMergeSortModel, FileMergeSortView>>();
        }
    }
}
