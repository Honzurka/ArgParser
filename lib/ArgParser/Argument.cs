namespace ArgParser
{
	/// <summary>
	/// Specifies that this class is an argument. The interface contains internal methods
	/// and should not be implemented. Use <see cref="ArgumentBase{T}">ArgumentBase</see> instead.
	/// </summary>
	public interface IArgument
	{
		internal string Name { get; }
		internal string Description { get; }
		internal ParameterAccept ParameterAccept { get; }

		internal void CallParse(string[] optVals);

		internal string GetHelp();
	}

	/// <summary>
	/// Shared by all arguments.
	/// </summary>
	/// <typeparam name="T">Type of argument value</typeparam>
	public abstract class ArgumentBase<T> : IArgument
	{
		internal readonly string name;
		string IArgument.Name => name;

		internal readonly string description;
		string IArgument.Description => description;

		internal readonly ParameterAccept parameterAccept;
		ParameterAccept IArgument.ParameterAccept => parameterAccept;

		protected abstract string GetTypeAsString();
		protected abstract string GetConstraintsAsString();

		string IArgument.GetHelp() =>
			$"\t{name} : {GetTypeAsString()}{parameterAccept.GetHelp()} {GetConstraintsAsString()}\n" +
			$"\t\t{description}\n\n";

		protected ArgumentBase(string name, string description, ParameterAccept parameterAccept)
		{
			this.name = name;
			this.description = description;
			this.parameterAccept = parameterAccept;
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
		abstract protected void Parse(string[] optVals);

		/// <summary>
		/// Called by user to access parsed value(s).
		/// </summary>
		/// <param name="idx">Index of accessed value</param>
		/// <returns>The parsed value, or default value if idx is out of range.</returns>
		public abstract T GetValue(int idx = 0);


		/// <summary>
		/// Returns the count of plain arguments parsed by this instance.
		/// </summary>
		public int ParsedArgumentCount { get; private set; }

	}

	public sealed class IntArgument : ArgumentBase<int?>
	{
		readonly ParsableInt parsable;

		public IntArgument(string name, string description,
			int minValue = int.MinValue, int maxValue = int.MaxValue,
			ParameterAccept parameterAccept = new ParameterAccept(),
			int? defaultValue = null) : base(name, description, parameterAccept)
		{
			parsable = new(minValue, maxValue, defaultValue);
		}

		protected override void Parse(string[] optVals) => parsable.Parse(optVals);

		public override int? GetValue(int idx = 0) => parsable.GetValue(idx);

		protected override string GetTypeAsString() => parsable.GetTypeAsString();
		protected override string GetConstraintsAsString() => parsable.GetConstraintsAsString();
	}

	public sealed class StringArgument : ArgumentBase<string?>
	{
		ParsableString parsable;

		public StringArgument(string name, string description,
			ParameterAccept parameterAccept = new ParameterAccept(),
			string? defaultValue = null) : base(name, description, parameterAccept)
		{
			parsable = new(defaultValue);
		}

		protected override void Parse(string[] optVals) => parsable.Parse(optVals);

		public override string? GetValue(int idx = 0) => parsable.GetValue(idx);

		protected override string GetTypeAsString() => parsable.GetTypeAsString();
		protected override string GetConstraintsAsString() => parsable.GetConstraintsAsString();
	}

	public sealed class EnumArgument : ArgumentBase<string?>
	{
		ParsableString parsable;

		public EnumArgument(string name, string description, string[] domain,
			ParameterAccept parameterAccept = new ParameterAccept(),
			string? defaultValue = null) : base(name, description, parameterAccept)
		{
			parsable = new(defaultValue, domain);
		}

		protected override void Parse(string[] optVals) => parsable.Parse(optVals);

		public override string? GetValue(int idx = 0) => parsable.GetValue(idx);

		protected override string GetTypeAsString() => parsable.GetTypeAsString();
		protected override string GetConstraintsAsString() => parsable.GetConstraintsAsString();
	}

	public sealed class BoolArgument : ArgumentBase<bool?>
	{
		ParsableBool parsable;

		public BoolArgument(string name, string description,
			ParameterAccept parameterAccept = new ParameterAccept(),
			bool? defaultValue = null) : base(name, description, parameterAccept)
		{
			parsable = new(defaultValue);
		}

		protected override void Parse(string[] optVals) => parsable.Parse(optVals);

		public override bool? GetValue(int idx = 0) => parsable.GetValue(idx);

		protected override string GetTypeAsString() => parsable.GetTypeAsString();
		protected override string GetConstraintsAsString() => parsable.GetConstraintsAsString();
	}
}
