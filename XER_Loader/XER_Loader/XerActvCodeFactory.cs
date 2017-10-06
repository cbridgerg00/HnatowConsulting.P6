using System;

namespace XER_Loader
{
    internal class XerActvCodeFactory:XerTableFactory
    {
        private ErrorCode _lastErrorCode;
        public override ErrorCode LastErrorCode => _lastErrorCode;

        public override XerTable GetXerTable(string input)
        {
            _lastErrorCode = XerActvCode.CheckHeadersMatch(input);
            if (_lastErrorCode != ErrorCode.NoError)
            {
                return null;
            }

            XerTable newTable;
            try
            {
                newTable = new XerActvCode(input);
            }
            catch (FormatException e)
            {
                //Headers didn't match
                if (e.Message.StartsWith("Input Format Unexpected."))
                {
                    _lastErrorCode = ErrorCode.FailedHeaderCheck;
                    newTable = null;
                }
                else
                {
                    throw;
                }
                
            }
            return newTable;
        }
    }
}
