using System;
using ArgParser;

var parser = new Parser();
try {
	parser.Parse(args);
} catch (ParseException) {
	if (parser.Help.GetValue()) {
		Console.Write(parser.GenerateHelp());
	} else {
		Console.Error.WriteLine("Arguments don't conform to program specification. Type --help for help.");
	}
	Environment.Exit(1);
}
if (parser.Help.GetValue()) {
	Console.WriteLine(parser.GenerateHelp());
} else {
	if (parser.BoolOpt.GetValue() != null) {
		Console.WriteLine($"boolOpt = {parser.BoolOpt.GetValue()}");
	}
	for (int i = 0; parser.Files.GetValue(i) != null; i++) {
		Console.WriteLine($"file[{i}] = {parser.Files.GetValue(i)}");
	}
	Console.WriteLine($"number = {parser.Number.GetValue()}");
}

class Parser : ParserBase
{
	public BoolOption BoolOpt = new(new string[] { "b", "bool" }, "Pass some bool here");
	public NoValueOption Help = new(new string[] { "h", "help", "?" }, "Display help");

	public StringArgument Files = new("strings", "Accepts any amount of strings", ParameterAccept.Any);
	public IntArgument Number = new("number", "Pass some number here", minValue: 0, defaultValue: 42);

	protected override IArgument[] GetArgumentOrder() => new IArgument[] { Files, Number };
}
