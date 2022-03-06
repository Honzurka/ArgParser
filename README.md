# Library Description
The ArgParser library allows for parsing command line arguments through a user-defined class which enables easy access to parsed values.

## key concepts
- The library relies on C# reflection. Which allows accessing parsed values with correct type and through pre-defined field.
- Possibility of custom Options or Arguments by inheriting from `OptionBase<T>` or `ArgumentBase<T>`
- Short option vs long option
    - short option (uses `-` with parameter name) has name consisting of 1 char only
    - long option (uses `--` with parameter name) has name consisting of at least 2 chars
- Parsed option/arg types are restricted using inheritance: descendants of `ArgumentBase<T>` and `OptionBase<T>`. Which limits possible types that could be parsed.

## use cases
1. The user will define the accepted arguments as fields in a class inherited from `ParserBase`.
    - The fields defined in the parser should themselves inherit from `ArgumentBase<T>` or `OptionBase<T>`, however, the intended use-case is to use pre-defined classes such as `IntOption` and `IntArgument` (see [Simple example](#simple-example)).
2. User calls `ParserBase.Parse()`. The parser will identify all the necesarry fields it contains and fill them with the parsed values.
    - Throws `ParseException` if parsed arguments don't satisfy declared option fields
    - Throws `ParserCodeException` if the class doesn't conform to the parser requirements
        - Fields not inheriting from predefined classes (descendants of `OptionBase<T>` or `ArgumentBase<T>`)
        - Defining argument fields without overriding `GetArgumentOrder`
3. User obtains parsed values from the fields by calling a method `GetValue(int idx)` on them.

# Simple example
```C#
namespace SimpleExample
{
	class Parser : ParserBase
	{
		public BoolOption boolOpt = new(new string[] { "b", "bool" }, "bool description", isMandatory: true);
		public NoValueOption help = new(new string[] { "help" }, "show help");

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
```





# Building instructions
## Example program
The repository contains a dotnet solution.
With `dotnet` installed, simply execute `dotnet build` in the repo directory,
then to run execute `dotnet run --project src/Time/Time.csproj`.

## Including library in your project
todo