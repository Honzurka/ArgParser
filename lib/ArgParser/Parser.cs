using System;

namespace ArgParser
{
    public interface IOptions {
        public string Delimiter { get; }
    }

    public class Parser
    {
        public Parser(IOptions specification) { }
        public string GenerateHelp() { return ""; }
        public void Parse(string[] args) { }
    }

    public enum ParameterAccept { Mandatory, Optional, None };

    public abstract class IOption<T>
    {
        public T Value { get; /*private set;*/ }

        // 1. nacte do pameti parsovane parametry
        // 2. hodi vyjimku / vrati false pokud selze
        abstract protected void Parse(string[] optVals);
        internal void CallParse(string[] optVals) => Parse(optVals); //impl. detail

    }

    public class OptionSettings
    {
        public readonly string[] Names;
        public readonly string Description;
        public readonly bool IsMandatory;
        public readonly ParameterAccept parameterAccept;

        public OptionSettings(string[] names, string description, ParameterAccept parameterAccept, bool isMandatory = true) {}
    }

    public class IntOption : IOption<int>
    {
        public IntOption(OptionSettings settings, int? defaultValue = null, int minValue = int.MinValue, int maxValue = int.MaxValue) { }

        protected override void Parse(string[] optVals) { }
        public int Value { get; private set; }
    }
    public class StringOption : IOption<string>
    {
        public StringOption(OptionSettings settings, string defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
        public string Value { get; private set; }
    }
    public class EnumOption : IOption<string>
    {
        public EnumOption(OptionSettings settings, string[] domain, string defaultValue) { }
        protected override void Parse(string[] optVals) { }
        public string Value { get; private set; }
    }
    public class BoolOption : IOption<bool>
    {
        public BoolOption(OptionSettings settings, bool? defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
        public bool Value { get; private set; }
    }

}
