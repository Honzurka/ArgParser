using System;
using ArgParser;

namespace Time
{
    class Options : IOptions
    {
        public string Delimiter => "--";

        public StringOption format = new StringOption(
            new OptionSettings(new string[] { "f", "format" }, "Specify output format.", ParameterAccept.Mandatory));
        public BoolOption portability = new BoolOption(
            new OptionSettings(new string[] { "p", "portability" }, "Use the portable output format.", ParameterAccept.None, false));

        public StringOption output = new StringOption(
            new OptionSettings(new string[] { "o", "output" }, "Do not send the results to stderr, but overwrite the specified file.", ParameterAccept.Mandatory));
        // vazba na output ???
        public BoolOption append = new BoolOption(
            new OptionSettings(new string[] { "a", "append" }, "(Used together with -o.) Do not overwrite but append.", ParameterAccept.None, false));
        public BoolOption verbose = new BoolOption(
            new OptionSettings(new string[] { "v", "verbose" }, "Give very verbose output about all the program knows about.", ParameterAccept.None));
        
        public BoolOption help = new BoolOption(
            new OptionSettings(new string[] { "help" }, "Print a usage message on standard output and exit successfully", ParameterAccept.None));
        public BoolOption version = new BoolOption(
            new OptionSettings(new string[] { "V", "version" }, "Print version information on standard output, then exit successfully.", ParameterAccept.None));
    }
    class Program
    {
        /*
        time [options] command [arguments...]

        GNU Options
            -f FORMAT, --format=FORMAT
                Specify output format, possibly overriding the format specified
                in the environment variable TIME.
            -p, --portability
                Use the portable output format.
            -o FILE, --output=FILE
                Do not send the results to stderr, but overwrite the specified file.
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
        const string version = "1.0";

        static void Main(string[] args)
        {
            Options specification = new Options();
            Parser parser = new Parser(specification);
            parser.Parse(args);
            if (specification.help.Value) {
                Console.WriteLine(parser.GenerateHelp());
            } else if (specification.version.Value) {
                Console.WriteLine("Current version: " + version);
            }
            else {
                ProgramMain(specification);
            }
        }

        static void ProgramMain(Options parsedSpec) {
            var format = parsedSpec.format.Value;
            //...
        }
    }
}
