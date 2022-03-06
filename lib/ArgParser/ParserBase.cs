using System;

namespace ArgParser
{
    /// <summary>
    /// To specify options/arguments
    /// Inherit from this class and declare Option / Argument fields that are descendants of ArgumentBase / OptionBase
    /// After parse is called: Parsed results could be obtained using fieldName.GetValue()
    /// </summary>
    public abstract class ParserBase
    {
        /// <summary>
        /// A command-line argument consisting of two dashes only, default: --. Any subsequent argument is considered to be a plain argument
        /// </summary>
        protected virtual string Delimiter => "--";

        /// <summary>
        /// Used by the parser to determine order of arguments
        /// </summary>
        /// <returns>References to argument fields</returns>
        protected virtual IArgument[] GetArgumentOrder() => new IArgument[0];
        internal IArgument[] CallGetArgumentOrder() => GetArgumentOrder();

        /// <summary>
        /// Parses args and stores parsed values in declared fields.
        /// </summary>
        /// <param name="args">Arguments to parse</param>
        /// <exception cref="ParserCodeException">Thrown when the class doesn't conform to the parser requirements</exception>
        /// <exception cref="ParseException">Thrown when parsed arguments don't satisfy declared option fields</exception>
        public void Parse(string[] args) { /*internal impl*/ }

        /// <summary>
        /// Automatically generates help message from declared fields (name, description)
        /// </summary>
        public string GenerateHelp() { return ""; }
    }


    /// <summary>
    /// Describes number of accepted parameters. The default is to accept exactly 1 parameter
    /// (the default constructor will construct such an instance).
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

        public static ParameterAccept Mandatory = new ParameterAccept();
        public static ParameterAccept Optional = new ParameterAccept(0, 1);
        public static ParameterAccept AtleastOne = new ParameterAccept(1, int.MaxValue);
        public static ParameterAccept Any = new ParameterAccept(0, int.MaxValue);
    };
}
