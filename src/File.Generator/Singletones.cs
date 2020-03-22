using ConsoleLoop;
using File.Generator.Generators;
using System.Diagnostics;
using System.Threading;

namespace File.Generator
{
    public static class Singletones
    {
        public static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public static DefaultGenerator Generator => new DefaultGenerator(new DefaultGenerator.GeneratorConfiguration(
            Constants.DefaultRangeOfInputedLines,
            Constants.DefaultFileBasePath
        ));

        public static class Metrics
        {
            public static Stopwatch Stopwatch = new Stopwatch();
        }

        public static class UI
        {
            public static void UpdateElapsed() => Model.TimeElapsed = Metrics.Stopwatch.Elapsed;

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
                    .Model(Model)
                    .ToView<FileGeneratorView>()
                    .WithLoop<TaskBasedRenderingLoop<FileGeneratorModel, FileGeneratorView>>();
        }
    }
}
