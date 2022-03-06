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
        /// <exception cref="ParserCodeException">Thrown when not all arguments are specified in order</exception>
        protected virtual IArgument[] GetArgumentOrder() => null;
        internal IArgument[] CallGetArgumentOrder() => GetArgumentOrder();

        /// <summary>
        /// Parses args and stores parsed values in declared fields.
        /// </summary>
        /// <param name="args">Arguments to parse</param>
        /// <exception cref="ParseException">Thrown when parsed arguments don't satisfy declared option fields</exception>
        public void Parse(string[] args) { /*internal impl*/ }

        /// <summary>
        /// Automatically generates help message from declared fields (name, description)
        /// </summary>
        public string GenerateHelp() { return ""; }
    }


    public struct ParameterAccept {
        public readonly int MinParameterAmount, MaxParameterAmount;

        /// <summary>
        /// Describes number of accepted parameters.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when minParameterAmount < 0 or maxParameterAmount < minParameterAmount </exception>
        public ParameterAccept(int minParameterAmount = 1, int maxParameterAmount = 1) {
            //if (minParameterAmount < 0 || maxParameterAmount < minParameterAmount) throw new Exception();
            MinParameterAmount = minParameterAmount;
            MaxParameterAmount = maxParameterAmount;
        }

        public static ParameterAccept Mandatory = new ParameterAccept(1, 1);
        public static ParameterAccept Optional = new ParameterAccept(0, 1);
        public static ParameterAccept AtleastOne = new ParameterAccept(1, int.MaxValue);
        public static ParameterAccept Any = new ParameterAccept(0, int.MaxValue);
    };
}
