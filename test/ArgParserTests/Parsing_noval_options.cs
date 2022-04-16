using ArgParser;
using Xunit;

namespace ArgParserTests
{
    public class Parsing_noval_options
    {
        class NoValParser : ParserBase
        {
            public NoValueOption option = new(new string[] { "o", "option" }, "noValOptDesc");
        }

        [Fact]
        public void Stores_true_if_flag_is_present()
        {
            var args = new string[] { "-o" };
            var parser = new NoValParser();

            parser.Parse(args);

            Assert.True(parser.option.IsSet);
            Assert.True(parser.option.GetValue());
        }

        [Fact]
        public void Stores_false_if_flag_is_not_present()
        {
            var args = new string[0];
            var parser = new NoValParser();

            parser.Parse(args);

            Assert.False(parser.option.IsSet);
            Assert.False(parser.option.GetValue());
        }
    }
}
