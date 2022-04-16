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

        public IntArgument IntArg = new("int1", "int description", parameterAccept: ParameterAccept.Optional);
        
        protected override IArgument[] GetArgumentOrder() => new IArgument[] { IntArg };
    }

    class ParserTestDeclarationForBool : ParserBase
	{
        public BoolArgument BoolArg = new("bool1", "bool description", ParameterAccept.Optional);

        protected override IArgument[] GetArgumentOrder() => new IArgument[] { BoolArg };
    }

    class ParserTestDeclarationForString : ParserBase
	{
        public StringArgument StringArg = new("string1", "string descriptio,", ParameterAccept.Optional);

        protected override IArgument[] GetArgumentOrder() => new IArgument[] { StringArg };
    }

    class ParserTestEdgeCases : ParserBase
    {
        public BoolOption BoolOpt = new(new string[] { "b", "bool" }, "bool description");
        public StringOption StringOpt = new(new string[] { "s", "string" }, "string description");
        public NoValueOption NoValOpt = new(new string[] { "n", "NoVal" }, "NoValue description");
        public IntOption IntOpt = new(new string[] { "i", "int" }, "int description", 10, 20);
        public EnumOption EnumOpt = new(new string[] { "e", "enum" }, "enum description", new string[] { "a", "b", "c" });

        public IntArgument IntArg = new("int1", "int description", 10, 20);

        protected override IArgument[] GetArgumentOrder() => new IArgument[] { IntArg };
    }

    class ParserTestMandatory : ParserBase
    {
        public BoolOption BoolOpt = new(new string[] { "b", "bool" }, "bool description", parameterAccept: ParameterAccept.Mandatory, isMandatory: true);
        public StringOption StringOpt = new(new string[] { "s", "string" }, "string description");
    }

    class ParserTestAtLeastOne : ParserBase
    {
        public IntOption IntOpt = new(new string[] { "i", "int" }, "int description", 10, 20, parameterAccept: ParameterAccept.AtLeastOne, isMandatory: true);
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
            var BoolParserAddValue = new ParserTestDeclarationForBool();
            string[] args = new string[] { "true" };
            BoolParserAddValue.Parse(args);
            Assert.AreEqual(true, BoolParserAddValue.BoolArg.GetValue(0).Value);
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
            var StringParserAddValue = new ParserTestDeclarationForString();
            string[] args = new string[] { "testText" };
            StringParserAddValue.Parse(args);
            Assert.AreEqual("testText", StringParserAddValue.StringArg.GetValue(0));
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
        [Category("ParameterAccept")]
        public void Mandatory()
        {
            ParserTestMandatory par = new ParserTestMandatory();
            string[] args = new string[] { "-s", "String text" };
            Assert.Throws<ParseException>(delegate { par.Parse(args); });
        }
        [Test]
        [Category("ParameterAccept")]
        public void AtLeastOne()
        {
            ParserTestAtLeastOne par = new ParserTestAtLeastOne();
            string[] args = new string[] { "-s", "String Text" };
            Assert.Throws<ParseException>(delegate { par.Parse(args); });
        }
        [Test]
        [Category("ParameterAccept")]
        public void MinParamFail()
        {
            ParserTestMinParam par = new ParserTestMinParam();
            string[] args = new string[] { "-i", "10","20"};
            Assert.Throws<ParseException>(delegate { par.Parse(args); });
        }
    }
}
