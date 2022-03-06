using System;
using ArgParser;

namespace SimpleExample
{
	class Parser : ParserBase
	{
		public BoolOption boolOpt = new BoolOption(new string[] { "b", "bool" }, "bool description", isMandatory: true);
		public NoValueOption help = new NoValueOption(new string[] { "help" }, "show help");

		public IntArgument number = new IntArgument("number", "number description", minValue: 0, defaultValue: 42,
			parameterAccept: ParameterAccept.Mandatory);
		public StringArgument file = new StringArgument("file", "file description");

		protected override IArgument[] GetArgumentOrder() => new IArgument() { number, file };
	}

	class Program
	{
		static void Main(string[] args)
		{
			Parser parser = new Parser();
			parser.Parse(args);

			if(parser.help.GetValue())
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
