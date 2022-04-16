using ArgParser;
using Xunit;

namespace ArgParserTests
{
    public class Parsing_integer_options
    {

        class IntegerParser : ParserBase
        {
            public IntOption option = new(new string[] { "o" }, "optionDesc");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Works_for_valid_input(int optionValue)
        {
            var args = new string[] { "-o", optionValue.ToString() };
            var parser = new IntegerParser();

            parser.Parse(args);

            Assert.True(parser.option.IsSet);
            Assert.Equal(optionValue, parser.option.GetValue());
        }

        [Theory]
        [InlineData("O")]
        [InlineData("99g9")]
        [InlineData("1 000")]
        [InlineData("1_000")]
        [InlineData("1.0")]
        [InlineData("1f")]
        [InlineData("--1")]
        public void Fails_when_value_is_not_a_number(string optionValue)
        {
            var args = new string[] { "-o", optionValue };
            var parser = new IntegerParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

        class BoundedIntegerParser : ParserBase
        {
            public IntOption option = new(new string[] { "o" }, "optionDesc", 0, 100);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(100, true)]
        [InlineData(-1, false)]
        [InlineData(101, false)]
        [InlineData(int.MinValue, false)]
        [InlineData(int.MaxValue, false)]
        public void Fails_when_value_is_out_of_range(int optionValue, bool shouldPass)
        {
            var args = new string[] { "-o", optionValue.ToString() };
            var parser = new BoundedIntegerParser();

            var recordedException = Record.Exception(() => parser.Parse(args));

            if (shouldPass)
            {
                Assert.Null(recordedException);
                Assert.True(parser.option.IsSet);
                Assert.Equal(optionValue, parser.option.GetValue());
            }
            else
            {
                Assert.NotNull(recordedException);
            }
        }

    }
}
