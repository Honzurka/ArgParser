using System;

namespace ArgParser
{
    //similar to Option
    public abstract class ArgumentBase<T> {
        internal readonly string Name;
        internal readonly string Description;
        internal readonly ParameterAccept parameterAccept;

        public T Value(int idx = 0) { return default(T); }

        // 1. nacte do pameti parsovane parametry
        // 2. hodi vyjimku / vrati false pokud selze
        // allows creating custom Options
        abstract protected void Parse(string[] optVals);

        internal void CallParse(string[] optVals) => Parse(optVals); //impl. detail
    }

    public abstract class ParserBase
    {
        public virtual string Delimiter => "--";

        protected virtual ArgumentBase[] GetArgumentOrder() => null;
        internal ArgumentBase[] CallGetArgumentOrder() => GetArgumentOrder();

        public void Parse(string[] args) { }
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


    public abstract class OptionBase<T>
    {
        internal readonly string[] Names;
        internal readonly string Description;
        internal readonly bool IsMandatory;
        internal readonly ParameterAccept parameterAccept;

        //#nullable enable
        public T Value(int idx = 0) { return default(T); }
        //#nullable disable

        // 1. nacte do pameti parsovane parametry
        // 2. hodi vyjimku / vrati false pokud selze
        // allows creating custom Options
        abstract protected void Parse(string[] optVals);

        internal void CallParse(string[] optVals) => Parse(optVals); //impl. detail
    }

    //public sealed class OptionSettings
    //{
    //    internal OptionSettings(string[] names, string description, ParameterAccept parameterAccept, bool isMandatory = true) {}
    //}

    public class IntOption : OptionBase<int?>
    {
        public IntOption(string[] names, string description, int minValue = int.MinValue, int maxValue = int.MaxValue, 
            ParameterAccept parameterAccept = new ParameterAccept(),
            int? defaultValue = null, bool isMandatory = false) { }

        protected override void Parse(string[] optVals) { }
    }
    public class StringOption : OptionBase<string>
    {
        public StringOption(string[] names, string description, ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null, bool isMandatory = false) { }
        protected override void Parse(string[] optVals) { }
    }
    public class EnumOption : OptionBase<string>
    {
        public EnumOption(string[] names, string description, string[] domain,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null, bool isMandatory = false) { }
        protected override void Parse(string[] optVals) { }
    }
    public class BoolOption : OptionBase<bool?>
    {
        public BoolOption(string[] names, string description, ParameterAccept parameterAccept = new ParameterAccept(),
            bool? defaultValue = null, bool isMandatory = false) { }
        protected override void Parse(string[] optVals) { }
    }

    //in args => true otherwise false
    public class NoValueOption : OptionBase<bool>
    {
        // mandatory je vzdy false, parameter accept je vzdy nic, defaultValue bude false
        public NoValueOption(string[] names, string description) { }
        protected override void Parse(string[] optValue = null) { }
    }
}
