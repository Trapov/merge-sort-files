using System;
using System.Collections.Concurrent;

namespace File.MergeSort
{
    public sealed class FileMergeSortModel
    {
        private int _skip;
        public int Skip { get => _skip; set => _skip = value >= 0 ? (value < ChunkedFiles.Count ? value : ChunkedFiles.Count) : 0; }

        public sealed class ChunkedFile
        {
            public string ChunkName { get; set; }
            public string FromTo { get; set; }
            public bool IsDone { get; set; }
        }

        public TimeSpan TimeElapsed { get; set; } = TimeSpan.Zero;
        public ConcurrentBag<ChunkedFile> ChunkedFiles { get; } = new ConcurrentBag<ChunkedFile>();

        public bool IsDone { get; set; }
    }
}
