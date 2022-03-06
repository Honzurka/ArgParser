namespace ArgParser
{
    public abstract class ParserBase
    {
        protected virtual string Delimiter => "--";

        protected virtual IArgument[] GetArgumentOrder() => null;
        internal IArgument[] CallGetArgumentOrder() => GetArgumentOrder();

        public void Parse(string[] args) { /*internal impl*/ }
        public string GenerateHelp() { return ""; }
    }


    public struct ParameterAccept {
        public readonly int MinParameterAmount, MaxParameterAmount;

        public ParameterAccept(int minParameterAmount = 1, int maxParameterAmount = 1) {
            //if (minParameterAmount < 0 || maxParameterAmount < minParameterAmount) throw new Exception();
            MinParameterAmount = minParameterAmount;
            MaxParameterAmount = maxParameterAmount;
        }

        public static ParameterAccept Mandatory = new ParameterAccept(1, 1);
        public static ParameterAccept Optional = new ParameterAccept(0, 1);
        public static ParameterAccept AtleastOne = new ParameterAccept(1, int.MaxValue);
        public static ParameterAccept Any = new ParameterAccept(0, int.MaxValue);
    };
}
