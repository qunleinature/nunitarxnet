// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.1.26：
//  在NUnit2.6.2基础上修改
// 2013.6.28
//  1.已改CommandOptionsArxNet
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.8.21：
//  在NUnit2.6.3基础上修改
// ****************************************************************

namespace NUnit.CommandRunner.ArxNet.Tests//2013.1.25改
{
	using System;
	using System.IO;
	using System.Reflection;
	using NUnit.Framework;

    using NUnit.CommandRunner.ArxNet;
    using NUnit.Core;

	[TestFixture]
	public class CommandLineTests
	{
		[Test]
		public void NoParametersCount()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet();
			Assert.IsTrue(options.NoArgs);
		}

		[Test]
		public void AllowForwardSlashDefaultsCorrectly()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet();
			Assert.AreEqual( Path.DirectorySeparatorChar != '/', options.AllowForwardSlash );
		}

		[TestCase( "nologo", "nologo")]
		[TestCase( "help", "help" )]
		[TestCase( "help", "?" )]
		[TestCase( "wait", "wait" )]
		[TestCase( "xmlConsole", "xmlConsole")]
		[TestCase( "labels", "labels")]
		[TestCase( "noshadow", "noshadow" )]
        //[TestCase( "nothread", "nothread" )]//2013.1.26改
		public void BooleanOptionAreRecognized( string fieldName, string option )
		{
			FieldInfo field = typeof(CommandOptionsArxNet).GetField( fieldName );
			Assert.IsNotNull( field, "Field '{0}' not found", fieldName );
			Assert.AreEqual( typeof(bool), field.FieldType, "Field '{0}' is wrong type", fieldName );

			CommandOptionsArxNet options = new CommandOptionsArxNet( "-" + option );
			Assert.AreEqual( true, (bool)field.GetValue( options ), "Didn't recognize -" + option );
			options = new CommandOptionsArxNet( "--" + option );
			Assert.AreEqual( true, (bool)field.GetValue( options ), "Didn't recognize --" + option );
			options = new CommandOptionsArxNet( false, "/" + option );
			Assert.AreEqual( false, (bool)field.GetValue( options ), "Incorrectly recognized /" + option );
			options = new CommandOptionsArxNet( true, "/" + option );
			Assert.AreEqual( true, (bool)field.GetValue( options ), "Didn't recognize /" + option );
		}

        /*2013.1.26加*/
        //nothread = true;
        [Test]
        public void NothreadOptionIsTrue()
        {
            FieldInfo field = typeof(CommandOptionsArxNet).GetField("nothread");
            Assert.IsNotNull(field, "Field 'nothread' not found");
            Assert.AreEqual(typeof(bool), field.FieldType, "Field 'nothread' is wrong type");

            CommandOptionsArxNet options = new CommandOptionsArxNet();
            Assert.AreEqual(true, (bool)field.GetValue(options), "Field 'nothread' is not true");
            options = new CommandOptionsArxNet("-nothread");
            Assert.AreEqual(true, (bool)field.GetValue(options), "Field 'nothread' is not true");
            options = new CommandOptionsArxNet("--nothread");
            Assert.AreEqual(true, (bool)field.GetValue(options), "Field 'nothread' is not true");
            options = new CommandOptionsArxNet(false, "/nothread");
            Assert.AreEqual(true, (bool)field.GetValue(options), "Field 'nothread' is not true");
            options = new CommandOptionsArxNet(true, "/nothread");
            Assert.AreEqual(true, (bool)field.GetValue(options), "Field 'nothread' is not true");            
        }
        /*2013.1.26加*/

        /*2013.1.26加*/
        //process = ProcessModel.Single;
        [TestCase("Single")]
        [TestCase("Separate")]
        [TestCase("Multiple")]
        [Test]
        public void ProcessModelOptionIsSingle(string value)
        {
            FieldInfo field = typeof(CommandOptionsArxNet).GetField("process");
            Assert.IsNotNull(field, "Field 'process' not found");
            Assert.AreEqual(typeof(ProcessModel), field.FieldType, "process 'nothread' is wrong type");

            CommandOptionsArxNet options = new CommandOptionsArxNet();
            Assert.AreEqual(ProcessModel.Single, (ProcessModel)field.GetValue(options), "Field 'process' is not Single");
            options = new CommandOptionsArxNet("-process:" + value);
            Assert.AreEqual(ProcessModel.Single, (ProcessModel)field.GetValue(options), "Field 'process' is not Single");
            options = new CommandOptionsArxNet("--process" + value);
            Assert.AreEqual(ProcessModel.Single, (ProcessModel)field.GetValue(options), "Field 'process' is not Single");
            options = new CommandOptionsArxNet(false, "/process" + value);
            Assert.AreEqual(ProcessModel.Single, (ProcessModel)field.GetValue(options), "Field 'process' is not Single");
            options = new CommandOptionsArxNet(true, "/process" + value);
            Assert.AreEqual(ProcessModel.Single, (ProcessModel)field.GetValue(options), "Field 'process' is not Single");            
        }
        /*2013.1.26加*/

        /*2013.1.26加*/
        //domain = DomainUsage.None;
        [TestCase("None")]
        [TestCase("Single")]
        [TestCase("Multiple")]
        [Test]
        public void DomainUsageOptionIsNone(string value)
        {
            FieldInfo field = typeof(CommandOptionsArxNet).GetField("domain");
            Assert.IsNotNull(field, "Field 'domain' not found");
            Assert.AreEqual(typeof(DomainUsage), field.FieldType, "process 'domain' is wrong type");

            CommandOptionsArxNet options = new CommandOptionsArxNet();
            Assert.AreEqual(DomainUsage.None, (DomainUsage)field.GetValue(options), "Field 'domain' is not Single");
            options = new CommandOptionsArxNet("-domain:" + value);
            Assert.AreEqual(DomainUsage.None, (DomainUsage)field.GetValue(options), "Field 'domain' is not Single");
            options = new CommandOptionsArxNet("--domain" + value);
            Assert.AreEqual(DomainUsage.None, (DomainUsage)field.GetValue(options), "Field 'domain' is not Single");
            options = new CommandOptionsArxNet(false, "/domain" + value);
            Assert.AreEqual(DomainUsage.None, (DomainUsage)field.GetValue(options), "Field 'domain' is not Single");
            options = new CommandOptionsArxNet(true, "/domain" + value);
            Assert.AreEqual(DomainUsage.None, (DomainUsage)field.GetValue(options), "Field 'domain' is not Single");
        }
        /*2013.1.26加*/

		[TestCase( "fixture", "fixture" )]
		[TestCase( "config", "config")]
        [TestCase( "result", "result")]
		[TestCase( "result", "xml" )]
		[TestCase( "output", "output" )]
		[TestCase( "output", "out" )]
		[TestCase( "err", "err" )]
        [TestCase( "include", "include" )]
		[TestCase( "exclude", "exclude" )]
        [TestCase("run", "run")]
        [TestCase("runlist", "runlist")]
        [TestCase("basepath", "basepath")]
        [TestCase("privatebinpath", "privatebinpath")]
		public void StringOptionsAreRecognized( string fieldName, string option )
		{
			FieldInfo field = typeof(CommandOptionsArxNet).GetField( fieldName );
			Assert.IsNotNull( field, "Field {0} not found", fieldName );
			Assert.AreEqual( typeof(string), field.FieldType );

			CommandOptionsArxNet options = new CommandOptionsArxNet( "-" + option + ":text" );
			Assert.AreEqual( "text", (string)field.GetValue( options ), "Didn't recognize -" + option );
			options = new CommandOptionsArxNet( "--" + option + ":text" );
			Assert.AreEqual( "text", (string)field.GetValue( options ), "Didn't recognize --" + option );
			options = new CommandOptionsArxNet( false, "/" + option + ":text" );
			Assert.AreEqual( null, (string)field.GetValue( options ), "Incorrectly recognized /" + option );
			options = new CommandOptionsArxNet( true, "/" + option + ":text" );
			Assert.AreEqual( "text", (string)field.GetValue( options ), "Didn't recognize /" + option );
		}

        [TestCase("domain")]
        [TestCase("trace")]
		public void EnumOptionsAreRecognized( string fieldName )
		{
			FieldInfo field = typeof(CommandOptionsArxNet).GetField( fieldName );
			Assert.IsNotNull( field, "Field {0} not found", fieldName );
			Assert.IsTrue( field.FieldType.IsEnum, "Field {0} is not an enum", fieldName );
		}

		[Test]
		public void AssemblyName()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet( "nunit.tests.dll" );
			Assert.AreEqual( "nunit.tests.dll", options.Parameters[0] );
		}

		[Test]
		public void FixtureNamePlusAssemblyIsValid()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet( "-fixture:NUnit.Tests.AllTests", "nunit.tests.dll" );
			Assert.AreEqual("nunit.tests.dll", options.Parameters[0]);
			Assert.AreEqual("NUnit.Tests.AllTests", options.fixture);
			Assert.IsTrue(options.Validate());
		}

		[Test]
		public void AssemblyAloneIsValid()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet( "nunit.tests.dll" );
			Assert.IsTrue(options.Validate(), "command line should be valid");
		}

		[Test]
		public void InvalidOption()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet( "-asembly:nunit.tests.dll" );
			Assert.IsFalse(options.Validate());
		}


		[Test]
		public void NoFixtureNameProvided()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet( "-fixture:", "nunit.tests.dll" );
			Assert.IsFalse(options.Validate());
		}

		[Test] 
		public void InvalidCommandLineParms()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet( "-garbage:TestFixture", "-assembly:Tests.dll" );
			Assert.IsFalse(options.Validate());
		}

		[Test]
		public void XmlParameter()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet( "tests.dll", "-xml:results.xml" );
			Assert.IsTrue(options.ParameterCount == 1, "assembly should be set");
			Assert.AreEqual("tests.dll", options.Parameters[0]);
			Assert.AreEqual("results.xml", options.result);
		}

		[Test]
		public void XmlParameterWithFullPath()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet( "tests.dll", "-xml:C:/nunit/tests/bin/Debug/console-test.xml" );
			Assert.IsTrue(options.ParameterCount == 1, "assembly should be set");
			Assert.AreEqual("tests.dll", options.Parameters[0]);
			Assert.AreEqual("C:/nunit/tests/bin/Debug/console-test.xml", options.result);
		}

		[Test]
		public void XmlParameterWithFullPathUsingEqualSign()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet( "tests.dll", "-xml=C:/nunit/tests/bin/Debug/console-test.xml" );
			Assert.IsTrue(options.ParameterCount == 1, "assembly should be set");
			Assert.AreEqual("tests.dll", options.Parameters[0]);
			Assert.AreEqual("C:/nunit/tests/bin/Debug/console-test.xml", options.result);
		}

		[Test]
		public void FileNameWithoutXmlParameterLooksLikeParameter()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet( "tests.dll", "result.xml" );
			Assert.IsTrue(options.Validate());
			Assert.AreEqual(2, options.Parameters.Count);
		}

		[Test]
		public void XmlParameterWithoutFileNameIsInvalid()
		{
			CommandOptionsArxNet options = new CommandOptionsArxNet( "tests.dll", "-xml:" );
			Assert.IsFalse(options.Validate());			
		}

		[Test]
		public void HelpTextUsesCorrectDelimiterForPlatform()
		{
			string helpText = new CommandOptionsArxNet().GetHelpText();
			char delim = System.IO.Path.DirectorySeparatorChar == '/' ? '-' : '/';

			string expected = string.Format( "{0}output=", delim );
			StringAssert.Contains( expected, helpText );
			
			expected = string.Format( "{0}out=", delim );
			StringAssert.Contains( expected, helpText );
		}
	}
}
