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


    public abstract class IOption<T>
    {
        public T Value { get; private set; }

        // 1. nacte do pameti parsovane parametry
        // 2. hodi vyjimku / vrati false pokud selze
        // allows creating custom Options
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
    }
    public class StringOption : IOption<string>
    {
        public StringOption(OptionSettings settings, string defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
    }
    public class EnumOption : IOption<string>
    {
        public EnumOption(OptionSettings settings, string[] domain, string defaultValue) { }
        protected override void Parse(string[] optVals) { }
    }
    public class BoolOption : IOption<bool>
    {
        public BoolOption(OptionSettings settings, bool? defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
    }
    public class NoValueOption : IOption<bool>
    {
        // mandatory je vzdy false, parameter accept je vzdy nic, defaultValue bude false
        public NoValueOption(string[] names, string description) { }
        public void Parse(string[] optValue) { }

        public bool Value { get; private set; }
    }
}
