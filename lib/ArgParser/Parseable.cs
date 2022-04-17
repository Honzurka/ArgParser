using System;
using System.Linq;

namespace ArgParser
{
	public interface IParsable<T>
	{
		void Parse(string[] optVals);
		public T? GetValue(int idx);

		string TypeAsString { get; }
		string ConstraintsAsString { get; }
	}

	internal class ParsableInt : IParsable<int?>
	{
		readonly int minValue;
		readonly int maxValue;
		readonly int? defaultValue;
		int[] parsedValues = Array.Empty<int>();

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
			if (idx < parsedValues.Length) return parsedValues[idx];
			return defaultValue;
		}

		string IParsable<int?>.TypeAsString => "integer";

		string IParsable<int?>.ConstraintsAsString
		{
			get
			{
				string result = "";
				if (minValue != int.MinValue) result += $"min: {minValue} ";
				if (maxValue != int.MaxValue) result += $"max: {maxValue}";

				return result;
			}
		}

		/*
		string IParsable<int?>.GetConstraintsAsString()
		{
			string result = "";
			if (minValue != int.MinValue) result += $"min: {minValue} ";
			if (maxValue != int.MaxValue) result += $"max: {maxValue}";

			return result;
		}*/
	}

	internal class ParsableString : IParsable<string?>
	{
		readonly string? defaultValue;
		readonly string[]? domain;
		string[] parsedValues = Array.Empty<string>();

		public ParsableString(string? defaultValue, string[]? domain = null)
		{
			this.domain = domain;
			this.defaultValue = defaultValue;
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

		public string? GetValue(int idx = 0)
		{
			if (idx < parsedValues.Length) return parsedValues[idx];
			return defaultValue;
		}

		string IParsable<string?>.TypeAsString => "string";

		string IParsable<string?>.ConstraintsAsString
		{
			get
			{
				string result = "";

				if (domain != null)
				{
					result += "Allowed values: ";
					foreach (var d in domain)
					{
						result += $"{d} ";
					}
				}

				return result;
			}
		}
	}

	internal class ParsableBool : IParsable<bool?>
	{
		static readonly string[] trues = new string[] { "true", "1" };
		static readonly string[] falses = new string[] { "false", "0" };

		readonly bool? defaultValue;
		bool[] parsedValues = Array.Empty<bool>();

		public ParsableBool(bool? defaultValue)
		{
			this.defaultValue = defaultValue;
		}

		private static bool ParseSingle(string opt)
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
			if (idx < parsedValues.Length) return parsedValues![idx];
			return defaultValue;
		}

		string IParsable<bool?>.TypeAsString => "boolean";

		string IParsable<bool?>.ConstraintsAsString
		{
			get
			{
				string result = "";

				result += $"\n\t\tAllowed values(case insensitive): ";
				result += $"(true):";
				foreach (var t in trues)
				{
					result += $" {t}";
				}
				result += $" | (false):";
				foreach (var f in falses)
				{
					result += $" {f}";
				}

				return result;
			}
		}
	}

	internal class ParsableFlag : IParsable<bool>
	{
		bool isset = false;

		public string TypeAsString => "flag";

		public string ConstraintsAsString => "";

		public bool GetValue(int idx) => isset;
		public void Parse(string[] optVals) => isset = true;
	}
}
