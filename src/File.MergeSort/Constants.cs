namespace File.MergeSort
{
    public static class Constants
    {
        public const string DefaultInputFileBasePath = "../../../../../generated-payloads";
        public const string DefaultChunkFileBasePath = "../../../../../chunks";
        public const int DefaultLinesBuffer = 1000_0000/2; //todo: change to bytes
        public const string DefaultOutputFile = "../../../../../sorted.txt";
    }
}
