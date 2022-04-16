namespace ArgParser
{
    /// <summary>
    /// Specifies that this class is an argument. The interface contains internal methods
    /// and should not be implemented. Use <see cref="ArgumentBase{T}">ArgumentBase</see> instead.
    /// </summary>
    public interface IArgument {
        internal void CallParse(string[] optVals);
        internal ParameterAccept ParameterAccept { get; }
    }

    /// <summary>
    /// Shared by all arguments.
    /// </summary>
    /// <typeparam name="T">Type of argument value</typeparam>
    public abstract class ArgumentBase<T> : IArgument
    {
        internal readonly string Name;
        internal readonly string Description;
        internal readonly ParameterAccept parameterAccept;

		protected ArgumentBase(string name, string description, ParameterAccept parameterAccept)
		{
			Name = name;
			Description = description;
			this.parameterAccept = parameterAccept;
		}

		ParameterAccept IArgument.ParameterAccept => parameterAccept;

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
        int minValue;
        int maxValue;
        int? defaultValue;
        int[] parsedValues;

        public IntArgument(string name, string description,
            int minValue = int.MinValue, int maxValue = int.MaxValue,
            ParameterAccept parameterAccept = new ParameterAccept(),
            int? defaultValue = null) : base(name, description, parameterAccept)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.defaultValue = defaultValue;
        }

        protected override void Parse(string[] optVals)
        {
            parsedValues = new int[optVals.Length];

            for (int i = 0; i < optVals.Length; i++)
            {
                /*parsing int with constraints ---- move outside to share with options*/
                if (!int.TryParse(optVals[i], out var value))
                    throw new ParseException("parsing failed");
                if (value < minValue || value > maxValue)
                    throw new ParseException("validation failed");

                parsedValues[i] = value;
            }
        }

        public override int? GetValue(int idx = 0)
        {
            if (idx < parsedValues.Length) return parsedValues[idx];

            return defaultValue;
        }
    }

    public sealed class StringArgument : ArgumentBase<string>
    {
        public StringArgument(string name, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null) : base(name, description, parameterAccept)
        { }

        protected override void Parse(string[] optVals) { }

        public override string GetValue(int idx = 0) => default;
    }

    public sealed class EnumArgument : ArgumentBase<string>
    {
        public EnumArgument(string name, string description, string[] domain,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null) : base(name, description, parameterAccept)
        { }

        protected override void Parse(string[] optVals) { }

        public override string GetValue(int idx = 0) => default;
    }

    public sealed class BoolArgument : ArgumentBase<bool?>
    {
        public BoolArgument(string name, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            bool? defaultValue = null) : base(name, description, parameterAccept)
        { }

        protected override void Parse(string[] optVals) { }

        public override bool? GetValue(int idx = 0) => default;
    }
}
