using System.Threading.Tasks;

namespace File.Generator.Generators
{
    public sealed class GenerationFileProcess
    {
        public GenerationFileProcess(string fileName, int numberOfLines, Task task)
        {
            FileName = fileName;
            NumberOfLines = numberOfLines;
            Task = task;
        }

        public string FileName { get; }
        public int NumberOfLines { get; }
        public Task Task { get; }
    }
}
