using ConsoleLoop;
using File.MergeSort.Sorters;
using System.Diagnostics;
using System.Threading;

namespace File.MergeSort
{
    public static class Singletones
    {
        public static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public static DefaultSorter Sorter => new DefaultSorter(new DefaultSorter.DefaultSorterConfiguration(
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
            public static void UpdateElapsed() => Model.TimeElapsed = Metrics.Stopwatch.Elapsed;

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
                    .Model(Model)
                    .ToView<FileMergeSortView>()
                    .WithLoop<TaskBasedRenderingLoop<FileMergeSortModel, FileMergeSortView>>();
        }
    }
}
