using ArgParser;
using Xunit;

namespace ArgParserTests
{
    public class Parsing_string_arguments
    {
        class ArgumentParser : ParserBase
        {
            public StringArgument simpleArg = new("simpleArg", "simpleArgDesc");
            public StringArgument defaultArg = new("defaultArg", "defaultArgDesc", defaultValue: "defaultVal");

            protected override IArgument[] GetArgumentOrder() => new IArgument[] { simpleArg, defaultArg };
        }

        [Fact]
        public void Works_for_valid_input()
        {
            var args = new string[] { "firstVal", "secondVal" };
            var parser = new ArgumentParser();

            parser.Parse(args);

            Assert.Equal("firstVal", parser.simpleArg.GetValue());
            Assert.Equal("secondVal", parser.defaultArg.GetValue());
        }

        [Fact]
        public void Works_when_args_with_default_values_are_omitted()
        {
            var args = new string[] { "simpleVal" };
            var parser = new ArgumentParser();

            parser.Parse(args);

            Assert.Equal("simpleVal", parser.simpleArg.GetValue());
            Assert.Equal("defaultVal", parser.defaultArg.GetValue());
        }

        [Fact]
        public void Fails_when_nondefault_arguments_are_missing()
        {
            var args = new string[0];
            var parser = new ArgumentParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

        class MultiArgumentParser : ParserBase
        {
            public StringArgument argument = new("argument", "argumentDesc", parameterAccept: new ParameterAccept(2));

            protected override IArgument[] GetArgumentOrder() => new IArgument[] { argument };
        }

        [Fact]
        public void Works_when_multival_argument_gets_correct_number_of_inputs()
        {
            var args = new string[] { "first", "second" };
            var parser = new MultiArgumentParser();

            parser.Parse(args);

            Assert.Equal("first", parser.argument.GetValue(0));
            Assert.Equal("second", parser.argument.GetValue(1));
        }

        [Theory]
        [InlineData("first")]
        [InlineData("first", "second", "third")]
        public void Fails_when_multival_argument_gets_invalid_number_of_inputs(params string[] args)
        {
            var parser = new MultiArgumentParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

        class RangeArgumentParser : ParserBase
        {
            public StringArgument argument = new("argument", "argumentDesc", parameterAccept: new ParameterAccept(1, 3));
            
            protected override IArgument[] GetArgumentOrder() => new IArgument[] { argument };
        }

        [Theory]
        [InlineData("first")]
        [InlineData("first", "second")]
        [InlineData("first", "second", "third")]
        public void Works_when_variable_argument_gets_valid_number_of_inputs(params string[] args)
        {
            var parser = new RangeArgumentParser();

            parser.Parse(args);

            // Assert.Equal(args.Length, parser.argument.ParsedParameterCount);
            for(int i = 0; i < args.Length; i++)
            {
                Assert.Equal(args[i], parser.argument.GetValue(i));
            }
        }

        [Theory]
        [InlineData()]
        [InlineData("first", "second", "third", "fourth")]
        public void Fails_when_variable_argument_gets_incorrect_number_of_inputs(params string[] args)
        {
            var parser = new RangeArgumentParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

        class OptionArgumentParser : ParserBase
        {
            public StringOption option = new(new string[] { "o" }, "optionDesc");

            public StringArgument argument = new("argument", "argumentDesc");

            protected override IArgument[] GetArgumentOrder() => new IArgument[] { argument };
        }

        [Fact]
        public void Works_when_parsing_delimited_arguments_from_options()
        {
            var args = new string[] { "-o", "first", "--", "second" };
            var parser = new OptionArgumentParser();

            parser.Parse(args);

            Assert.True(parser.option.IsSet);
            Assert.Equal("first", parser.option.GetValue());
            Assert.Equal("second", parser.argument.GetValue());
        }

    }
}
