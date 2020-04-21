using ConsoleLoop;
using System;
using System.Linq;

namespace File.Generator
{
    public sealed class FileGeneratorView : IView<FileGeneratorModel>
    {
        public static int FileNameLength = " FileName ".Length;
        public static int NumberOfLinesLength = " NumberOfLines ".Length;

        public static string RenderRow(FileGeneratorModel.GeneratedFile generatedFile)
        {
            var fileNameEntry = string.Join("", generatedFile.FileName.PadRight(FileNameLength).Take(FileNameLength));
            var numberOfLinesEntry = string.Join("", generatedFile.NumberOfLines.ToString().PadRight(NumberOfLinesLength).Take(NumberOfLinesLength - 3));

            var stateEntry = generatedFile.IsDone ? "Done".InGreen() : "Writing".InYellow();

            return $" {fileNameEntry} {numberOfLinesEntry} {stateEntry}";
        }
        public static string RenderTime(TimeSpan timeElapsed) =>
            $"Time ==> {timeElapsed.TotalSeconds} seconds  \n";
        public static string RenderHeader() =>
            $" FileName | NumberOfLines | State \n";

        public string Render(FileGeneratorModel model)
        {
            return
                RenderHeader() +
                "".PadRight(70, '-') + "\n" +
                "    ".PadRight(10, '.') + "\n" +
                string.Join("\n",
                    model
                        .GeneratedFiles
                        .OrderBy(f => f.NumberOfLines)
                        .Skip(model.Skip)
                        .Take(10)
                        .Select(RenderRow)
                ) +
                "\n" + "    ".PadRight(10, '.') + "\n" +
                "".PadRight(70, '-') + "\n" +
                $" ↓ ↑ To Navigate | Done {model.GeneratedFiles.Count(f => f.IsDone)} out of {model.GeneratedFiles.Count} | " + RenderTime(model.TimeElapsed) +
                "".PadRight(70, '-') + "\n";
        }
    }
}
