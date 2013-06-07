// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
//2012年8月25日，雷群修改
//2012-6-7
//  1.已改GuiOptionsArxNet
//  2.改在nunit2.6.2基础
// ****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace NUnit.Gui.ArxNet.Tests
{
	[TestFixture]
	public class CommandLineArxNetTests
	{
		[Test]
		public void NoParametersCount()
		{
			GuiOptionsArxNet options = new GuiOptionsArxNet(new string[] {});
			Assert.IsTrue(options.NoArgs);
		}

		[Test]
		public void Help()
		{
			GuiOptionsArxNet options = new GuiOptionsArxNet(new string[] {"-help"});
			Assert.IsTrue(options.help);
		}

		[Test]
		public void ShortHelp()
		{
			GuiOptionsArxNet options = new GuiOptionsArxNet(new string[] {"-?"});
			Assert.IsTrue(options.help);
		}

		[Test]
		public void AssemblyName()
		{
			string assemblyName = "nunit.tests.dll";
			GuiOptionsArxNet options = new GuiOptionsArxNet(new string[]{ assemblyName });
			Assert.AreEqual(assemblyName, options.Parameters[0]);
		}

		[Test]
		public void ValidateSuccessful()
		{
			GuiOptionsArxNet options = new GuiOptionsArxNet(new string[] { "nunit.tests.dll" });
			Assert.IsTrue(options.Validate(), "command line should be valid");
		}

		[Test]
		public void InvalidArgs()
		{
			GuiOptionsArxNet options = new GuiOptionsArxNet(new string[] { "-asembly:nunit.tests.dll" });
			Assert.IsFalse(options.Validate());
		}


		[Test] 
		public void InvalidCommandLineParms()
		{
			GuiOptionsArxNet parser = new GuiOptionsArxNet(new String[]{"-garbage:TestFixture", "-assembly:Tests.dll"});
			Assert.IsFalse(parser.Validate());
		}

		[Test] 
		public void NoNameValuePairs()
		{
			GuiOptionsArxNet parser = new GuiOptionsArxNet(new String[]{"TestFixture", "Tests.dll"});
			Assert.IsFalse(parser.Validate());
		}

		[Test]
		public void HelpTextUsesCorrectDelimiterForPlatform()
		{
			string helpText = new GuiOptionsArxNet(new String[] {"Tests.dll"} ).GetHelpText();
			char delim = System.IO.Path.DirectorySeparatorChar == '/' ? '-' : '/';

			string expected = string.Format( "{0}config=", delim );
			StringAssert.Contains( expected, helpText );
		}
	}
}

