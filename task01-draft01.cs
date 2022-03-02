/*

#Short option
A command-line argument that starts with a single dash, followed by a single character (e.g. “-v")

#Long option
A command-line argument that starts with two dashes, followed by one or more characters (e.g. “--version")

#Option
Short option or Long option

#Option parameter
A command-line argument that follows an option (if the option is defined to accept parameters by the user of your library)

#Plain argument
A command-line argument that is neither an option nor an option parameter.

#Delimiter
A command-line argument consisting of two dashes only, i.e. --. Any subsequent argument is considered to be a plain argument

For example, in the following command

cmd -v --version -s OLD --length=20 -- -my-file your-file
there are short options v and s, long options version and length, the OLD and 20 values represent arguments to -s and --length options, respectively. The -my-file and your-file arguments after the -- delimiter are plain arguments.


    - we might have a class for arg parsing. Should it be static? It's not
      going to be instantiated more than once a program anyway.

    - default values for parameters! Simply solves binary parameters, for other
      uses of absence simply use Nullable<T> and null.
      Specify by user that it's default

    - solve duplicit options? might be just part of implementation...

- specify what options the client program accepts, which of them are optional
  and which of them are mandatory, verify the actual arguments passed to the
  client program conform to this specification.


    // names = [] => plain argument

    - array of command class instances, each having a mandatory parameter ?
    - or instead of defining options in constructor, use methods
      c.AddMandatoryOption(o) and c.AddOptionalOption(o)
    - we could also leave it at a binary enumeration, just to give symbols
      to the API -> Option.OptionAccept.Mandatory

- define synonyms (at the very least 1:1 between short and long options, but
  ideally in a more general way).

    - include dashes ? string[] synonyms || string[] longOptions, shortOptions
    - again, methods .AddLongSynonym(o) and .AddShortSynonym(o), but we might
      want to ensure a parameter has atleast one option, i guess...?

    - addParam(['-f', '--force',...]), else throw error
    - autodetect based on if short options can have more than one letter

- specify, whether an option may/may not/must accept parameters, verify that
  the actual arguments passed to the client program conform to this
  specification.

    - since this is enumerated, it can be specified in the constructor as
      Option.ParameterAccept.Mandatory
    - or, again, have multiple classes inherited from one class
      OptionWithNoParam, OptionWithOptionalParam, OptionWithMandatoryParam

    - inheritance based classes
  python: nargs= ? * + 0 [1 2 3 ...]
  - constructor / method overload


- specify types of parameters, verify that the actual arguments passed to the
  client program conform to this specification. At the very least the library
  has to distinguish between string parameters, integral parameters (with
  either or both: lower and upper bound), boolean parameters, and string
  parameters with fixed domain (enumeration).

    delegate f 
    - have builtin functions like IsInteger(p), IsInRange(l,u,p), etc.,
      then define some anonymous function (maybe?)
      (string p) => IsInteger(p) && isInRange(0, 65536, p);

    if f(string)

    - 

- allow adapting the client program behavior or configuration in response to
  the options and arguments passed on the command line.

- access the values of all plain arguments. The delimiter may be omitted unless
  a plain argument starts with -.

- document the options and to present the documentation to the user in form of
  a help text.

    - simple, hopefully
*/

enum OptionAccept { Mandatory, Optional };
// maybe put parameterAccept inside Iparam
enum ParameterAccept { Mandatory, Optional, None };

interface IParam {
    // public object GetValue();
    public void IsValid(string value) {}
}

class IntParam : IParam {
    public IntParam(int minValue = Int.MinValue, int maxValue = Int.MaxValue, int? default = null) {
      
    }
}
class BoolParam : IParam {
    public BoolParam(bool? default = null)
}
class StringParam : IParam {
    public StringParam(string? default = null) { }
}
class EnumParam : IParam {
    string[] domain;
    public EnumParam(string[] domain, string? default = null);
}

class ArgParser
{
    public ArgParser(string delimiter = "--") {}

    public void AddOption(string[] names, string description, OptionAccept optionAccept,
      ParameterAccept parameterAccept, IParam type) {
    }

    // 
    public void AddPlainArgument(string name, string description, IParam type) {}



    public IReadOnlyDictionary<string name, object value> Parse() {}
    //public T Get<T>(string name) {}
}

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
