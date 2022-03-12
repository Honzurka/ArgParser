namespace ArgParser
{
    /// <summary>
    /// Shared by all arguments.
    /// </summary>
    public interface IArgument<T>
    {
        /// <summary>
        /// Called by user to access parsed value(s).
        /// </summary>
        /// <param name="idx">Index of accessed value</param>
        /// <returns>Null if idx is out of range</returns>
        public T GetValue(int idx = 0);// { return default(T); }
        //    => idx < 0 || idx > paramValues.Count ? default(T) : paramValues[idx];
        // public T GetValueWithDefaultReplace(int idx = 0);
    }

    /// <typeparam name="T">Type of argument value</typeparam>
    public abstract class ArgumentBase
    {
        /// <summary>
        /// Checks type and restrictions.
        /// Saves typed result in its internal state.
        /// </summary>
        /// <param name="optVals">Arguments passed to the parser that correspond to this option/argument</param>
        /// <exception cref="ParseException">Thrown when type or restrictions aren't fulfilled</exception>
        abstract protected void Parse(string[] optVals);
    }

    public sealed class IntArgument : ArgumentBase, IArgument<int?>
    {
        public IntArgument(string name, string description,
            int minValue = int.MinValue, int maxValue = int.MaxValue,
            ParameterAccept parameterAccept = new ParameterAccept(),
            int? defaultValue = null)
        { }

		public int? GetValue(int idx = 0)
		{
            return null;
		}

		protected override void Parse(string[] optVals) { }
    }
    public sealed class StringArgument : ArgumentBase, IArgument<string>
    {
        public StringArgument(string name, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null)
        { }

		public string GetValue(int idx = 0)
		{
            return null;
		}

		protected override void Parse(string[] optVals) { }
    }
    public sealed class EnumArgument : ArgumentBase, IArgument<string>
    {
        public EnumArgument(string name, string description, string[] domain,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null)
        { }

		public string GetValue(int idx = 0)
		{
            return null;
		}

		protected override void Parse(string[] optVals) { }
    }
    public sealed class BoolArgument : ArgumentBase, IArgument<bool?>
    {
        public BoolArgument(string name, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            bool? defaultValue = null)
        { }

		public bool? GetValue(int idx = 0)
		{
            return null;
		}

		protected override void Parse(string[] optVals) { }
    }

}
