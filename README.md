- project created as a part of school subject [Best Practices in Programming](https://d3s.mff.cuni.cz/teaching/nprg043/)

# Library Description
The ArgParser library allows for parsing command line arguments through a
user-defined class which enables easy access to parsed values.

## key concepts
- The library relies on C# reflection. Which allows accessing parsed values
  with correct type and through pre-defined field.
- Possibility of custom Options or Arguments by inheriting from `OptionBase<T>`
  or `ArgumentBase<T>`
- Short option vs long option
    - short option (uses `-` with parameter name) has name consisting of 1 char
      only
    - long option (uses `--` with parameter name) has name consisting of at
      least 2 chars
- Parsed option/arg types are restricted using inheritance: descendants of
  `ArgumentBase<T>` and `OptionBase<T>`. Which limits possible types that could
  be parsed.
- Parsed options should precede plain arguments.
- Variadic option consumes as many parameters as possible. Could be interrupted
  by another option or a delimiter.

## use cases
1. The user will define the accepted arguments as fields in a class inherited
   from `ParserBase`.
	- The fields defined in the parser are descendants of `ArgumentBase<T>` or
	  `OptionBase<T>`. If possible use predefined classes such as `IntOption`
	  and `IntArgument`. If you require a non-standard type see
	  [Extension](#Extending-the-library) for more information.
    - Difference between option and argument is that options are prefixed by
      `-` or `--` and arguments are plain arguments, therefore without
      delimiter.
    - `ParameterAccept` describes number of parsed parameters. By default min
      and max number of parsed params is set to 1.
    - `GetArgumentOrder` is used to define order of plain arguments. It should
      contain references to the parsed fields of the Parser.
    - *Only one* Argument of a parser can have variable acceptable count of
      arguments (`ParameterAccept` where its minimum and maximum value are not
      the same). Otherwise `ParserCodeException` is thrown.
1. User calls `ParserBase.Parse()`. The parser will identify all the necesarry
   fields it contains and fill them with the parsed values.
    - Throws `ParseException` if parsed arguments don't satisfy declared option
      fields
    - Throws `ParserCodeException` if the class doesn't conform to the parser
      requirements
        - Fields not inheriting from predefined classes (descendants of
          `OptionBase<T>` or `ArgumentBase<T>`)
        - Defining argument fields without overriding `GetArgumentOrder`
2. User obtains parsed values from the fields by calling a method `GetValue(int idx)` on them.

# Simple example
```csharp
using System;
using ArgParser;

var parser = new Parser();
try {
	parser.Parse(args);
} catch (ParseException) {
	if (parser.Help.GetValue()) {
		Console.Write(parser.GenerateHelp());
	} else {
		Console.Error.WriteLine("Error while parsing.");
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
```

# Building instructions
## Example program
The repository contains a dotnet solution. With `dotnet` installed, simply
execute `dotnet build` in the repo directory, then to run execute
`dotnet run --project src/Time/Time.csproj`.

## Including library in your project
To use the library you only need the assembly from
`lib/ArgParser/bin/(Debug|Release)/ArgParser/net5.0/ArgParser.dll`.

For dotnet 5.0, you can copy the ArgParser dll to some local project folder
(e.g. `include/`), then, inside your including project .csproj file, add this
reference (include your path to the dll if you decided on a custom one):
```xml
<ItemGroup>
	<Reference Include="ArgParser">
		<HintPath>include/ArgParser.dll</HintPath>
	</Reference>
</ItemGroup>
```

## Running Tests
With `dotnet` installed execute `dotnet test` from project root folder.

## Generating documentation
With `doxygen` installed execute `doxygen Doxyfile`. After documentation
is generated it can be viewed at
[Documentation/html/index.html](./Documentation/html/index.html)

# Extending the library

1. Implement `IParsable<T>`
    - `Parse(string[])` and `GetValue(int)` methods are necessary for correct
      parsing
    - `TypeAsString` and `ConstraintsAsString` are used for generating help
      text 2. Inherit from `OptionBase<T>` / `ArgumentBase<T>` / both using the
      implemented IParsable.
    - Pass instance of `IParsable<T>` into constructor

# Design ideas
## Defining synonyms
When adding option we add all its synonyms. Adding synonyms later would be more
error-prone and it would also be harder to use.

## Parameter
We use `ParameterAccept` structure to hold range of parameters.
Compared to using predefined enums this allows great flexibility. 

## Parameter type
We have created all possible types using inheritance. In comparison with
generic type this allows us to limit parsed types. This solution is extensible
because user can defined his own types through SPI. It also allows us to
associate restrictions with specific type.

## Accessing parsed values
We thought about multiple ways of accessing values.

1. Calling `result.Get(OptionName)`. Option is search by string name which is
error prone. Get method can't return exact type it would probably have to
return `object` that the user would have to cast to specific type.

2. Callbacks used for saving values. This allows great flexibility because user
can specify excatly what he wants. Possible negative is that option order might
change behavior. Lots of code is required for simple actions compared to other
methods.

3. Named variable reference. Method `AddOption()`, for adding option, would
return reference to parsed result. 
    - This solution is more type-safe because we don't have to access option
      through string name.
    - It also allows us to return values with correct type.
    - However this would lead to too many unstructured variables.

4. User specifies options through predefined class - current solution. This
solution is similar to Named variable reference but it comes with structure for
parsed values.
