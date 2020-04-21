using System;
using System.Linq;
using System.Threading.Tasks;

namespace File.Generator
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Singletones.Metrics.Stopwatch.Start();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            await Task.WhenAll(
                Singletones
                .Generator
                .GenerateFiles(Constants.DefaultFileCount, Constants.DefaultFilePostfix, Singletones.CancellationTokenSource.Token)
                .Select(process =>
                {
                    var model = new FileGeneratorModel.GeneratedFile
                    {
                        FileName = process.FileName,
                        IsDone = false,
                        NumberOfLines = process.NumberOfLines
                    };
                    process.Task.ContinueWith(_ =>
                      {
                          model.IsDone = true;
                          Singletones.UI.UpdateElapsed();
                      }, TaskScheduler.Current);

                    Singletones.UI.Model.GeneratedFiles.Add(model);
                    return process.Task;
                })
            );

            Singletones.Metrics.Stopwatch.Stop();
            Singletones.UI.UpdateElapsed();

            await Singletones.UI.RenderingLoop.RunningTask;
        }
    }
}
