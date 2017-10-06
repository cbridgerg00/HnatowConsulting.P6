namespace XER_Loader
{
    public enum ErrorCode { NoError, ArrayLengthMismatch, UnexpectedRowIdentifier, FailedRegExTest, FailedHeaderCheck };
    internal abstract class XerTableFactory
    {
        public abstract ErrorCode LastErrorCode { get; }
        public abstract XerTable GetXerTable(string input);
    }
}
