using System;

namespace File.Generator.Generators
{
    public sealed class GeneratorConfiguration
    {
        public GeneratorConfiguration(Range numberOfLines, string filePathBase)
        {
            NumberOfLinesRange = numberOfLines;
            FilePathBase = filePathBase;
        }

        public string FilePathBase { get; }
        public Range NumberOfLinesRange { get; }
    }
}
