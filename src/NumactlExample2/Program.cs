using System;
using ArgParser;

namespace Numactl
{
    class Parser : ParserBase
    {
        public NoValueOption show = new(new string[] { "S", "show" }, "Show current NUMA policy.");
        public NoValueOption hardware = new(new string[] { "H", "hardware" }, "Print hardware configuration.");
        
        // Name of param (e.g. <node>) cannot be set for generating help
        public IntOption preferred = new(new string[] { "p", "preferred" }, 
            "Prefer memory allocations from given node.", parameterAccept: ParameterAccept.Mandatory);

        public StringOption interleave = new(new string[] { "i", "interleave" },
            "Interleave memory allocation across given nodes.", ParameterAccept.Mandatory);

        public StringOption membind = new(new string[] { "m", "membind" },
            "Allocate memory from given nodes only.", ParameterAccept.Mandatory);

        public StringOption physcpubind = new(new string[] { "C", "physcpubind" },
            "Run on given CPUs only.", ParameterAccept.Mandatory);

        public StringArgument command = new("command", "", ParameterAccept.Mandatory);
        public StringArgument args = new("args", "", ParameterAccept.Any);

        protected override IArgument[] GetArgumentOrder() => new IArgument[] { command, args };
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            try
            {
                parser.Parse(args);
            } catch (ParseException)
            {
                Console.WriteLine("Invalid arguments");
                return;
            } catch (ParserCodeException)
            {
                Console.WriteLine("Invalid parser definition");
                return;
            }

            // What IsSet actually does?
            // Parser could maybe somehow support exlusivness of specific options to avoid the big condition below.
            if ((parser.preferred.GetValue() != null && (parser.interleave.IsSet || parser.membind.IsSet)) ||
                (parser.interleave.IsSet && (parser.preferred.GetValue() != null || parser.membind.IsSet)) ||
                (parser.membind.IsSet && (parser.interleave.IsSet || parser.preferred.GetValue() != null)))
            {
                Console.WriteLine("Options are in conflict");
                return;
            }

            if (parser.show.GetValue())
            {
                PrintPolicy(parser);
            }
            else if (parser.hardware.GetValue())
            {
                PrintHardwareConfiguration();
            }
            else if (parser.command.GetValue() != null)
            {
                Console.WriteLine("Running command: " + parser.command.GetValue());
                // How to access all args if we do not know the final number of them?
                Console.WriteLine("Command args: " + parser.args.GetValue());
                PrintPolicy(parser);
            } else
            {
                Console.WriteLine(parser.GenerateHelp());
            }
        }

        private static void PrintPolicy(Parser parser)
        {
            Console.WriteLine("policy: default");
            Console.WriteLine("preferred node: " + parser.preferred.GetValue());
            Console.WriteLine("physcpubind: " + parser.physcpubind.GetValue());
            Console.WriteLine("cpubind: 0 1");
            Console.WriteLine("nodebind: " + parser.preferred.GetValue());
            Console.WriteLine("membind: " + parser.membind.GetValue());
        }

        private static void PrintHardwareConfiguration()
        {
            Console.WriteLine("available: 2 nodes (0-1)");
            Console.WriteLine("node 0 cpus: 0 2 4 6 8 10 12 14 16 18 20 22");
            Console.WriteLine("node 0 size: 24189 MB");
            Console.WriteLine("node 0 free: 18796 MB");
            Console.WriteLine("node 1 cpus: 1 3 5 7 9 11 13 15 17 19 21 23");
            Console.WriteLine("node 1 size: 24088 MB");
            Console.WriteLine("node 1 free: 16810 MB");
            Console.WriteLine("node distances:");
            Console.WriteLine("node   0   1");
            Console.WriteLine("  0:  10  20");
            Console.WriteLine("  1:  20  10");
        }
    }
}
