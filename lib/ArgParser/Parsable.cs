using System;
using System.Linq;
using System.Collections.Generic;

namespace ArgParser
{
	/// <summary>
	/// Base interface for implementation of all options / arguments.
	///
	/// A class implementing this interface can be used to provide
	/// functionality to both an option and an argument (or only one of them,
	/// if needed) by creating a class inheriting from
	/// <see cref="OptionBase{T}">OptionBase</see> /
	/// <see cref="ArgumentBase{T}">ArgumentBase</see>,
	/// and passing the IParsable class in the constructor.
	/// </summary>
	/// 
	/// <typeparam name="T">
	/// Type of the parsed value (often nullable, to avoid making default
	/// values confusing).
	/// </typeparam>
	public interface IParsable<T>
	{
		/// <summary>
		/// Parses, validates and stores the given option values.
		/// 
		/// The function shall try to parse the optVals, validate it against
		/// the instance's constraints, and either save the result in its
		/// internal state to be obtained later by
		/// <see cref="IParsable{T}.GetValue(int)">GetValue</see>
		/// or throw an exception indicating that the parsing failed.
		/// </summary>
		/// 
		/// <param name="optVals">
		/// Arguments passed to the parser that correspond to this option /
		/// argument.
		/// </param>
		/// 
		/// <exception cref="ParseException">
		/// Thrown when type or constraints aren't fulfilled.
		/// </exception>
		void Parse(string[] optVals);

		/// <summary>
		/// Called by user to access parsed value(s).
		/// </summary>
		/// <param name="idx">Index of accessed value</param>
		/// <returns>Default value if idx is out of range</returns>
		public T? GetValue(int idx);

		/// <summary>
		/// Returns a string with a name of the parsed type
		/// (e.g. "comma_separated_ints").
		/// </summary>
		string TypeAsString { get; }

		/// <summary>
		/// Returns a string describing how the parsable is constrained
		/// (e.g. "min: 0 max: 42").
		/// </summary>
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
				throw new ParseException($"{opt} couldn't be parsed as int");
			if (value < minValue || value > maxValue)
				throw new ParseException($"{opt} is out of specified range [{minValue}..{maxValue}]");

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
				throw new ParseException($"{opt} doesnt belong to domain `{string.Join(' ', domain)}`");

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
			throw new ParseException($"{opt} not recognized as boolean value");
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