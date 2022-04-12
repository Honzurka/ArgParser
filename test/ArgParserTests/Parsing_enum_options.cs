using ArgParser;
using Xunit;

namespace ArgParserTests
{
    public class Parsing_enum_options
    {
        class EnumParser : ParserBase
        {
            public EnumOption option = new(new string[] { "o" }, "optionDesc", new string[] { "alpha", "beta" });
        }

        [Theory]
        [InlineData("alpha", true)]
        [InlineData("beta", true)]
        [InlineData("gamma", false)]
        public void Works_only_for_values_from_the_domain(string optionValue, bool shouldPass)
        {
            var args = new string[] { "-o", optionValue };
            var parser = new EnumParser();

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
