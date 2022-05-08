using System;
using ArgParser;

namespace Time
{
    class Parser : ParserBase
    {
        public StringOption format = new (new string[] { "f", "format" },
            "Specify output format.");
        public NoValueOption portability = new(new string[] { "p", "portability" },
            "Use the portable output format.");
        public StringOption output = new(new string[] { "o", "output" },
            "Do not send the results to stderr, but overwrite the specified file.");
        public NoValueOption append = new(new string[] { "a", "append" },
            "(Used together with -o.) Do not overwrite but append.");
        public NoValueOption verbose = new(new string[] { "v", "verbose" },
            "Give very verbose output about all the program knows about.");
        
        public NoValueOption help = new(new string[] { "help" },
            "Print a usage message on standard output and exit successfully");
        public NoValueOption version = new(new string[] { "V", "version" },
            "Print version information on standard output, then exit successfully.");

        public StringArgument command = new("command",
            "Executed command.");
        public StringArgument arguments = new("arguments",
            "Arguments provided to the executed command.", ParameterAccept.Any);

        protected override IArgument[] GetArgumentOrder() => new IArgument[] { command, arguments };
    }
    class Program
    {
        const string version = "1.0";

        static void Main(string[] args)
        {
            var parser = new Parser();
            try {
                parser.Parse(args);
            } catch(ParseException e) {
                if (parser.help.GetValue()) {
                    Console.Write(parser.GenerateHelp());
                } else {
                    Console.Error.WriteLine(e.Message);
                }
                Environment.Exit(1);
            }
            if (parser.help.GetValue()) {
                Console.WriteLine(parser.GenerateHelp());
            } else if (parser.version.GetValue()) {
                Console.WriteLine("Current version: " + version);
            } else {
                ProgramMain(parser);
            }
        }

        static void ProgramMain(Parser parser) {
            var format = parser.format.GetValue();
            //...
        }
    }
}
