// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
//  2013.7.30
//   1.在NUnit2.6.2基础
//   2.改TestDomainArxNet
//   3.测试未通过,可能是在CAD环境下不支持程序域下的测试？
//  2014.7.17
//   1.测试未通过,可能是在CAD环境下不支持程序域下的测试？
//   2.RemoteTestResultArxNetTest测试类加上IgnoreAttribute
// ****************************************************************

using System;
using NUnit.Framework;
using NUnit.Core;

namespace NUnit.Util.ArxNet.Tests
{
	[TestFixture]
    [Ignore("CAD环境下忽略")]//2014.7.17 Lei Qun添加，在cad环境下不能通过，忽略
	public class RemoteTestResultArxNetTest
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
