using ArgParser;
using Xunit;

namespace ArgParserTests
{
    public class Parsing_bool_arguments
    {
        class BoolArgumentParser : ParserBase
        {
            public BoolArgument argument = new("argument", "argumentDesc");
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
        public void Works_for_valid_boolean_representations(string argumentValue, bool expectedValue)
        {
            var args = new string[] { argumentValue };
            var parser = new BoolArgumentParser();

            parser.Parse(args);

            Assert.Equal(expectedValue, parser.argument.GetValue());
        }
    }
}
