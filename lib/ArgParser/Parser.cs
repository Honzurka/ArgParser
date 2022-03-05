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

    public interface IOption<T>
    {
        public T Value { get; /*private set;*/ }
        public /*idealne internal?*/ void Parse(string[] optValue);
        // 1. nacte do pameti parsovane parametry
        // 2. hodi vyjimku / vrati false pokud selze
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

        public void Parse(string[] optValue) { }
        public int Value { get; private set; }
    }
    public class StringOption : IOption<string>
    {
        public StringOption(OptionSettings settings, string defaultValue = null) { }
        public void Parse(string[] optValue) { }
        public string Value { get; private set; }
    }
    public class EnumOption : IOption<string>
    {
        public EnumOption(OptionSettings settings, string[] domain, string defaultValue) { }
        public void Parse(string[] optValue) { }
        public string Value { get; private set; }
    }
    public class BoolOption : IOption<bool>
    {
        public BoolOption(OptionSettings settings, bool? defaultValue = null) { }
        public void Parse(string[] optValue) { }
        public bool Value { get; private set; }
    }

}
