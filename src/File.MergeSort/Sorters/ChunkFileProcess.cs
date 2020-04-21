using System.Threading.Tasks;

namespace File.MergeSort.Sorters
{
    public sealed class ChunkFileProcess
    {
        public string ChunkFileName { get; set; }
        public string FromTo { get; set; }
        public Task Task { get; set; }
    }
}
