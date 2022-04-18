using System;
using System.Linq;
using System.Collections.Generic;

namespace ArgParser
{
	public interface IParsable<T>
	{
		void Parse(string[] optVals);

		public T? GetValue(int idx);


		string TypeAsString { get; }

		string ConstraintsAsString { get; }
	}

	internal sealed class ParsableInt : IParsable<int?>
	{
		readonly int minValue, maxValue;
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
				throw new ParseException("parsing failed");	// TODO: change parse error texts
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

		public int? GetValue(int idx)
		{
			if (idx < parsedValues.Length) return parsedValues[idx];
			return defaultValue;
		}

		string IParsable<int?>.TypeAsString => "integer";

		string IParsable<int?>.ConstraintsAsString
		{
			get
			{
				var constraints = new List<string>(); 
				if (minValue != int.MinValue) constraints.Add($"min: {minValue}");
				if (maxValue != int.MaxValue) constraints.Add($"max: {maxValue}");
				return string.Join(' ', constraints);
			}
		}
	}

	internal sealed class ParsableString : IParsable<string?>
	{
		readonly string[]? domain;
		readonly string? defaultValue;
		string[] parsedValues = Array.Empty<string>();

		public ParsableString(string? defaultValue, string[]? domain = null)
		{
			this.domain = domain;
			this.defaultValue = defaultValue;
		}

		private string ParseSingle(string opt)
		{
			if (domain != null && !domain.Contains(opt))
				throw new ParseException("validation failed");	// TODO: change parse error texts

			return opt;
		}

		public void Parse(string[] optVals)
		{
			parsedValues = new string[optVals.Length];
			for (int i = 0; i < optVals.Length; i++)
				parsedValues[i] = ParseSingle(optVals[i]);
		}

		public string? GetValue(int idx)
		{
			if (idx < parsedValues.Length) return parsedValues[idx];
			return defaultValue;
		}

		string IParsable<string?>.TypeAsString => "string";

		string IParsable<string?>.ConstraintsAsString => domain != null
			? $"Allowed values: {string.Join(' ', domain)}" : "";
	}

	internal sealed class ParsableBool : IParsable<bool?>
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
			throw new ParseException("parsing failed");	// TODO: change parse error texts
		}

		public void Parse(string[] optVals)
		{
			parsedValues = new bool[optVals.Length];
			for (int i = 0; i < optVals.Length; i++)
				parsedValues[i] = ParseSingle(optVals[i]);
		}

		public bool? GetValue(int idx)
		{
			if (idx < parsedValues.Length) return parsedValues[idx];
			return defaultValue;
		}

		string IParsable<bool?>.TypeAsString => "boolean";

		string IParsable<bool?>.ConstraintsAsString =>
			$"\n\t\tAllowed values(case insensitive): " +
			$"(true): {string.Join(' ', trues)} | " +
			$"(false): {string.Join(' ', falses)}";
	}

	internal sealed class ParsableFlag : IParsable<bool>
	{
		bool isSet = false;

		public void Parse(string[] optVals) => isSet = true;

		public bool GetValue(int idx) => isSet;

		public string TypeAsString => "flag";

		public string ConstraintsAsString => "";
	}
}