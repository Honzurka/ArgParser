using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgParser
{
    internal class ParsableInt
    {
        int minValue;
        int maxValue;
        int? defaultValue;
        int[] parsedValues;

        public ParsableInt(int minValue, int maxValue, int? defaultValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.defaultValue = defaultValue;
        }

        private int ParseSingle(string opt)
        {
            if (!int.TryParse(opt, out var value))
                throw new ParseException("parsing failed");
            if (value < minValue || value > maxValue)
                throw new ParseException("validation failed");

            return value;
        }

        public void Parse(string[] optVals)
		{
            parsedValues = new int[optVals.Length];
            for (int i = 0; i < optVals.Length; i++)
                parsedValues[i] = ParseSingle(optVals[i]);
        }

        public int? GetValue(int idx = 0)
        {
            if (idx < (parsedValues?.Length ?? 0)) return parsedValues[idx];
            return defaultValue;
        }
    }

    internal class ParsableString
	{
        string defaultValue;
        string[] domain;
        string[] parsedValues;

        public ParsableString(string defaultValue, string[] domain = null)
        {
            this.domain = domain;
            this.defaultValue = defaultValue;
            parsedValues = new string[0];
        }

        private string ParseSingle(string opt)
        {
            if (domain != null && !domain.Contains(opt))
                throw new ParseException("validation failed");

            return opt;
        }

        public void Parse(string[] optVals)
        {
            parsedValues = new string[optVals.Length];
            for (int i = 0; i < optVals.Length; i++)
                parsedValues[i] = ParseSingle(optVals[i]);
        }

        public string GetValue(int idx = 0)
        {
            if (idx < (parsedValues?.Length ?? 0)) return parsedValues[idx];
            return defaultValue;
        }
    }

    internal class ParsableBool
    {
        static readonly string[] trues = new string[] { "true", "1" };
        static readonly string[] falses = new string[] { "false", "0" };

        bool? defaultValue;
        bool[] parsedValues;

        public ParsableBool(bool? defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        private bool ParseSingle(string opt)
        {
            if (trues.Contains(opt.ToLower())) return true;
            if (falses.Contains(opt.ToLower())) return false;
            throw new ParseException("parsing failed");
        }

        public void Parse(string[] optVals)
        {
            parsedValues = new bool[optVals.Length];
            for (int i = 0; i < optVals.Length; i++)
                parsedValues[i] = ParseSingle(optVals[i]);
        }

        public bool? GetValue(int idx = 0)
        {
            if (idx < (parsedValues?.Length ?? 0)) return parsedValues[idx];
            return defaultValue;
        }
    }
}
