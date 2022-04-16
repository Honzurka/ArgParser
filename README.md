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
    - The fields defined in the parser should themselves inherit from `ArgumentBase<T>` or `OptionBase<T>`, however, the pre-defined classes such as `IntOption` and `IntArgument` should cover most basic use-cases (see [Simple example](#simple-example)).
	- Difference between option and argument is that options are prefixed by `-` or `--` and arguments are plain arguments, therefore without delimiter.
	- `ParameterAccept` describes number of parsed parameters. By default min and max number of parsed params is set to 1.
	- `GetArgumentOrder` is used to define order of plain arguments. It should contain references to the parsed fields of the Parser.
	- *Only one* Argument of a parser can have variable acceptable count of arguments (`ParameterAccept` where its minimum and maximum value are not the same). Otherwise `ParserCodeException` is thrown.
2. User calls `ParserBase.Parse()`. The parser will identify all the necesarry fields it contains and fill them with the parsed values.
    - Throws `ParseException` if parsed arguments don't satisfy declared option fields
    - Throws `ParserCodeException` if the class doesn't conform to the parser requirements
        - Fields not inheriting from predefined classes (descendants of `OptionBase<T>` or `ArgumentBase<T>`)
        - Defining argument fields without overriding `GetArgumentOrder`
3. User obtains parsed values from the fields by calling a method `GetValue(int idx)` on them.

# Simple example
```csharp

class Parser : ParserBase
{
	public BoolOption BoolOpt = new(new string[] { "b", "bool" }, "bool description", isMandatory: true);
	public NoValueOption Help = new(new string[] { "h", "help", "?" }, "show help");

	public StringArgument Files = new("files", "files to read", ParameterAccept.Any);
	public IntArgument Number = new("number", "number description", minValue: 0, defaultValue: 42);

	protected override ArgumentBase[] GetArgumentOrder() => new ArgumentBase[]{ Files, Number };
}

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

```

# Building instructions
## Example program
The repository contains a dotnet solution.
With `dotnet` installed, simply execute `dotnet build` in the repo directory,
then to run execute `dotnet run --project src/Time/Time.csproj`.

## Including library in your project
To use the library you only need the assembly from `lib/ArgParser/bin/(Debug|Release)/ArgParser/net5.0/ArgParser.dll`.

For dotnet 5.0, you can copy the ArgParser dll to some local project folder (e.g. `include/`),
then, inside your including project .csproj file, add this reference (include your path to the dll if you decided on a custom one):
```xml
<ItemGroup>
  <Reference Include="ArgParser">
    <HintPath>include/ArgParser.dll</HintPath>
  </Reference>
</ItemGroup>
```