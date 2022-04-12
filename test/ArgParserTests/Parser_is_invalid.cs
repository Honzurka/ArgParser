using ArgParser;
using Xunit;

namespace ArgParserTests
{
    public class Parser_is_invalid
    {
        class UnknownArgumentParser : ParserBase
        {
            public StringArgument argument = new("argument", "argumentDesc");

            protected override IArgument[] GetArgumentOrder() => new IArgument[] { argument, new StringArgument("unknownArg", "unknownArgDesc") };
        }

        [Fact]
        public void When_argument_order_contains_unknown_arguments()
        {
            var args = new string[] { "first" };
            var parser = new UnknownArgumentParser();

            Assert.Throws<ParserCodeException>(() => parser.Parse(args));
        }

        class TwoVarArgParser : ParserBase
        {
            public StringArgument firstArg = new("firstArg", "firstArgDesc", new ParameterAccept(1, 2));
            public StringArgument secondArg = new("secondArg", "secondArgDesc", new ParameterAccept(1, 2));

            protected override IArgument[] GetArgumentOrder() => new IArgument[] { firstArg, secondArg };
        }

        [Fact]
        public void When_multiple_arguments_have_variable_count_of_parameters()
        {
            var args = new string[] { "first" };
            var parser = new TwoVarArgParser();

            Assert.Throws<ParserCodeException>(() => parser.Parse(args));
        }

        class ClashingNamesParser : ParserBase
        {
            public StringOption firstOpt = new(new string[] { "o", "first" }, "firstOptDesc");
            public StringOption secondOpt = new(new string[] { "o", "second" }, "secondOptDesc");
        }

        [Fact]
        public void When_multiple_options_contain_the_same_name()
        {
            var args = new string[] { "--first", "x", "-o", "y" };
            var parser = new ClashingNamesParser();

            Assert.Throws<ParserCodeException>(() => parser.Parse(args));
        }
    }
}
