namespace ArgParser
{
    /// <typeparam name="T">Type of option value</typeparam>
    public abstract class OptionBase<T>
    {

        /// <summary>
        /// Use to determine whether option was not set or set (even without parameters).
        /// </summary>
        public bool IsSet { get; private set; }
		// internal T defaultValue;

		/// <summary>
		/// Checks type and restrictions.
		/// Saves typed result in its internal state.
		/// </summary>
		/// <param name="optVals">Arguments passed to the parser that correspond to this option/argument</param>
		/// <exception cref="ParseException">Thrown when type or restrictions aren't fulfilled</exception>
		abstract protected void Parse(string[] optVals);

        
        /// <summary>
        /// Called by user to access parsed value(s).
        /// </summary>
        /// <param name="idx">Index of accessed value</param>
        /// <returns>Null if idx is out of range</returns>
        public T GetValue(int idx = 0) { return default; }
        //    => idx < 0 || idx > paramValues.Count ? default(T) : paramValues[idx];
    }

    public sealed class IntOption : OptionBase<int?>
    {
        public IntOption(string[] names, string description,
            int minValue = int.MinValue, int maxValue = int.MaxValue,
            ParameterAccept parameterAccept = new ParameterAccept(),
            int? defaultValue = null, bool isMandatory = false) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class StringOption : OptionBase<string>
    {
        public StringOption(string[] names, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null, bool isMandatory = false) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class EnumOption : OptionBase<string>
    {
        public EnumOption(string[] names, string description, string[] domain,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null, bool isMandatory = false) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class BoolOption : OptionBase<bool?>
    {
        public BoolOption(string[] names, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            bool? defaultValue = null, bool isMandatory = false) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class NoValueOption : OptionBase<bool>
    {
        public NoValueOption(string[] names, string description) { }
        protected override void Parse(string[] optValue = null) { }
    }
}