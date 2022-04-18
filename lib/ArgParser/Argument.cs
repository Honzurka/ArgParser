namespace ArgParser
{
	/// <summary>
	/// Specifies that this class is an argument. The interface contains internal methods
	/// and should not be implemented. Use <see cref="ArgumentBase{T}">ArgumentBase</see> instead.
	/// </summary>
	public interface IArgument
	{
		internal ParameterAccept ParameterAccept { get; }

		internal void CallParse(string[] optVals);


		internal string Name { get; }

		internal string GetHelp();
	}


	/// <summary>
	/// Shared by all arguments.
	/// </summary>
	/// <typeparam name="T">Type of argument value</typeparam>
	public abstract class ArgumentBase<T> : IArgument
	{
		readonly IParsable<T> parsable;
		readonly ParameterAccept parameterAccept;
		readonly string name;
		readonly string description;

		string IArgument.Name => name;
		ParameterAccept IArgument.ParameterAccept => parameterAccept;

		string IArgument.GetHelp() =>	string.Format("\t{0} : {1}{2}\n\t\t{3}",
			name, parsable.TypeAsString + parameterAccept.GetHelp(),
			parsable.ConstraintsAsString, description
		);

		protected ArgumentBase(string name, string description, ParameterAccept parameterAccept, IParsable<T> parsable)
		{
			this.name = name;
			this.description = description;
			this.parameterAccept = parameterAccept;
			this.parsable = parsable;
		}

		void IArgument.CallParse(string[] optVals)
		{
			// if (optVals.Length > parameterAccept.MaxParamAmount ||
			//     optVals.Length < parameterAccept.MaxParamAmount) throw
			ParsedArgumentCount = optVals.Length;
			Parse(optVals);
		}


		/// <summary>
		/// Checks type and restrictions and saves the typed result in its internal state.
		/// </summary>
		/// <param name="optVals">Arguments passed to the parser that correspond to this option / argument</param>
		/// <exception cref="ParseException">Thrown when type or restrictions aren't fulfilled</exception>
		void Parse(string[] optVals) => parsable.Parse(optVals);


		/// <summary>
		/// Called by user to access parsed value(s).
		/// </summary>
		/// <param name="idx">Index of accessed value</param>
		/// <returns>The parsed value, or default value if idx is out of range.</returns>
		public T? GetValue(int idx = 0) => parsable.GetValue(idx);

		/// <summary>
		/// Returns the count of plain arguments parsed by this instance.
		/// </summary>
		public int ParsedArgumentCount { get; private set; }
	}

	public sealed class IntArgument : ArgumentBase<int?>
	{
		public IntArgument(string name, string description,
			int minValue = int.MinValue, int maxValue = int.MaxValue,
			ParameterAccept parameterAccept = new ParameterAccept(),
			int? defaultValue = null)
				: base(name, description, parameterAccept,
					new ParsableInt(minValue, maxValue, defaultValue)) { }
	}

	public sealed class StringArgument : ArgumentBase<string?>
	{
		public StringArgument(string name, string description,
			ParameterAccept parameterAccept = new ParameterAccept(),
			string? defaultValue = null)
				: base(name, description, parameterAccept,
					new ParsableString(defaultValue)) { }
	}

	public sealed class EnumArgument : ArgumentBase<string?>
	{
		public EnumArgument(string name, string description, string[] domain,
			ParameterAccept parameterAccept = new ParameterAccept(),
			string? defaultValue = null)
				: base(name, description, parameterAccept,
					new ParsableString(defaultValue, domain)) { }
	}

	public sealed class BoolArgument : ArgumentBase<bool?>
	{
		public BoolArgument(string name, string description,
			ParameterAccept parameterAccept = new ParameterAccept(),
			bool? defaultValue = null)
				: base(name, description, parameterAccept,
					new ParsableBool(defaultValue)) { }
	}
}