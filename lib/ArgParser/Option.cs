using System;
using System.Linq;

namespace ArgParser
{
	internal interface IOption
	{
		bool IsMandatory { get; }

		bool IsSet { get; }

		ParameterAccept ParameterAccept { get; }

		bool MatchingOptionName(string optName);

		void CallParse(string[] optVals);

		string[] Names { get; }

		string GetHelp();
	}

	/// <typeparam name="T">Type of option value</typeparam>
	public abstract class OptionBase<T> : IOption
	{
		public const int NOT_SET = -1;

		readonly IParsable<T> parsable;
		readonly ParameterAccept parameterAccept;
		readonly bool isMandatory;
		readonly string[] names;
		readonly string description;

		protected OptionBase(string[] names, string description, ParameterAccept parameterAccept, bool isMandatory, IParsable<T> parsable)
		{
			this.names = names.Select(name => name.Length == 1 ? "-" + name : "--" + name).ToArray();
			this.description = description;
			this.parameterAccept = parameterAccept;
			this.isMandatory = isMandatory;
			this.parsable = parsable;
		}

		/// <summary>
		/// Returns the count of parsed parameters or <see cref="OptionBase{T}.NOT_SET">NOT_SET</see> (-1) if not parsed at all.
		/// </summary>
		public int ParsedParameterCount { get; private set; } = NOT_SET;

		/// <summary>
		/// Shortcut for <see cref="OptionBase{T}.ParsedParameterCount">ParsedParameterCount == <see cref="OptionBase{T}.NOT_SET">NOT_SET</see>
		/// </summary>
		public bool IsSet => ParsedParameterCount != NOT_SET;

		bool IOption.IsMandatory => isMandatory;
		
		bool IOption.IsSet => IsSet;
		
		ParameterAccept IOption.ParameterAccept => parameterAccept;
		
		string[] IOption.Names => names;

		bool IOption.MatchingOptionName(string optName) => names.Contains(optName);

		void IOption.CallParse(string[]? optVals)
		{
			ParsedParameterCount = optVals?.Length ?? 0;
			Parse(optVals ?? Array.Empty<string>());
		}

		/// <summary>
		/// Checks type and restrictions.
		/// Saves typed result in its internal state.
		/// </summary>
		/// <param name="optVals">Arguments passed to the parser that correspond to this option/argument</param>
		/// <exception cref="ParseException">Thrown when type or restrictions aren't fulfilled</exception>
		void Parse(string[] optVals) => parsable.Parse(optVals);

		/// <summary>
		/// Called by user to access parsed value(s).
		/// </summary>
		/// <param name="idx">Index of accessed value</param>
		/// <returns>Default value if idx is out of range</returns>
		public T? GetValue(int idx = 0)
		{
			if (idx < 0) throw new IndexOutOfRangeException("Index must be non-negative value.");
			return parsable.GetValue(idx);
		}

		string IOption.GetHelp() =>	string.Format("\t{0} : {1}{2}{3}\n\t\t{4}",
			string.Join(' ', names),
			parsable.TypeAsString + parameterAccept.GetHelp(),
			isMandatory ? " (MANDATORY)" : "",
			parsable.ConstraintsAsString,
			description
		);
	}

	public sealed class IntOption : OptionBase<int?>
	{
		public IntOption(string[] names, string description,
			int minValue = int.MinValue, int maxValue = int.MaxValue,
			ParameterAccept parameterAccept = new ParameterAccept(),
			int? defaultValue = null, bool isMandatory = false)
				: base(names, description, parameterAccept, isMandatory,
					new ParsableInt(minValue, maxValue, defaultValue)) { }
	}

	public sealed class StringOption : OptionBase<string?>
	{
		public StringOption(string[] names, string description,
			ParameterAccept parameterAccept = new ParameterAccept(),
			string? defaultValue = null, bool isMandatory = false)
				: base(names, description, parameterAccept, isMandatory,
					new ParsableString(defaultValue)) { }
	}

	public sealed class EnumOption : OptionBase<string?>
	{
		public EnumOption(string[] names, string description, string[] domain,
			ParameterAccept parameterAccept = new ParameterAccept(),
			string? defaultValue = null, bool isMandatory = false)
				: base(names, description, parameterAccept, isMandatory,
					new ParsableString(defaultValue, domain)) { }
	}

	public sealed class BoolOption : OptionBase<bool?>
	{
		public BoolOption(string[] names, string description,
			ParameterAccept parameterAccept = new ParameterAccept(),
			bool? defaultValue = null, bool isMandatory = false) :
				base(names, description, parameterAccept, isMandatory,
					new ParsableBool(defaultValue)) { }
	}


	/// <summary>
	/// A special case of an option. Its <see cref="OptionBase.GetValue(int)">GetValue</see>
	/// method returns whether it was in parsed arguments.
	/// </summary>
	public sealed class NoValueOption : OptionBase<bool>
	{
		public NoValueOption(string[] names, string description)
			: base(names, description, ParameterAccept.Zero,
				isMandatory: false, new ParsableFlag()) { }
	}
}