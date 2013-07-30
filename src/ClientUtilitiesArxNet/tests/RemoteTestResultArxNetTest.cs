// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
//  2013.7.30
//   1.ÔÚNUnit2.6.2»ù´¡
//   2.¸ÄTestDomainArxNet
// ****************************************************************

using System;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Util.ArxNet.Tests
{
	[TestFixture]
	public class RemoteTestResultTest
	{
        private static readonly string mockDll = 
            NUnit.Tests.Assemblies.MockAssembly.AssemblyPath; 
        private TestDomainArxNet  domain;

        [SetUp]
        public void CreateRunner()
        {
            domain = new TestDomainArxNet ();
        }

        [TearDown]
        public void UnloadRunner()
        {
            if ( domain != null )
                domain.Unload();
        }

		[Test]
		public void ResultStillValidAfterDomainUnload() 
		{
            //TODO: This no longer appears to test anything
			TestPackage package = new TestPackage( mockDll );
			Assert.IsTrue( domain.Load( package ) );
			TestResult result = domain.Run( new NullListener(), TestFilter.Empty, false, LoggingThreshold.Off );
			TestResult caseResult = findCaseResult(result);
			Assert.IsNotNull(caseResult);
            //TestResultItem item = new TestResultItem(caseResult);
            //string message = item.GetMessage();
            //Assert.IsNotNull(message);
		}

        [Test, Explicit("Fails intermittently")]
        public void AppDomainUnloadedBug()
        {
            TestDomainArxNet  domain = new TestDomainArxNet();
            domain.Load( new TestPackage( mockDll ) );
            domain.Run(new NullListener(), TestFilter.Empty, false, LoggingThreshold.Off);
            domain.Unload();
        }

		private TestResult findCaseResult(TestResult suite) 
		{
			foreach (TestResult r in suite.Results) 
			{
				if (!r.Test.IsSuite)
				{
					return r;
				}
				else 
				{
					TestResult result = findCaseResult(r);
					if (result != null)
						return result;
				}

			}

			return null;
		}
	}
}
