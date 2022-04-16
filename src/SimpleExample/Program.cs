using System;
using ArgParser;

namespace SimpleExample
{
	class Parser : ParserBase
	{
		public BoolOption BoolOpt = new(new string[] { "b", "bool" }, "bool description", isMandatory: true);
		public NoValueOption Help = new(new string[] { "h", "help", "?" }, "show help");

		public StringArgument Files = new("files", "files to read", ParameterAccept.Any);
		public IntArgument Number = new("number", "number description", minValue: 0, defaultValue: 42);

		protected override IArgument[] GetArgumentOrder() => new IArgument[]{ Files, Number };
	}

	class Program
	{
		static void Main(string[] args)
		{
			var parser = new Parser();

			Console.WriteLine(parser.GenerateHelp());
			return;

			try
			{
				parser.Parse(args);
			}
			catch (ParseException)
			{
				Console.Error.WriteLine("Passed arguments doesn't conform to program specification. See help for more explanation.");
				Environment.Exit(1);
			}
			if (parser.Help.GetValue())
			{
				Console.WriteLine(parser.GenerateHelp());
			}
			else
			{
				if (parser.BoolOpt.GetValue() != null) Console.WriteLine($"boolOpt = ${parser.BoolOpt.GetValue()}");
				int i = 0;
				while (parser.Files.GetValue(i) != null) {
					// equivalent to while (i < parser.files.ParsedParameterCount)
					Console.WriteLine($"file{i} = {parser.Files.GetValue(i)}");
					i++;
				}
			}
		}
	}
}
