using ConsoleLoop;
using System;
using System.Linq;

namespace File.MergeSort
{
    public sealed class FileMergeSortView : IView<FileMergeSortModel>
    {
        public static int ChunkNameLength = " ChunkName ".Length;
        public static int FromToLength = "   From-To   ".Length;

        public static string RenderRow(FileMergeSortModel.ChunkedFile chunkedFile)
        {
            var fileNameEntry = string.Join("", chunkedFile.ChunkName.PadRight(ChunkNameLength).Take(ChunkNameLength));
            var numberOfLinesEntry = string.Join("", chunkedFile.FromTo.PadRight(FromToLength).Take(FromToLength - 3));

            var stateEntry = chunkedFile.IsDone ? "Done".InGreen() : "Writing".InYellow();

            return $"{fileNameEntry} {numberOfLinesEntry} {stateEntry}";
        }

        public static string RenderTime(TimeSpan timeElapsed) =>
            $" Time ==> {timeElapsed.TotalSeconds} seconds  \n";
        public static string RenderHeader() =>
            $" ChunkName |   From-To   | State \n";

        public string Render(FileMergeSortModel model)
        {
            return
                RenderHeader() +
                "".PadRight(70, '-') + "\n" +
                "    ".PadRight(10, '.') + "\n" +
                string.Join("\n",
                    model
                        .ChunkedFiles
                        .OrderBy(f => f.FromTo)
                        .Skip(model.Skip)
                        .Take(10)
                        .Select(RenderRow)
                ) + "\n" +
                "    ".PadRight(10, '.') + "\n" +
                "".PadRight(70, '-') + "\n" +
                $" ↓ ↑ To Navigate | Done {model.ChunkedFiles.Count(f => f.IsDone)} out of {model.ChunkedFiles.Count} | " + RenderTime(model.TimeElapsed) +
                "".PadRight(70, '-') + "\n" +
                "State => " + (model.IsDone ? "Sorted".InGreen() : "Sorting".InYellow()) + "\n" +
                "".PadRight(70, '-') + "\n";
        }
    }
}
