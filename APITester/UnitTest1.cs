using NUnit.Framework;
using System.IO;
using System;
using ArgParser;

namespace APITester
{
    class ParserTestDeclaration : ParserBase
    {
        public BoolOption BoolOpt = new(new string[] { "b", "bool" }, "bool description");
        public StringOption StringOpt = new(new string[] { "s", "string" }, "string description");
        public NoValueOption NoValOpt = new(new string[] { "n", "NoVal" }, "NoValue description");
        public IntOption IntOpt = new(new string[] { "i", "int" }, "int description");
        public EnumOption EnumOpt = new(new string[] { "e", "enum" }, "enum description", new string[] { "a", "b", "c" });

        public IntArgument IntArg = new("int1", "int description");
        public StringArgument StringArg = new("string1", "string descriptio,");
        public EnumArgument EnumArg = new("enum1", "enum description", new string[] { "a", "b", "c" });
        public BoolArgument BoolArg = new("bool1", "bool description");
    }
    
    class ParserTestEdgeCases : ParserBase
    {
        public BoolOption BoolOpt = new(new string[] { "b", "bool" }, "bool description");
        public StringOption StringOpt = new(new string[] { "s", "string" }, "string description");
        public NoValueOption NoValOpt = new(new string[] { "n", "NoVal" }, "NoValue description");
        public IntOption IntOpt = new(new string[] { "i", "int" }, "int description", 10, 20);
        public EnumOption EnumOpt = new(new string[] { "e", "enum" }, "enum description", new string[] { "a", "b", "c" });

        public IntArgument IntArg = new("int1", "int description", 10, 20);
    }

    class ParserTestMandatory : ParserBase
    {
        public BoolOption BoolOpt = new(new string[] { "b", "bool" }, "bool description", parameterAccept: ParameterAccept.Mandatory);
        public StringOption StringOpt = new(new string[] { "s", "string" }, "string description");
    }

    class ParserTestAtLeastOne : ParserBase
    {
        public IntOption IntOpt = new(new string[] { "i", "int" }, "int description", 10, 20, parameterAccept: ParameterAccept.AtLeastOne);
        public StringOption StringOpt = new(new string[] { "s", "string" }, "string description");
    }

    class ParserTestMinParam : ParserBase
    {
        public IntOption IntOpt = new(new string[] { "j", "int2" }, "int description", parameterAccept: new ParameterAccept(3, 3));
        public StringOption StringOpt = new(new string[] { "s", "string" }, "string description");
    }

    public class Tests
    {
        private ParserTestDeclaration ParserDeclare;
        private ParserTestDeclaration ParserAddValue;
        private ParserTestEdgeCases EdgeCases;
        [SetUp]
        public void Setup()
        {
            ParserDeclare = new ParserTestDeclaration();
        }

        [Test]
        [Category("AddOpt")]
        public void AddBoolOpt()
        {
            Assert.IsNotNull(ParserDeclare.BoolOpt);
        }
        [Test]
        [Category("AddOpt")]
        public void AddStringOpt()
        {
            Assert.IsNotNull(ParserDeclare.StringOpt);
        }
        [Test]
        [Category("AddOpt")]
        public void AddNoValOpt()
        {
            Assert.IsNotNull(ParserDeclare.NoValOpt);
        }
        [Test]
        [Category("AddOpt")]
        public void AddIntOpt()
        {
            Assert.IsNotNull(ParserDeclare.IntOpt);
        }
        [Test]
        [Category("AddOpt")]
        public void AddEnumOpt()
        {
            Assert.IsNotNull(ParserDeclare.EnumOpt);
        }
        
        [Test]
        [Category("AddArg")]
        public void AddIntArg()
        {
            Assert.IsNotNull(ParserDeclare.IntArg);
        }
        [Test]
        [Category("AddArg")]
        public void AddStringArg()
        {
            Assert.IsNotNull(ParserDeclare.StringArg);
        }
        [Test]
        [Category("AddArg")]
        public void AddEnumArg()
        {
            Assert.IsNotNull(ParserDeclare.EnumArg);
        }
        [Test]
        [Category("AddArg")]
        public void AddBoolArg()
        {
            Assert.IsNotNull(ParserDeclare.BoolArg);
        }

