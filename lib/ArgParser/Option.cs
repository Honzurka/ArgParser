using System;
using System.Linq;

namespace ArgParser
{
    internal interface IOption
    {
        string[] Names { get; }
        void CallParse(string[] optVals);
        bool MatchingOptionName(string optName);
        ParameterAccept ParameterAccept { get; }
        bool IsMandatory { get; }
        bool IsSet { get; }
    }

    /// <typeparam name="T">Type of option value</typeparam>
    public abstract class OptionBase<T> : IOption
    {
        internal readonly string[] names;
        internal readonly string Description;
        internal readonly bool IsMandatory;

        bool IOption.IsMandatory => IsMandatory;
        bool IOption.IsSet => IsSet;

        string[] IOption.Names => names;

        internal readonly ParameterAccept parameterAccept;
        ParameterAccept IOption.ParameterAccept => parameterAccept;

        void IOption.CallParse(string[] optVals)
        {
            ParsedParameterCount = optVals?.Length ?? 0;
            Parse(optVals);
        }

        bool IOption.MatchingOptionName(string optName)
        {
            return names.Contains(optName);
            // --opt -o -opt --o
            // -abc = -a -b -c
            // -p42 = -p 42
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

        public const int NOT_SET = -1;

        protected OptionBase(string[] names, string description, ParameterAccept parameterAccept, bool isMandatory)
        {
            this.names = names.Select(name => name.Length == 1 ? "-" + name : "--" + name).ToArray();
            Description = description;
            this.parameterAccept = parameterAccept;
            IsMandatory = isMandatory;
        }

        /// <summary>
        /// Shortcut for <see cref="OptionBase{T}.ParsedParameterCount">ParsedParameterCount == <see cref="OptionBase{T}.NOT_SET">NOT_SET</see>
        /// </summary>
        public bool IsSet => ParsedParameterCount != NOT_SET;
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
    }

    /// <summary>
    /// A special case of an option. Its <see cref="OptionBase.GetValue(int)">GetValue</see>
    /// method returns whether it was in parsed arguments.
    /// </summary>
    public sealed class NoValueOption : OptionBase<bool>
    {
        public NoValueOption(string[] names, string description)
            : base(names, description, ParameterAccept.CreateZeroParameterAccept(), isMandatory: false) { }

        protected override void Parse(string[] optValue = null) { }

        protected override bool GetValueImpl(int idx = 0) => IsSet;
    }
}