using System;
using ArgParser;

namespace SimpleExample
{
	class Parser : ParserBase
	{
		public BoolOption boolOpt = new(new string[] { "b", "bool" }, "bool description", isMandatory: true);
		public NoValueOption help = new(new string[] { "h", "help", "?" }, "show help");

		public IntArgument number = new("number", "number description", minValue: 0, defaultValue: 42,
			parameterAccept: ParameterAccept.Mandatory);
		public StringArgument file = new("file", "file description");

		protected override ArgumentBase[] GetArgumentOrder() => new ArgumentBase[]{ number, file };
	}

	class Program
	{
		static void Main(string[] args)
		{
			var parser = new Parser();
			try
			{
				parser.Parse(args);
			}
			catch (ParseException)
			{
				Console.Error.WriteLine("Passed arguments doesn't conform to program specification. See help for more explanation.");
				Environment.Exit(1);
			}
			if (parser.help.GetValue())
			{
				Console.WriteLine(parser.GenerateHelp());
			}
			else
			{
				if (parser.boolOpt.GetValue() != null) Console.WriteLine($"boolOpt = ${parser.boolOpt.GetValue()}");
				if (parser.file.GetValue() != null) Console.WriteLine($"file = ${parser.file.GetValue()}");
			}
		}
	}
}
