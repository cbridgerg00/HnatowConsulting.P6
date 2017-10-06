using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XER_Loader
{
    internal class XerActvType:XerTable
    {
        //format should be "%F	actv_code_type_id	actv_short_len	seq_num	actv_code_type	proj_id	wbs_id	actv_code_type_scope"
        public static readonly string[] CorrectHeaders = { "%F", "actv_code_type_id", "actv_short_len", "seq_num", "actv_code_type", "proj_id", "wbs_id", "actv_code_type_scope" };
        public sealed override string[] Headers { get; }
        public sealed override List<string[]> Data { get; }

        public XerActvType(string input)
        {
            if (CheckHeadersMatch(input) != ErrorCode.NoError)
            {
                throw new FormatException("Input Format Unexpected. Expected Array Length: " + CorrectHeaders.Length + " -- Received Array Length: " + input.Length);
            }
            var substrings = input.Split('\t');
            Headers = new string[substrings.Length - 1];
            for (var index = 0; index < Headers.Length; index++)
            {
                Headers[index] = substrings[index + 1];
            }
            Data = new List<string[]>();
        }

        public override ErrorCode AddData(string input)
        {
            var substrings = input.Split('\t');
            var newData = new string[substrings.Length - 1];
            var regularExpressions = new Regex[substrings.Length - 1];


            //substring.Length should equal Headers.Length
            if (substrings.Length != Headers.Length)
            {
                return ErrorCode.ArrayLengthMismatch;
            }

            //substrings[0] should be "%R"
            if (!substrings[0].Equals("%R"))
            {
                return ErrorCode.UnexpectedRowIdentifier;
            }

            regularExpressions[0] = new Regex("\\d+");  //actv_code_type_id
            regularExpressions[1] = new Regex("\\d+");  //actv_short_len
            regularExpressions[2] = new Regex("\\d+");  //seq_num
            regularExpressions[3] = new Regex(".*");    //actv_code_type
            regularExpressions[4] = new Regex("\\d*");  //proj_id
            regularExpressions[5] = new Regex("\\d*");  //wbs_id
            regularExpressions[6] = new Regex(".*");    //actv_code_type_scope

            for (var index = 0; index < regularExpressions.Length; index++)
            {
                if (regularExpressions[index].IsMatch(substrings[index + 1]))
                {
                    newData[index] = substrings[index + 1];
                }
                else
                {
                    return ErrorCode.FailedRegExTest;
                }
            }

            Data.Add(newData);

            return ErrorCode.NoError;
        }

        public static ErrorCode CheckHeadersMatch(string input)
        {
            var substrings = input.Split('\t');
            if (substrings.Length != 8)
            {
                return ErrorCode.ArrayLengthMismatch;
            }

            for (var index = 0; index < substrings.Length; index++)
            {
                if (!substrings[index].Equals(CorrectHeaders[index]))
                {
                    return ErrorCode.FailedHeaderCheck;
                }
            }

            return ErrorCode.NoError;
        }

        public static int GetCorrectDataLength()
        {
            return CorrectHeaders.Length;
        }
    }
}
