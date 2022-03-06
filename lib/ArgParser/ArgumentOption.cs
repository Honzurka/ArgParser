using System.Collections.Generic;

namespace ArgParser
{
    public abstract class ArgumentOptionBase {
        internal readonly string Description;
        internal readonly ParameterAccept parameterAccept;

        /// <summary>
        /// Checks type and restrictions.
        /// Saves typed result in its internal state.
        /// </summary>
        /// <param name="optVals">Arguments passed to the parser that correspond to this option/argument</param>
        /// <exception cref="ParseException">Thrown when type or restrictions aren't fulfilled</exception>
        abstract protected void Parse(string[] optVals);

        internal void CallParse(string[] optVals) => Parse(optVals);
    }

    /// <summary>
    /// Shared by all arguments.
    /// </summary>
    public interface IArgument {}

    /// <typeparam name="T">Type of argument value</typeparam>
    public abstract class ArgumentBase<T> : ArgumentOptionBase, IArgument {
        internal readonly string Name;

        /// <summary>
        /// Called by user to access parsed value(s).
        /// </summary>
        /// <param name="idx">Index of accessed value</param>
        /// <returns>Null if idx is out of range</returns>
        public T GetValue(int idx = 0) {return default(T);}
        //    => idx < 0 || idx > paramValues.Count ? default(T) : paramValues[idx];
        // public T GetValueWithDefaultReplace(int idx = 0);
    }

    public sealed class IntArgument : ArgumentBase<int?>
    {
        public IntArgument(string name, string description, int minValue = int.MinValue, int maxValue = int.MaxValue, 
            ParameterAccept parameterAccept = new ParameterAccept(),
            int? defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class StringArgument : ArgumentBase<string>
    {
        public StringArgument(string name, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class EnumArgument : ArgumentBase<string>
    {
        public EnumArgument(string name, string description, string[] domain,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class BoolArgument : ArgumentBase<bool?>
    {
        public BoolArgument(string name, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            bool? defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
    }


    /// <typeparam name="T">Type of option value</typeparam>
    public abstract class OptionBase<T> : ArgumentOptionBase
    {
        internal readonly string[] Names;
        internal readonly bool IsMandatory;
        // internal T defaultValue;

        /// <summary>
        /// Called by user to access parsed value(s).
        /// </summary>
        /// <param name="idx">Index of accessed value</param>
        /// <returns>Null if idx is out of range</returns>
        public T GetValue(int idx = 0) {return default(T);}
        //    => idx < 0 || idx > paramValues.Count ? default(T) : paramValues[idx];
    }

    public sealed class IntOption : OptionBase<int?>
    {
        public IntOption(string[] names, string description, int minValue = int.MinValue, int maxValue = int.MaxValue, 
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