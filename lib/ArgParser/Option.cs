namespace ArgParser
{
    /// <typeparam name="T">Type of option value</typeparam>
    public abstract class OptionBase<T>
    {
        internal readonly string[] Names;
        internal readonly string Description;
        internal readonly ParameterAccept parameterAccept;
        internal readonly bool IsMandatory;

        internal void CallParse(string[] optVals) => Parse(optVals);

        // internal T defaultValue;

        /// <summary>
        /// Checks type and restrictions.
        /// Saves typed result in its internal state.
        /// </summary>
        /// <param name="optVals">Arguments passed to the parser that correspond to this option/argument</param>
        /// <exception cref="ParseException">Thrown when type or restrictions aren't fulfilled</exception>
        protected abstract void Parse(string[] optVals);


        /// <summary>
        /// Called by user to access parsed value(s).
        /// </summary>
        /// <param name="idx">Index of accessed value</param>
        /// <returns>Default value if idx is out of range</returns>
        public abstract T GetValue(int idx = 0);
        //    => idx < 0 || idx > paramValues.Count ? default(T) : paramValues[idx];

        /// <summary>
        /// Returns the count of parsed parameters or <see cref="OptionBase{T}.NOT_SET">NOT_SET</see> (-1) if not parsed at all.
        /// </summary>
        public int ParsedParameterCount { get; }

        public const int NOT_SET = -1;

        /// <summary>
        /// Shortcut for <see cref="OptionBase{T}.ParsedParameterCount">ParsedParameterCount == <see cref="OptionBase{T}.NOT_SET">NOT_SET</see>
        /// </summary>
        public bool IsSet { get; }
    }

    public sealed class IntOption : OptionBase<int?>
    {
        public IntOption(string[] names, string description,
            int minValue = int.MinValue, int maxValue = int.MaxValue,
            ParameterAccept parameterAccept = new ParameterAccept(),
            int? defaultValue = null, bool isMandatory = false)
        { }
        protected override void Parse(string[] optVals) { }
        public override int? GetValue(int idx = 0) => default;
    }

    public sealed class StringOption : OptionBase<string>
    {
        public StringOption(string[] names, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null, bool isMandatory = false)
        { }
        protected override void Parse(string[] optVals) { }
        public override string GetValue(int idx = 0) => default;
    }

    public sealed class EnumOption : OptionBase<string>
    {
        public EnumOption(string[] names, string description, string[] domain,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null, bool isMandatory = false)
        { }
        protected override void Parse(string[] optVals) { }
        public override string GetValue(int idx = 0) => default;
    }

    public sealed class BoolOption : OptionBase<bool?>
    {
        public BoolOption(string[] names, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            bool? defaultValue = null, bool isMandatory = false)
        { }
        protected override void Parse(string[] optVals) { }
        public override bool? GetValue(int idx = 0) => default;
    }

    /// <summary>
    /// A special case of an option. Its <see cref="OptionBase.GetValue(int)">GetValue</see>
    /// method returns whether it was in parsed arguments.
    /// </summary>
    public sealed class NoValueOption : OptionBase<bool>
    {
        public NoValueOption(string[] names, string description) { }
        protected override void Parse(string[] optValue = null) { }
        public override bool GetValue(int idx = 0) => IsSet;
    }
}