using ArgParser;
using Xunit;

namespace ArgParserTests
{
    public class Parsing_bool_options
    {
        class BoolOptionParser : ParserBase
        {
            public BoolOption option = new(new string[] { "o" }, "optionDesc");
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("False", false)]
        // [InlineData("T", true)]
        // [InlineData("F", false)]
        // [InlineData("t", true)]
        // [InlineData("f", false)]
        // [InlineData("Yes", true)]
        // [InlineData("No", false)]
        // [InlineData("yes", true)]
        // [InlineData("no", false)]
        // [InlineData("Y", true)]
        // [InlineData("N", false)]
        // [InlineData("y", true)]
        // [InlineData("n", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        public void Works_for_valid_boolean_representations(string optionValue, bool expectedValue)
        {
            var args = new string[] { "-o", optionValue };
            var parser = new BoolOptionParser();

            parser.Parse(args);

            Assert.True(parser.option.IsSet);
            Assert.Equal(expectedValue, parser.option.GetValue());
        }
    }
}
