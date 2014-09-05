// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.1.26：
//  在NUnit2.6.2基础上修改
// ****************************************************************

namespace NUnit.CommandRunner.ArxNet.Tests
{
    using System;
    using System.Collections;
    using NUnit.Core;
    using NUnit.Framework;

    using NUnit.CommandRunner.ArxNet;//2013.1.26改

	[TestFixture]
	public class CommandLineTests_MultipleAssemblies
	{
        private readonly string firstAssembly = "nunit.tests.dll";
        private readonly string secondAssembly = "mock-assembly.dll";
        private readonly string fixture = "NUnit.Tests.CommandLine";
        private CommandOptionsArxNet assemblyOptions;//2013.1.26改
        private CommandOptionsArxNet fixtureOptions;//2013.1.26改

		[SetUp]
		public void SetUp()
		{
            assemblyOptions = new CommandOptionsArxNet(new string[] 
                { firstAssembly, secondAssembly });//2013.1.26改
            fixtureOptions = new CommandOptionsArxNet(new string[] 
                { "-fixture:" + fixture, firstAssembly, secondAssembly });//2013.1.26改
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
