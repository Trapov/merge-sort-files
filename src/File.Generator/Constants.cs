using System;

namespace File.Generator
{
    public static class Constants
    {
        public const string DefaultFilePostfix = "-generated.txt";
        public const string DefaultFileBasePath = "../../../../../generated-payloads";
        public const ushort DefaultFileCount = 20;

        public static Range DefaultRangeOfInputedLines = 1000..10000;
    }
}
