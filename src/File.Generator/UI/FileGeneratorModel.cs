using System;
using System.Collections.Concurrent;

namespace File.Generator
{
    public sealed class FileGeneratorModel
    {
        private int _skip;
        public int Skip { get => _skip; set => _skip = value >= 0 ? (value < GeneratedFiles.Count ? value : GeneratedFiles.Count) : 0; }

        public sealed class GeneratedFile
        {
            public string FileName { get; set; }
            public bool IsDone { get; set; }
            public int NumberOfLines { get; set; }
        }

        public TimeSpan TimeElapsed { get; set; } = TimeSpan.Zero;
        public ConcurrentBag<GeneratedFile> GeneratedFiles { get; } = new ConcurrentBag<GeneratedFile>();
    }
}
