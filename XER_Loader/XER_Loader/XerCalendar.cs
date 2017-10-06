using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XER_Loader
{
    internal class XerCalendar:XerTable
    {
        //format should be "%F	clndr_id	default_flag	clndr_name	proj_id	base_clndr_id	last_chng_date	clndr_type	day_hr_cnt	week_hr_cnt	month_hr_cnt	year_hr_cnt	rsrc_private	clndr_data"
        public static readonly string[] CorrectHeaders = { "%F", "clndr_id", "default_flag", "clndr_name", "proj_id", "base_clndr_id", "last_chng_date", "clndr_type", "day_hr_cnt", "week_hr_cnt", "month_hr_cnt", "year_hr_cnt", "rsrc_private", "clndr_data" };
        public sealed override string[] Headers { get; }
        public sealed override List<string[]> Data { get; }

        public XerCalendar(string input)
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

            regularExpressions[0] = new Regex("\\d+");                                  //clndr_id
            regularExpressions[1] = new Regex("[YN]");                                  //default_flag
            regularExpressions[2] = new Regex(".*");                                    //clndr_name
            regularExpressions[3] = new Regex("\\d*");                                  //proj_id
            regularExpressions[4] = new Regex("\\d*");                                  //base_clndr_id
            regularExpressions[5] = new Regex("\\d{4}-\\d{2}-\\d{2}\\s*\\d{2}:\\d{2}"); //last_chng_date
            regularExpressions[6] = new Regex(".*");                                    //clndr_type
            regularExpressions[7] = new Regex("\\d*");                                  //day_hr_cnt
            regularExpressions[8] = new Regex("\\d*");                                  //week_hr_cnt
            regularExpressions[9] = new Regex("\\d*");                                  //month_hr_cnt
            regularExpressions[10] = new Regex("\\d*");                                 //year_hr_cnt
            regularExpressions[11] = new Regex("[YN]");                                 //rsrc_private
            regularExpressions[12] = new Regex(".*");                                 //clndr_data

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
