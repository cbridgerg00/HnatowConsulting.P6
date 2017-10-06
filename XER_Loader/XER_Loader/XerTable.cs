using System.Collections.Generic;

namespace XER_Loader
{
    internal abstract class XerTable
    {
        public abstract string[] Headers { get; }
        public abstract List<string[]> Data { get; }

        public abstract ErrorCode AddData(string input);
    }
}
