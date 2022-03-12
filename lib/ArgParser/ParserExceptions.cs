using System;

namespace ArgParser
{
	/// <summary>
	/// Thrown on Parse error.
	/// </summary>
	public class ParseException : Exception { }; //todo: pridat info o spatnem fieldu / spatnych fieldech

	/// <summary>
	/// Thrown on bad implementation of specification.
	/// </summary>
	public class ParserCodeException : Exception { };
}
