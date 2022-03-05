/*

    - array of command class instances, each having a mandatory parameter ? --------------------------------- ??
    - or instead of defining options in constructor, use methods
      c.AddMandatoryOption(o) and c.AddOptionalOption(o)
    - we could also leave it at a binary enumeration, just to give symbols
      to the API -> Option.OptionAccept.Mandatory

*/

enum OptionAccept { Mandatory, Optional }; // 2 hodnoty => mohl by byt bool?
// maybe put parameterAccept inside Iparam
enum ParameterAccept { Mandatory, Optional, None };

interface IParamType<T> {
    // public object GetValue();
    internal /*public*/ void IsValid(string value) {} //nemela bych krome validace vracet i pretypovanou hodnotu?
}

class IntParam : IParamType<int> {
    public IntParam(int minValue = Int.MinValue, int maxValue = Int.MaxValue, int? default = null) {
      
    }
}
class BoolParam : IParamType {
    public BoolParam(bool? default = null)
}
class StringParam : IParamType {
    public StringParam(string? default = null) { }
}
class EnumParam : IParamType {
    string[] domain;
    public EnumParam(string[] domain, string? default = null);
}

interface VarRef { T GetValue(); }

interface Vars { T GetValue();  }

class ParsedResult {
    VarRef Get(string name) {

    }
}


interface intVar : Vars { int GetValue(); }

class ArgParser
{
    private static class Storage<T> { static List<VarRef<T>> stuff; }

    //List<object> varRefs;

    public ArgParser(string delimiter = "--") {}

    public void AddOption(string[] names, string description, OptionAccept optionAccept,
      ParameterAccept parameterAccept, IParamType<T> type) {
          VarRef<T> toSave;

          Storage<T>
    }

    // 
    public void AddPlainArgument(string name, string description, IParamType type) {}



    public IReadOnlyDictionary<string name, VarRef value> Parse() {} //mozna by bylo lepsi vracet tridu s Get(name) metodou
    //public T Get<T>(string name) {}
}



class Settings<T> {
    string[] names;
    T[] value;

}

class Options : IOption { //uzivatelovo
    public settings<T> Verbose = new Settings<bool>("verbose", ...);// -> GetValue(defaultIdx = 0)¨

    public PlainArg[] args = {

    };
}

Options o = new Options();
ParserV2.parse(options, args);
// vs
PArgParser p = new P().Parse(args);




{
    class P : ParserV2 {
        Option<int> num = new Option<int>("num", ...);
        Option<string> form;
    }

    new P().Parse(args);
}

// field in getAllFields
    // type.GetField(nameof(field) + Settings ) as MemberInfo

static class ParserV2 {
    public void Parse(); // reflection
    
}

/*

*/


class Example
{
    static void Main(string[] args)
    {
        // instantiate the required classes into the required form
        // parse the args

        new ArgParser {
            new (
                [ "--format", "-f" ],
                true
            )
        }.AddOption(["--verbose", "-v"],).AddArgument()



        // 1.
        // klient vytvori strukturu pro vysledek
        // knihovna vytvori -||-
        // dict = p.parse(args)
        // p.get("-a")
        // dict.get("--ahoj")

        // 2.
        // dictionary
          // ziskani plain argumentu -> skrze jmeno
        // callback
          // -zalezi na poradi optionu
          // -nevhodne napr. pro nastaveni promenne
          // spise ne
        // named var reference
          // -prilis mnoho promennych

        bool myVerbose;
        parser.AddOption(verbose, () => { myVerbose = true })


        var format = parser.AddOption("fprm")
        parser.parse();
        format.Value();

        parser.GetOptionValue("--format");

        parserResult.result


        string format = "format";
        parser.AddOption(format);
        parser.Get(format);


        /*

        cp SOURCES... DIRECTORY

        time [options] command [arguments...]

        GNU Options
            -f FORMAT, --format=FORMAT
                Specify output format, possibly overriding the format specified
                in the environment variable TIME.
            -p, --portability
                Use the portable output format.
            -o FILE, --output=FILE
                Do not send the results to stderr, but overwrite the specified file.

                output = Console.Error
                () => output = {new Writer(file);}
            -a, --append
                (Used together with -o.) Do not overwrite but append.
            -v, --verbose
                Give very verbose output about all the program knows about.

        GNU Standard Options
            --help Print a usage message on standard output and exit successfully.
            -V, --version
                Print version information on standard output, then exit successfully.
            --     Terminate option list.
    
        */
    }
}
