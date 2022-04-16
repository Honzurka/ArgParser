using ArgParser;
using Xunit;

namespace ArgParserTests
{
    public class Parsing_enum_arguments
    {
        class EnumArgumentParser : ParserBase
        {
            public EnumArgument argument = new("argument", "argumentDesc", new string[] { "alpha", "beta" });

            protected override IArgument[] GetArgumentOrder() => new IArgument[] { argument };
        }

        [Theory]
        [InlineData("alpha", true)]
        [InlineData("beta", true)]
        [InlineData("gamma", false)]
        public void Works_only_for_values_from_the_domain(string argumentValue, bool shouldPass)
        {
            var args = new string[] { argumentValue };
            var parser = new EnumArgumentParser();

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
