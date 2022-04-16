using System;
using System.Linq;

namespace ArgParser
{
    internal interface IOption
    {
        string[] Names { get; }
        string Description { get; }
        ParameterAccept ParameterAccept { get; }

        void CallParse(string[] optVals);
        bool MatchingOptionName(string optName);

        bool IsMandatory { get; }
        bool IsSet { get; }

        string GetHelp();
    }

    /// <typeparam name="T">Type of option value</typeparam>
    public abstract class OptionBase<T> : IOption
    {
        public const int NOT_SET = -1;

        internal readonly string[] names;
        string[] IOption.Names => names;

        internal readonly string description;
        string IOption.Description => description;

        internal readonly bool isMandatory;
        bool IOption.IsMandatory => isMandatory;

        /// <summary>
        /// Shortcut for <see cref="OptionBase{T}.ParsedParameterCount">ParsedParameterCount == <see cref="OptionBase{T}.NOT_SET">NOT_SET</see>
        /// </summary>
        public bool IsSet => ParsedParameterCount != NOT_SET;
        bool IOption.IsSet => IsSet;

        internal readonly ParameterAccept parameterAccept;
        ParameterAccept IOption.ParameterAccept => parameterAccept;

        protected abstract string GetTypeAsString();
        protected abstract string GetConstraintsAsString();

        string IOption.GetHelp() =>
            $"\t{string.Join(' ', names)} : {GetTypeAsString()}{parameterAccept.GetHelp()}" + (isMandatory ? " (MANDATORY)" : "") + GetConstraintsAsString()
                + $"\n\t\t{description}\n\n";

        void IOption.CallParse(string[] optVals)
        {
            ParsedParameterCount = optVals?.Length ?? 0;
            Parse(optVals);
        }

        bool IOption.MatchingOptionName(string optName)
        {
            return names.Contains(optName);
        }

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
        public T GetValue(int idx = 0)
        {
            if (idx < 0) throw new IndexOutOfRangeException();
            return GetValueImpl(idx);
        }


        protected abstract T GetValueImpl(int idx);

        /// <summary>
        /// Returns the count of parsed parameters or <see cref="OptionBase{T}.NOT_SET">NOT_SET</see> (-1) if not parsed at all.
        /// </summary>
        public int ParsedParameterCount { get; private set; } = NOT_SET;

        protected OptionBase(string[] names, string description, ParameterAccept parameterAccept, bool isMandatory)
        {
            this.names = names.Select(name => name.Length == 1 ? "-" + name : "--" + name).ToArray();
            this.description = description;
            this.parameterAccept = parameterAccept;
            this.isMandatory = isMandatory;
        }

	}

    public sealed class IntOption : OptionBase<int?>
    {
        ParsableInt parsable;

        public IntOption(string[] names, string description,
            int minValue = int.MinValue, int maxValue = int.MaxValue,
            ParameterAccept parameterAccept = new ParameterAccept(),
            int? defaultValue = null, bool isMandatory = false) : base(names, description, parameterAccept, isMandatory)
        {
            parsable = new(minValue, maxValue, defaultValue);
        }

        protected override void Parse(string[] optVals) => parsable.Parse(optVals);

        protected override int? GetValueImpl(int idx) => parsable.GetValue(idx);

        protected override string GetTypeAsString() => parsable.GetTypeAsString();
        protected override string GetConstraintsAsString() => parsable.GetConstraintsAsString();
    }

    public sealed class StringOption : OptionBase<string>
    {
        ParsableString parsable;

        public StringOption(string[] names, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null, bool isMandatory = false) : base(names, description, parameterAccept, isMandatory)
        {
            parsable = new(defaultValue);
        }

        protected override void Parse(string[] optVals) => parsable.Parse(optVals);

        protected override string GetValueImpl(int idx) => parsable.GetValue(idx);

        protected override string GetTypeAsString() => parsable.GetTypeAsString();
        protected override string GetConstraintsAsString() => parsable.GetConstraintsAsString();
    }

    public sealed class EnumOption : OptionBase<string>
    {
        ParsableString parsable;

        public EnumOption(string[] names, string description, string[] domain,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null, bool isMandatory = false) : base(names, description, parameterAccept, isMandatory)
        {
            parsable = new(defaultValue, domain);
        }

        protected override void Parse(string[] optVals) => parsable.Parse(optVals);

        protected override string GetValueImpl(int idx) => parsable.GetValue(idx);

        protected override string GetTypeAsString() => parsable.GetTypeAsString();
        protected override string GetConstraintsAsString() => parsable.GetConstraintsAsString();
    }

    public sealed class BoolOption : OptionBase<bool?>
    {
        ParsableBool parsable;

        public BoolOption(string[] names, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            bool? defaultValue = null, bool isMandatory = false) : base(names, description, parameterAccept, isMandatory)
        {
            parsable = new(defaultValue);
        }

        protected override void Parse(string[] optVals) => parsable.Parse(optVals);

        protected override bool? GetValueImpl(int idx) => parsable.GetValue(idx);

        protected override string GetTypeAsString() => parsable.GetTypeAsString();
        protected override string GetConstraintsAsString() => parsable.GetConstraintsAsString();
    }

    /// <summary>
    /// A special case of an option. Its <see cref="OptionBase.GetValue(int)">GetValue</see>
    /// method returns whether it was in parsed arguments.
    /// </summary>
    public sealed class NoValueOption : OptionBase<bool>
    {
        public NoValueOption(string[] names, string description)
            : base(names, description, ParameterAccept.Zero, isMandatory: false) { }

        protected override void Parse(string[] optValue = null) { }

        protected override bool GetValueImpl(int idx = 0) => IsSet;

        protected override string GetTypeAsString() => "flag";
        protected override string GetConstraintsAsString() => "";
    }
}