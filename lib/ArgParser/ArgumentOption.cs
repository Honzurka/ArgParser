using System.Collections.Generic;

namespace ArgParser
{
        //similar to Option

    public abstract class ArgumentOptionBase {
        internal readonly string Description;
        internal readonly ParameterAccept parameterAccept;

        // public T Value(int idx = 0) { return default(T); }

        // 1. nacte do pameti parsovane parametry
        // 2. hodi vyjimku / vrati false pokud selze
        // allows creating custom Options
        abstract protected void Parse(string[] optVals);

        internal void CallParse(string[] optVals) => Parse(optVals); //impl. detail
    }

    public interface IArgument {}
    public abstract class ArgumentBase<T> : ArgumentOptionBase, IArgument {
        internal readonly string Name;
        public T GetValue(int idx = 0) {return default(T);}
        //    => idx < 0 || idx > paramValues.Count ? default(T) : paramValues[idx];
        // public T GetValueWithDefaultReplace(int idx = 0);
    }
    public sealed class IntArgument : ArgumentBase<int?>
    {
        public IntArgument(string name, string description, int minValue = int.MinValue, int maxValue = int.MaxValue, 
            ParameterAccept parameterAccept = new ParameterAccept(),
            int? defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class StringArgument : ArgumentBase<string>
    {
        public StringArgument(string name, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class EnumArgument : ArgumentBase<string>
    {
        public EnumArgument(string name, string description, string[] domain,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class BoolArgument : ArgumentBase<bool?>
    {
        public BoolArgument(string name, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            bool? defaultValue = null) { }
        protected override void Parse(string[] optVals) { }
    }


    // public interface IOption {}
    public abstract class OptionBase<T> : ArgumentOptionBase
    {
        internal readonly string[] Names;
        internal readonly bool IsMandatory;
        // internal T defaultValue;

        protected List<T> paramValues;
        public int ParamCount => paramValues.Count;
        public T GetValue(int idx = 0) {return default(T);}
        //    => idx < 0 || idx > paramValues.Count ? default(T) : paramValues[idx];
    }

    public sealed class IntOption : OptionBase<int?>
    {
        public IntOption(string[] names, string description, int minValue = int.MinValue, int maxValue = int.MaxValue, 
            ParameterAccept parameterAccept = new ParameterAccept(),
            int? defaultValue = null, bool isMandatory = false) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class StringOption : OptionBase<string>
    {
        public StringOption(string[] names, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null, bool isMandatory = false) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class EnumOption : OptionBase<string>
    {
        public EnumOption(string[] names, string description, string[] domain,
            ParameterAccept parameterAccept = new ParameterAccept(),
            string defaultValue = null, bool isMandatory = false) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class BoolOption : OptionBase<bool?>
    {
        public BoolOption(string[] names, string description,
            ParameterAccept parameterAccept = new ParameterAccept(),
            bool? defaultValue = null, bool isMandatory = false) { }
        protected override void Parse(string[] optVals) { }
    }
    public sealed class NoValueOption : OptionBase<bool>
    {
        // mandatory je vzdy false, parameter accept je vzdy nic, defaultValue bude false
        public NoValueOption(string[] names, string description) { }
        protected override void Parse(string[] optValue = null) { }
    }
}