using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace K12.Data
{
    internal enum DateTimeField
    {
        Year,
        Month,
        Day,
        Hour,
        Minute,
        Second
    }

    internal class PatternMatcher
    {
        private static string pattern = @"^(?:(?<Year>\d{4})? *(?:/|-)? *(?<Month>\d{1,2})? *(?:/|-)? *(?<Day>\d{1,2})?)?(?: {1,}(?<Hour>\d{1,2})? *:? *(?<Minute>\d{1,2})? *:? *(?<Second>\d{1,2})?)?$";
        private static Regex parser = new Regex(pattern, RegexOptions.Singleline);

        private Match _result;

        public PatternMatcher(string input)
        {
            _result = parser.Match(input.Trim());
        }

        public bool IsSuccess
        {
            get { return _result.Success; }
        }

        public bool ContainField(DateTimeField field)
        {
            return _result.Groups[field.ToString()].Success;
        }

        public int GetFieldValue(DateTimeField field)
        {
            if (ContainField(field))
                return int.Parse(_result.Groups[field.ToString()].Value);
            else
                return -1;
        }

        public IEnumerable<Group> EachGroup()
        {
            foreach (Group each in _result.Groups)
                yield return each;
        }

        public string GetReassemblyString()
        {
            return GetFieldValue(DateTimeField.Year) + "/" +
                GetFieldValue(DateTimeField.Month) + "/" +
                GetFieldValue(DateTimeField.Day) + " " +
                GetFieldValue(DateTimeField.Hour) + ":" +
                GetFieldValue(DateTimeField.Minute) + ":" +
                GetFieldValue(DateTimeField.Second);
        }
    }
}
