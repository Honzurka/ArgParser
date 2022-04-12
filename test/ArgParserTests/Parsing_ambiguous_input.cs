using ArgParser;
using Xunit;

namespace ArgParserTests
{
    public class Parsing_ambiguous_input
    {
        class AmbiguousParser : ParserBase
        {
            public StringOption ambigOption = new(new string[] { "o" }, "optionDesc", new ParameterAccept(1, 2));

            public StringArgument ambigArgument = new("argument", "argumentDesc", new ParameterAccept(1, 2));

            protected override IArgument[] GetArgumentOrder() => new[] { ambigArgument };
        }

        [Fact]
        public void Works_when_ambiguous_parts_dont_clash()
        {
            var args = new string[] { "arg1", "arg2" };
            var parser = new AmbiguousParser();

            parser.Parse(args);

            Assert.False(parser.ambigOption.IsSet);
            Assert.Equal("arg1", parser.ambigArgument.GetValue(0));
            Assert.Equal("arg2", parser.ambigArgument.GetValue(1));
        }

        [Fact]
        public void Works_when_ambiguous_parts_are_explicitly_delimited()
        {
            var args = new string[] { "-o", "val1", "--", "arg1" };
            var parser = new AmbiguousParser();

            parser.Parse(args);

            Assert.True(parser.ambigOption.IsSet);
            Assert.Equal(1, parser.ambigOption.ParsedParameterCount);
            Assert.Equal("val1", parser.ambigOption.GetValue(0));

            Assert.Equal("arg1", parser.ambigArgument.GetValue(0));
        }

        [Fact]
        public void Fails_when_ambiguous_parts_receive_ambiguous_input()
        {
            var args = new string[] { "-o", "val1", "val2_or_arg1", "arg1_or_arg2" };
            var parser = new AmbiguousParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

        [Fact]
        public void Fails_even_when_ambiguous_parts_receive_implicitly_unambiguous_input()
        {
            var args = new string[] { "-o", "val1", "val2", "arg1", "arg2" };
            var parser = new AmbiguousParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

        class OptionArgumentParser : ParserBase
        {
            public StringOption option = new(new string[] { "o" }, "optionDesc");

            public StringArgument argument = new("argument", "argumentDesc");

            protected override IArgument[] GetArgumentOrder() => new IArgument[] { argument };
        }

        [Fact]
        public void Fails_when_unknown_option_is_encountered()
        {
            var args = new string[] { "--first", "second" };
            var parser = new OptionArgumentParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

        [Fact]
        public void Works_when_unknown_option_is_explicitly_delimited()
        {
            var args = new string[] { "--", "--first", "second" };
            var parser = new OptionArgumentParser();

            parser.Parse(args);

            Assert.Equal("--first", parser.argument.GetValue(0));
            Assert.Equal("second", parser.argument.GetValue(1));
        }

        [Fact]
        public void Parses_option_when_it_is_not_delimited()
        {
            var args = new string[] { "-o", "value" };
            var parser = new OptionArgumentParser();

            parser.Parse(args);

            Assert.True(parser.option.IsSet);
            Assert.Equal("value", parser.option.GetValue());
        }

        [Fact]
        public void Parses_option_as_argument_when_it_is_delimited()
        {
            var args = new string[] { "--", "-o", "value" };
            var parser = new OptionArgumentParser();

            parser.Parse(args);

            Assert.False(parser.option.IsSet);
            Assert.Equal("-o", parser.argument.GetValue(0));
            Assert.Equal("value", parser.argument.GetValue(1));
        }
    }
}
