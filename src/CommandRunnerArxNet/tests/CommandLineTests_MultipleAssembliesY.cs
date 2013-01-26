// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
//2012年1月16日，雷群修改
// ****************************************************************

namespace NUnit.CommandRunner.ArxNet.Tests
{
	using System;
	using System.Collections;
    using NUnit.Core;
	using NUnit.Framework;

    using NUnit.CommandRunner.ArxNet;

	[TestFixture]
	public class CommandLineTests_MultipleAssemblies
	{
		private readonly string firstAssembly = "nunit.tests.dll";
		private readonly string secondAssembly = "mock-assembly.dll";
		private readonly string fixture = "NUnit.Tests.CommandLine";
		private CommandOptionsArxNet assemblyOptions;
		private CommandOptionsArxNet fixtureOptions;

		[SetUp]
		public void SetUp()
		{
			assemblyOptions = new CommandOptionsArxNet(new string[]
				{ firstAssembly, secondAssembly });
			fixtureOptions = new CommandOptionsArxNet(new string[]
				{ "-fixture:"+fixture, firstAssembly, secondAssembly });
		}

		[Test]
		public void MultipleAssemblyValidate()
		{
			Assert.IsTrue(assemblyOptions.Validate());
		}

		[Test]
		public void ParameterCount()
		{
			Assert.AreEqual(2, assemblyOptions.Parameters.Count);
		}

		[Test]
		public void CheckParameters()
		{
			ArrayList parms = assemblyOptions.Parameters;
			Assert.IsTrue(parms.Contains(firstAssembly));
			Assert.IsTrue(parms.Contains(secondAssembly));
		}

		[Test]
		public void FixtureValidate()
		{
			Assert.IsTrue(fixtureOptions.Validate());
		}

		[Test]
		public void FixtureParameters()
		{
			Assert.AreEqual(fixture, fixtureOptions.fixture);
			ArrayList parms = fixtureOptions.Parameters;
			Assert.IsTrue(parms.Contains(firstAssembly));
			Assert.IsTrue(parms.Contains(secondAssembly));
		}
	}
}
