using System;

namespace ArgParser
{
    /// <summary>
    /// Thrown on Parse error (e.g. the given arguments cannot be parsed or
    /// aren't valid according to the specification).
    /// </summary>
    public class ParseException : Exception { };

    /// <summary>
    /// Thrown on invalid implementation of specification of the parser.
    /// </summary>
    public class ParserCodeException : Exception { };
}
