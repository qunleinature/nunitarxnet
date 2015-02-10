// ****************************************************************
// Copyright 2002-2003, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.5.28修改：
//      1.在nunit2.6.2基础上修改
//  2013.6.9
//      1.已改TestProgressBarArxNet
//  2015.2.9：
//      在NUnit2.6.4基础上修改
// ****************************************************************

namespace NUnit.UiKit.ArxNet.Tests
{
	using System;
	using System.Drawing;
	using NUnit.Framework;
	using NUnit.Core;
	using NUnit.Util;
	using NUnit.Tests.Assemblies;
	using NUnit.TestUtilities;

	/// <summary>
	/// Summary description for ProgressBarTests.
	/// </summary>
	[TestFixture]
	public class ProgressBarArxNetTests
	{
		private TestProgressBarArxNet progressBar;
		private MockTestEventSource mockEvents;
		private string testsDll = MockAssembly.AssemblyPath;
		private TestSuite suite;
		int testCount;

		[SetUp]
		public void Setup()
		{
			progressBar = new TestProgressBarArxNet();

			TestSuiteBuilder builder = new TestSuiteBuilder();
			suite = builder.Build( new TestPackage( testsDll ) );

			mockEvents = new MockTestEventSource( suite );
		}

        // .NET 1.0 sometimes throws:
        // ExternalException : A generic error occurred in GDI+.
        [Test, Platform(Exclude = "Net-1.0")]
        public void TestProgressDisplay()
		{
			progressBar.Subscribe( mockEvents );
			mockEvents.TestFinished += new TestEventHandler( OnTestFinished );

			testCount = 0;
			mockEvents.SimulateTestRun();
			
			Assert.AreEqual( 0, progressBar.Minimum );
			Assert.AreEqual( MockAssembly.Tests, progressBar.Maximum );
			Assert.AreEqual( 1, progressBar.Step );
			Assert.AreEqual( MockAssembly.ResultCount, progressBar.Value );
			Assert.AreEqual( Color.Red, progressBar.ForeColor );
		}

		private void OnTestFinished( object sender, TestEventArgs e )
		{
			++testCount;
			// Assumes delegates are called in order of adding
			Assert.AreEqual( testCount, progressBar.Value );
		}
	}
}
