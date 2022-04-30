using System;

namespace ArgParser
{
	/// <summary>
	/// Thrown when given arguments cannot be parsed or validated.
	/// 
	/// This exception is thrown when the program user passes arguments that
	/// don't conform to the parser specification (i.e. unparsable values,
	/// values not in specified range, etc.).
	/// </summary>
	public class ParseException : Exception
	{
		public ParseException(string? message) : base(message) { }
	};

	/// <summary>
	/// Thrown on invalid implementation of specification of the parser.
	/// 
	/// This exception is thrown when the specification of the parser itself
	/// is invalid (i.e. options with conflicting names, unrecognised arguments
	/// in <see cref="ParserBase.GetArgumentOrder">GetArgumentOrder()</see>,
	/// etc.). This exception is not meant to be caught, as it implies the
	/// parser itself is working improperly.
	/// </summary>
	public class ParserCodeException : Exception
	{
		public ParserCodeException() : base() { }

		public ParserCodeException(string? message) : base(message) { }
	};
}
