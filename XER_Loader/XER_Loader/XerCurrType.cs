using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XER_Loader
{
    internal class XerCurrType : XerTable
    {
        //format should be "%F	curr_id	decimal_digit_cnt	curr_symbol	decimal_symbol	digit_group_symbol	pos_curr_fmt_type	neg_curr_fmt_type	curr_type	curr_short_name	group_digit_cnt	base_exch_rate"
        public static readonly string[] CorrectHeaders = { "%F", "curr_id", "decimal_digit_cnt", "curr_symbol", "decimal_symbol", "digit_group_symbol", "pos_curr_fmt_type", "neg_curr_fmt_type", "curr_type", "curr_short_name", "group_digit_cnt", "base_exch_rate" };
        public sealed override string[] Headers { get; }
        public sealed override List<string[]> Data { get; }

        public XerCurrType(string input)
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

            //curr_id
            var regex = new Regex("\\d+");
            if (regex.IsMatch(substrings[1]))
            {
                newData[0] = substrings[1];
            }
            else
            {
                return ErrorCode.FailedRegExTest;
            }

            //decimal_digit_cnt
            regex = new Regex("\\d+");
            if (regex.IsMatch(substrings[2]))
            {
                newData[1] = substrings[2];
            }
            else
            {
                return ErrorCode.FailedRegExTest;
            }

            //curr_symbol
            regex = new Regex(".+");
            if (regex.IsMatch(substrings[3]))
            {
                newData[2] = substrings[3];
            }
            else
            {
                return ErrorCode.FailedRegExTest;
            }

            //decimal_symbol
            regex = new Regex(".+");
            if (regex.IsMatch(substrings[4]))
            {
                newData[3] = substrings[4];
            }
            else
            {
                return ErrorCode.FailedRegExTest;
            }

            //digit_group_symbol
            regex = new Regex(".+");
            if (regex.IsMatch(substrings[5]))
            {
                newData[4] = substrings[5];
            }
            else
            {
                return ErrorCode.FailedRegExTest;
            }

            //pos_curr_fmt_type
            regex = new Regex(".+");
            if (regex.IsMatch(substrings[6]))
            {
                newData[5] = substrings[6];
            }
            else
            {
                return ErrorCode.FailedRegExTest;
            }

            //neg_curr_fmt_type
            regex = new Regex(".+");
            if (regex.IsMatch(substrings[7]))
            {
                newData[6] = substrings[7];
            }
            else
            {
                return ErrorCode.FailedRegExTest;
            }

            //curr_type
            regex = new Regex(".*");
            if (regex.IsMatch(substrings[8]))
            {
                newData[7] = substrings[8];
            }
            else
            {
                return ErrorCode.FailedRegExTest;
            }

            //curr_short_name
            regex = new Regex(".{3,4}");
            if (regex.IsMatch(substrings[9]))
            {
                newData[8] = substrings[9];
            }
            else
            {
                return ErrorCode.FailedRegExTest;
            }

            //group_digit_cnt
            regex = new Regex("\\d+");
            if (regex.IsMatch(substrings[10]))
            {
                newData[9] = substrings[10];
            }
            else
            {
                return ErrorCode.FailedRegExTest;
            }

            //base_exch_rate
            regex = new Regex("\\d*(\\.\\d*)?");
            if (regex.IsMatch(substrings[11]))
            {
                newData[10] = substrings[11];
            }
            else
            {
                return ErrorCode.FailedRegExTest;
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
