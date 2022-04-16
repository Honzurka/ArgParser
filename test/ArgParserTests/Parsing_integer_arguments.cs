using ArgParser;
using Xunit;

namespace ArgParserTests
{
    public class Parsing_integer_arguments
    {

        class IntegerArgumentParser : ParserBase
        {
            public IntArgument argument = new("argument", "argumentDesc");

            protected override IArgument[] GetArgumentOrder() => new IArgument[] { argument };
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Works_for_valid_input(int argumentValue)
        {
            var args = new string[] { "--", argumentValue.ToString() };
            var parser = new IntegerArgumentParser();

            parser.Parse(args);

            Assert.Equal(argumentValue, parser.argument.GetValue());
        }

        [Theory]
        [InlineData("O")]
        [InlineData("99g9")]
        [InlineData("1 000")]
        [InlineData("1_000")]
        [InlineData("1.0")]
        [InlineData("1f")]
        [InlineData("--1")]
        public void Fails_when_value_is_not_a_number(string argumentValue)
        {
            var args = new string[] { argumentValue };
            var parser = new IntegerArgumentParser();

            Assert.Throws<ParseException>(() => parser.Parse(args));
        }

        class BoundedIntegerArgumentParser : ParserBase
        {
            public IntArgument argument = new("argument", "argumentDesc", 0, 100);

            protected override IArgument[] GetArgumentOrder() => new IArgument[] { argument };
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(100, true)]
        [InlineData(-1, false)]
        [InlineData(101, false)]
        [InlineData(int.MinValue, false)]
        [InlineData(int.MaxValue, false)]
        public void Works_only_when_value_is_in_range(int argumentValue, bool shouldPass)
        {
            var args = new string[] { argumentValue.ToString() };
            var parser = new BoundedIntegerArgumentParser();

            var recordedException = Record.Exception(() => parser.Parse(args));

            if (shouldPass)
            {
                Assert.Null(recordedException);
                Assert.Equal(argumentValue, parser.argument.GetValue());
            }
            else
            {
                Assert.NotNull(recordedException);
            }
        }
    }
}
