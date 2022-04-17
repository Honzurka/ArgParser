using ArgParser;
using System;
using Xunit;

namespace ArgParserTests
{
    public class Parsing_string_options
    {
        class StringParser : ParserBase
        {
            public StringOption shortOpt = new(new string[] { "s" }, "shortOptDesc");
            public StringOption longOpt = new(new string[] { "long" }, "longOptDesc");
            public StringOption mixedOpt = new(new string[] { "m", "mixed" }, "mixedOptDesc");
            public StringOption defaultOpt = new(new string[] { "d" }, "defaultOptDesc", defaultValue: "defaultVal");
        }

        [Fact]
        public void Works_when_optional_options_are_missing()
        {
            var args = Array.Empty<string>();
            var parser = new StringParser();

            parser.Parse(args);

            Assert.Equal("defaultVal", parser.defaultOpt.GetValue());

            Assert.False(parser.shortOpt.IsSet);
            Assert.False(parser.longOpt.IsSet);
            Assert.False(parser.mixedOpt.IsSet);
        }

        [Fact]
        public void Works_with_options_with_short_names()
        {
            var args = new string[] { "-s", "shortVal", "-m", "mixedVal" };
            var parser = new StringParser();

            parser.Parse(args);

            Assert.True(parser.shortOpt.IsSet);
            Assert.Equal("shortVal", parser.shortOpt.GetValue());

            Assert.True(parser.mixedOpt.IsSet);
            Assert.Equal("mixedVal", parser.mixedOpt.GetValue());

            Assert.False(parser.longOpt.IsSet);
        }

        [Fact]
        public void Works_with_options_with_long_names()
        {
            var args = new string[] { "--long", "longVal", "--mixed", "mixedVal" };
            var parser = new StringParser();

            parser.Parse(args);

            Assert.True(parser.longOpt.IsSet);
            Assert.Equal("longVal", parser.longOpt.GetValue());

            Assert.True(parser.mixedOpt.IsSet);
            Assert.Equal("mixedVal", parser.mixedOpt.GetValue());

            Assert.False(parser.shortOpt.IsSet);
        }

        [Fact]
        public void Fails_when_encountering_unknown_options()
        {
            var args = new string[] { "-s", "first", "-u", "second" };
            var parser = new StringParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

        class MultiStringParser : ParserBase
        {
            public StringOption option = new(new string[] { "o" }, "optionDesc", parameterAccept: new ParameterAccept(2));
        }

        [Fact]
        public void Works_when_multival_option_has_valid_number_of_values()
        {
            var args = new string[] { "-o", "first", "second" };
            var parser = new MultiStringParser();

            parser.Parse(args);

            Assert.True(parser.option.IsSet);
            Assert.Equal("first", parser.option.GetValue(0));
            Assert.Equal("second", parser.option.GetValue(1));
        }

        [Theory]
        [InlineData("first")]
        [InlineData("first", "second", "third")]
        public void Fails_when_multival_option_has_invalid_number_of_values(params string[] optionValues)
        {
            var args = new string[optionValues.Length + 1];
            args[0] = "-o";
            optionValues.CopyTo(args, 1);
            var parser = new MultiStringParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

        class RangeStringParser : ParserBase
        {
            public StringOption option = new(new string[] { "o" }, "optionDesc", parameterAccept: new ParameterAccept(1, 3));
        }

        [Theory]
        [InlineData("first")]
        [InlineData("first", "second")]
        [InlineData("first", "second", "third")]
        public void Works_when_variable_option_has_valid_number_of_values(params string[] optionValues)
        {
            var args = new string[optionValues.Length + 1];
            args[0] = "-o";
            optionValues.CopyTo(args, 1);
            var parser = new RangeStringParser();

            parser.Parse(args);

            Assert.True(parser.option.IsSet);
            Assert.Equal(optionValues.Length, parser.option.ParsedParameterCount);
            for (int i = 0; i < optionValues.Length; i++)
            {
                Assert.Equal(optionValues[i], parser.option.GetValue(i));
            }
        }

        [Theory]
        [InlineData(new string[] { }, false)]
        [InlineData(new string[] {"first", "second", "third", "fourth"}, false)]
#pragma warning disable xUnit1026 // Without the extra parameter, the test always fails
        public void Fails_when_variable_option_has_invalid_number_of_values(string[] optionValues, bool _)
#pragma warning restore xUnit1026
        {
            var args = new string[optionValues.Length + 1];
            args[0] = "-o";
            optionValues.CopyTo(args, 1);
            var parser = new RangeStringParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

        class MandatoryStringParser : ParserBase
        {
            public StringOption option = new(new string[] { "o" }, "optionDesc", isMandatory: true);
        }

        [Fact]
        public void Fails_when_mandatory_option_is_missing()
        {
            var args = Array.Empty<string>();
            var parser = new MandatoryStringParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

    }
}