using System;

namespace ArgParser
{
	/// <summary>
	/// Thrown on Parse error.
	/// </summary>
	public class ParseException : Exception { }; //info o spatnem fieldu

	/// <summary>
	/// Thrown on bad implementation of specification.
	/// </summary>
	public class ParserCodeException : Exception { };
}
