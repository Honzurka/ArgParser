# Library Description

The ArgParser library allows for parsing command line arguments through a user-defined class.
todo...?

## key concepts

The library relies on C# reflection. The user will define the accepted arguments as fields
in a class inherited from `ParserBase`, and upon calling `ParserBase.Parse()`, the parser
will identify all the necesarry fields it contains and fill them with the parsed values.
Finally, the user can obtain the parsed values from the fields by calling a method
`GetValue(int idx)` on them.

The fields defined in the parser should themselves inherit from `ArgumentBase<T>` or `OptionBase<T>`,
however, the intended use-case is to use pre-defined classes such as `IntArgument` (see [#Simple example]...?).

## use cases

todo...

# Simple example

todo...

# Building instructions

The repository contains a dotnet solution.
With `dotnet` installed, simply execute `dotnet build` in the repo directory,
then to run execute `dotnet run --project src/Time/Time.csproj`.
