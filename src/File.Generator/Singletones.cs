using ConsoleLoop;
using File.Generator.Generators;
using System.Diagnostics;
using System.Threading;

namespace File.Generator
{
    public static class Singletones
    {
        public static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public static DefaultGenerator Generator = new DefaultGenerator(new GeneratorConfiguration(
            Constants.DefaultRangeOfInputedLines,
            Constants.DefaultFileBasePath
        ));

        public static class Metrics
        {
            public static Stopwatch Stopwatch = new Stopwatch();
        }

        public static class UI
        {
            public static SemaphoreSlim SemaphoreSlim = default;

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

            public static FileGeneratorModel Model = new FileGeneratorModel();
            public static TaskBasedRenderingLoop<FileGeneratorModel, FileGeneratorView> RenderingLoop =
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
                    .ToView<FileGeneratorView>()
                    .WithLoop<TaskBasedRenderingLoop<FileGeneratorModel, FileGeneratorView>>();
        }
    }
}
