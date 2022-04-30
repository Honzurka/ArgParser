---
title:
- Implementace parsovací knihovny v C#
author:
- Jan Fürst a Filip Štrobl
theme:
- Copenhagen
header-includes:
- \newcommand{\btiny}{\begin{tiny}}
- \newcommand{\etiny}{\end{tiny}}
- \newcommand{\bsmall}{\begin{small}}
- \newcommand{\esmall}{\end{small}}
---

# API review
1. Deklarativni API - uzivatel specifikuje (zdedi) tridu s optiony / argumenty
2. Zavolani `Parse()`
3. Pristup k naparsovanym hodnotam skrze specifikovanou tridu

\bsmall
```csharp
class Parser : ParserBase
{
	public StringArgument Str = new("Str", "String arg");
}
var parser = new Parser();
try {
    parser.Parse(args);
catch (ParseException e)
{
    Console.Error.WriteLine(e.Message);
    Environment.Exit(1);
}
string str = parser.Str.GetValue();
```
\esmall

# Klicove koncepty
- reflection
- nepouzivame atributy, jen dedicnost
    - `IOption` -> `OptionBase<T>` -> `{T}Option` + `Parsable{T}`
        - stejna hierarchie pro Argumenty (Parsable je sdilene)
        - (interni) `IOption` - usnadnuje pristup k optionum skrze reflection
        - `OptionBase<T>` vs `ArgumentBase<T>`: obsahuji specificke informace o optionu/argumentu
        - `{T}Option` - Omezeni a pojmenovani specifickych typu optionu
    - `IParsable<T>` -> `Parsable{T}`
        - `<T>` zajistuje type-safety pri pristupu k hodnote
        - Obsahuje implementaci parsovani sdilenou mezi Option a Argument

# Highlighty designu
- generovani help textu
    - vyuzivame hierarchie dedicnosti
    - kazdy potomek IOption/IArgument zodpovida za svuj vypis
    ```C#
    public string GenerateHelp()
    {
        StringBuilder result = new();
        result.Append(GetUsageExampleLine());
        result.Append(GetOptionHelp());
        result.Append(GetPlainArgHelp());
        return result.ToString();
    }
    ```

# Highlighty designu
- pouziti LINQ

\bsmall
```C#
void CheckDuplicates(IArgument[] orderedPlainArgs)
{
    var duplicates = orderedPlainArgs
        .GroupBy(x => x)
        .Where(g => g.Count() > 1);
    if (duplicates.Any()) {
        throw new ParserCodeException("Duplicate elements");
    }
}
```
\esmall

# Unit testy
- sady testu pokryvaly siroke spektrum kvality
- nektere testy predpokladaly funkcionalitu kterou jsme nespecifikovali
    - nektere testy jsme smazali a upresnili jsme dokumentaci
- konkretne
    - vice variadic argumentu
        - podporujeme max 1
        - dalo by se implementovat (hladove)
    - nejednoznacnost variadic optionu a naslednych argumentu
        - nase reseni: optiony berou parametry hladove

## Zmeny API
- ovlivnene testy a rozhodovanim behem implementace
- pridani `IParsable<T>`
- interleave optionu a argumentu
    - puvodne jsme nechteli implementovat
    - rozsireni bylo snadne
