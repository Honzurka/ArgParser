using System;
using ArgParser;

namespace Time
{
    class Parser : ParserBase
    {
        public StringOption format = new StringOption(
            new string[] { "f", "format" }, "Specify output format.");
        public NoValueOption portability = new NoValueOption(
            new string[] { "p", "portability" }, "Use the portable output format.");
        public StringOption output = new StringOption(
            new string[] { "o", "output" }, "Do not send the results to stderr, but overwrite the specified file.");
        
        public NoValueOption append = new NoValueOption(
            new string[] { "a", "append" }, "(Used together with -o.) Do not overwrite but append.");
        public NoValueOption verbose = new NoValueOption(
            new string[] { "v", "verbose" }, "Give very verbose output about all the program knows about.");
        
        public NoValueOption help = new NoValueOption(
            new string[] { "help" }, "Print a usage message on standard output and exit successfully");
        public NoValueOption version = new NoValueOption(
            new string[] { "V", "version" }, "Print version information on standard output, then exit successfully.");


		
		//protected override IArgument[] GetParameterOrdering() => new IArgument[] { a, B };
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
            Parser parser = new Parser();
            parser.Parse(args);

            if (parser.help.Value()) {
                Console.WriteLine(parser.GenerateHelp());
            } else if (parser.version.Value()) {
                Console.WriteLine("Current version: " + version);
            }
            else {
                ProgramMain(parser);
            }
        }

        static void ProgramMain(Parser parser) {
            var format = parser.format.Value();
            //...
        }
    }
}
