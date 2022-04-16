using System;

namespace ArgParser
{
    /// <summary>
    /// Thrown on Parse error (e.g. the given arguments cannot be parsed or
    /// aren't valid according to the specification).
    /// </summary>
    public class ParseException : Exception
    {
        public ParseException(string? message) : base(message) { }
    };

    /// <summary>
    /// Thrown on invalid implementation of specification of the parser.
    /// </summary>
    public class ParserCodeException : Exception
    {
        public ParserCodeException() : base() { }

        public ParserCodeException(string? message) : base(message) { }
    };
}
