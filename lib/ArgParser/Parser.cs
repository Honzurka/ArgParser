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



    public struct ParameterAccept {
        public readonly int MinParameterAmount, MaxParameterAmount;

        public ParameterAccept(int minParameterAmount, int maxParameterAmount = int.MaxValue) {
            MinParameterAmount = minParameterAmount;
            MaxParameterAmount = maxParameterAmount;
        }

        public static ParameterAccept Mandatory = new ParameterAccept(1, 1);
        public static ParameterAccept Optional = new ParameterAccept(0, 1);
        public static ParameterAccept AtleastOne = new ParameterAccept(1, int.MaxValue);
        public static ParameterAccept Any = new ParameterAccept(0, int.MaxValue);
    };


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
    public class NoValueOption : IOption<bool>
    {
        // mandatory je vzdy false, parameter accept je vzdy nic, defaultValue bude false
        public NoValueOption(string[] names, string description) { }
        public void Parse(string[] optValue) { }

        public bool Value { get; private set; }
    }
}
