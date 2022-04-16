using System;

namespace ArgParser
{
    /// <summary>
    /// To specify options/arguments
    /// Inherit from this class and declare Option / Argument fields that are descendants of
    /// <see cref="ArgParser.ArgumentBase">ArgumentBase</see> / <see cref="ArgParser.OptionBase{T}">OptionBase</see>
    /// After the <see cref="ArgParser.ParserBase.Parse(string[])">Parse</see> method is called,
    /// parsed results could be obtained using fieldName.GetValue()
    /// </summary>
    public abstract class ParserBase
    {
        /// <summary>
        /// A command-line argument consisting of two dashes only, default: "--". Any subsequent argument is considered to be a plain argument
        /// </summary>
        protected virtual string Delimiter => "--";

        /// <summary>
        /// Used by the parser to determine order of arguments.
        /// Since reflection doesn't necessarily retain the order of fields,
        /// it needs to be specified by the user.
        /// </summary>
        /// <returns>References to argument fields</returns>
        protected virtual IArgument[] GetArgumentOrder() => Array.Empty<IArgument>();
        internal IArgument[] CallGetArgumentOrder() => GetArgumentOrder();
        
        /// <summary>
        /// Parses args and stores parsed values in declared fields.
        /// </summary>
        /// <param name="args">Arguments to parse</param>
        /// <exception cref="ParserCodeException">Thrown when the class doesn't conform to the parser requirements</exception>
        /// <exception cref="ParseException">Thrown when parsed arguments don't satisfy declared option fields:
        ///     Fields not inheriting from predefined classes (descendants of `OptionBase<T>` or `ArgumentBase<T>`)
        ///     Defining argument fields without overriding `GetArgumentOrder`</exception>
        public void Parse(string[] args) { /*internal impl*/ }

        /// <summary>
        /// Automatically generates help message from declared fields (name, description).
        /// </summary>
        public string GenerateHelp() { return ""; }
    }


    /// <summary>
    /// Describes number of accepted parameters. The default is to accept exactly 1 parameter
    /// (the default parameterless constructor will construct such an instance).
    /// </summary>
    public struct ParameterAccept
    {
        public int MinParamAmount { get; }
        public int MaxParamAmount { get; }

        /// <exception cref="ArgumentException">Thrown when minParamAmount < 0 or maxParamAmount < minParamAmount or maxParamAmount == 0</exception>
        public ParameterAccept(int minParamAmount, int maxParamAmount)
        {
            //if (minParamAmount < 0 || maxParamAmount < minParamAmount || maxParamAmount == 0) throw new ArgumentException();
            MinParamAmount = minParamAmount;
            MaxParamAmount = maxParamAmount;
        }
        public ParameterAccept(int paramAmount) : this(paramAmount, paramAmount) { }

        public readonly static ParameterAccept Mandatory = new();
        public readonly static ParameterAccept Optional = new(0, 1);
        public readonly static ParameterAccept AtLeastOne = new(1, int.MaxValue);
        public readonly static ParameterAccept Any = new(0, int.MaxValue);
    };
}