        [Test]
        [Category("AddValue")]
        public void BoolValOptShort()
        {
            ParserAddValue = new ParserTestDeclaration();
            string[] args = new string[] { "-b", "true" };
            ParserAddValue.Parse(args);
            Assert.AreEqual(true, ParserAddValue.BoolOpt.GetValue(0));
        }
        [Test]
        [Category("AddValue")]
        public void BoolValOptLong()
        {
            ParserAddValue = new ParserTestDeclaration();
            string[] args = new string[] { "--bool", "true" };
            ParserAddValue.Parse(args);
            Assert.AreEqual(true, ParserAddValue.BoolOpt.GetValue(0));
        }
        [Test]
        [Category("AddValue")]
        public void StringValOptShort()
        {
            ParserAddValue = new ParserTestDeclaration();
            string[] args = new string[] { "-s", "hello" };
            ParserAddValue.Parse(args);
            Assert.AreEqual("hello", ParserAddValue.StringOpt.GetValue(0));
        }
        [Test]
        [Category("AddValue")]
        public void StringValOptLong()
        {
            ParserAddValue = new ParserTestDeclaration();
            string[] args = new string[] { "--string", "there" };
            ParserAddValue.Parse(args);
            Assert.AreEqual("there", ParserAddValue.StringOpt.GetValue(0));
        }
        [Test]
        [Category("AddValue")]
        public void IntValOptShort()
        {
            ParserAddValue = new ParserTestDeclaration();
            string[] args = new string[] { "-i", "10" };
            ParserAddValue.Parse(args);
            Assert.AreEqual(10, ParserAddValue.IntOpt.GetValue(0));
        }
        [Test]
        [Category("AddValue")]
        public void IntValOptLong()
        {
            ParserAddValue = new ParserTestDeclaration();
            string[] args = new string[] { "--int", "20" };
            ParserAddValue.Parse(args);
            Assert.AreEqual(20, ParserAddValue.IntOpt.GetValue(0));
        }
        //TODO: to same pro Arguments
        [Test]
        [Category("AddValue")]
        public void BoolValArg()
        {
            ParserAddValue = new ParserTestDeclaration();
            string[] args = new string[] { "true" };
            ParserAddValue.Parse(args);
            Assert.AreEqual(true, ParserAddValue.BoolArg.GetValue(0));
        }
        [Test]
        [Category("AddValue")]
        public void IntValArg()
        {
            ParserAddValue = new ParserTestDeclaration();
            string[] args = new string[] { "20" };
            ParserAddValue.Parse(args);
            Assert.AreEqual(20, ParserAddValue.IntArg.GetValue(0));
        }
        [Test]
        [Category("AddValue")]
        public void StringValArg()
        {
            ParserAddValue = new ParserTestDeclaration();
            string[] args = new string[] { "testText" };
            ParserAddValue.Parse(args);
            Assert.AreEqual("testText", ParserAddValue.StringArg.GetValue(0));
        }

        [Test]
        [Category("EdgeCases")]
        public void IntOptUpperBound()
        {
            EdgeCases = new ParserTestEdgeCases();
            string[] args = new string[] {"--int","25"};
            Assert.Throws<ParseException>(delegate { EdgeCases.Parse(args); });
        }
        [Test]
        [Category("EdgeCases")]
        public void IntOptLowerBound()
        {
            EdgeCases = new ParserTestEdgeCases();
            string[] args = new string[] {"-i","8"};
            Assert.Throws<ParseException>(delegate { EdgeCases.Parse(args); });
        }
        [Test]
        [Category("EdgeCases")]
        public void IntArgUpperBound()
        {
            EdgeCases = new ParserTestEdgeCases();
            string[] args = new string[] { "25" };
            Assert.Throws<ParseException>(delegate { EdgeCases.Parse(args); });
        }
        [Test]
        [Category("EdgeCases")]
        public void IntArgLowerBound()
        {
            EdgeCases = new ParserTestEdgeCases();
            string[] args = new string[] { "8" };
            Assert.Throws<ParseException>(delegate { EdgeCases.Parse(args); });
        }

        [Test]
        [Category("ParameterAccect")]
        public void Mandatory()
        {
            ParserTestMandatory par = new ParserTestMandatory();
            string[] args = new string[] { "-s", "String text" };
            Assert.Throws<ParseException>(delegate { par.Parse(args); });
        }
        [Test]
        [Category("ParameterAccect")]
        public void AtLeastOne()
        {
            ParserTestAtLeastOne par = new ParserTestAtLeastOne();
            string[] args = new string[] { "-s", "String Text" };
            Assert.Throws<ParseException>(delegate { par.Parse(args); });
        }
        [Test]
        [Category("ParameterAccect")]
        public void MinParamFail()
        {
            ParserTestMinParam par = new ParserTestMinParam();
            string[] args = new string[] { "-i", "10","20"};
            Assert.Throws<ParseException>(delegate { par.Parse(args); });
        }
    }
}
