using System;
using ArgParser;

namespace ReviewExample
{
    static class PreparedMessages
    {
        public static void PrintNotGeneratedHelp()
        {
            Console.WriteLine("usage: numactl [--interleave= | -i <nodes>] [--preferred= | -p <node>]");
            Console.WriteLine("               [--physcpubind= | -C <cpus>] [--membind= | -m <nodes>]");
            Console.WriteLine("               command args ...");
            Console.WriteLine("       numactl [--show | -s]");
            Console.WriteLine("       numactl [--hardware | -H]");
            Console.WriteLine();
            Console.WriteLine("<nodes> is a comma delimited list of node numbers or A-B ranges or all.");
            Console.WriteLine("<cpus> is a comma delimited list of cpu numbers or A-B ranges or all.");
            Console.WriteLine();
        }
        public static void PrintHardwareConfig()
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
        public static void PrintCurrentConfig()
        {
            Console.WriteLine("policy: default");
            Console.WriteLine("preferred node: current");
            Console.WriteLine("physcpubind: 0 1 2 3 4 5 6 7 8");
            Console.WriteLine("cpubind: 0 1");
            Console.WriteLine("nodebind: 0 1");
            Console.WriteLine("membind: 0 1");
        }
    }
    class NumactlParser : ParserBase
    {
        public NoValueOption hardware = new(new string[] { "H", "hardware" }, "Print hardware configuration.");
        public NoValueOption show = new(new string[] { "S", "show" }, "Show current NUMA policy.");
        public StringOption physcpubind = new(new string[] { "C", "physcpubind" }, "Run on given CPUs only.");
        public StringOption membind = new(new string[] { "m", "membind" }, "Allocate memory from given nodes only.");
        public StringOption interleave = new(new string[] { "i", "interleave" }, "Interleave memory allocation across given nodes.");
        public StringOption preferred = new(new string[] { "p", "preferred" }, "Prefer memory allocations from given node.");
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new NumactlParser();
            bool nothingSet = true;
            try
            {
                parser.Parse(args);
            }
            catch (ParseException)
            {
                Console.Error.WriteLine("Passed arguments doesn't conform to program specification. See help for more explanation.");
                Environment.Exit(1);
                throw;
            }
            if (parser.hardware.GetValue())
            {
                nothingSet = false;
                PreparedMessages.PrintHardwareConfig();
            }

            if (parser.show.GetValue())
            {
                nothingSet = false;
                PreparedMessages.PrintCurrentConfig();
            }

            if (parser.physcpubind.IsSet)
            {
                nothingSet = false;
                Console.WriteLine(parser.physcpubind.GetValue() ?? "physcpubind has not been set");
            }
            if (parser.membind.IsSet)
            {
                nothingSet = false;
                Console.WriteLine(parser.membind.GetValue() ?? "membind has not been set");
            }
            if (parser.interleave.IsSet)
            {
                nothingSet = false;
                Console.WriteLine(parser.interleave.GetValue() ?? "interleave has not been set");
            }
            if (parser.preferred.IsSet)
            {
                nothingSet = false;
                Console.WriteLine(parser.preferred.GetValue() ?? "preferred has not been set");
            }
            if (nothingSet)
            {
                PreparedMessages.PrintNotGeneratedHelp();
                Console.WriteLine(parser.GenerateHelp());
            }
        }
    }
}
