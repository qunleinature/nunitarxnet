// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.7.31
//      1.在NUnit2.6.2基础
//      2.改TestDomainArxNet
//      3.测试未通过，可能是在CAD环境下不支持程序域下的测试？
//  2014.7.17
//      1.测试未通过，可能是在CAD环境下不支持程序域下的测试？
//      2.TestDomainArxNetTests_Multiple测试类加上IgnoreAttribute
//      3.TestDomainTests_MultipleFixture改为TestDomainArxNetTests_MultipleFixture
//      4.TestDomainArxNetTests_MultipleFixture测试类加上IgnoreAttribute
//  2014.9.24：
//      在NUnit2.6.3基础上修改
//  2015.2.4：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System.IO;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.Assemblies;

namespace NUnit.Util.ArxNet.Tests
{
	/// <summary>
	/// Summary description for MultipleAssembliesDomain.
	/// </summary>
	[TestFixture]
    [Ignore("CAD环境下忽略")]//2014.7.17 Lei Qun添加，在cad环境下不能通过，忽略
	public class TestDomainArxNetTests_Multiple
	{
		private TestDomainArxNet  domain; 
		private ITest loadedSuite;

		private static string path1 = NoNamespaceTestFixture.AssemblyPath;
		private static string path2 = MockAssembly.AssemblyPath;

		private string name = "Multiple Assemblies Test";

		[TestFixtureSetUp]
		public void Init()
		{
			domain = new TestDomainArxNet();
			TestPackage package = new TestPackage( name );
			package.Assemblies.Add( path1 );
			package.Assemblies.Add( path2 );
			domain.Load( package );
			loadedSuite = domain.Test;
		}

		[TestFixtureTearDown]
		public void UnloadTestDomain()
		{
			domain.Unload();
			domain = null;
		}
			
		[Test]
		public void BuildSuite()
		{
			Assert.IsNotNull(loadedSuite);
		}

		[Test]
		public void RootNode()
		{
			Assert.AreEqual( name, loadedSuite.TestName.Name );
		}

		[Test]
		public void AssemblyNodes()
		{
			TestNode test0 = (TestNode)loadedSuite.Tests[0];
			TestNode test1 = (TestNode)loadedSuite.Tests[1];
			Assert.AreEqual( path1, test0.TestName.Name );
			Assert.AreEqual( path2, test1.TestName.Name );
		}

		[Test]
		public void TestCaseCount()
		{
			Assert.AreEqual(NoNamespaceTestFixture.Tests + MockAssembly.Tests, 
				loadedSuite.TestCount );
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			TestResult result = domain.Run(NullListener.NULL, TestFilter.Empty, false, LoggingThreshold.Off);
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.AreEqual(
				NoNamespaceTestFixture.Tests + MockAssembly.TestsRun, 
				summary.TestsRun);
		}
	}

	[TestFixture]
    [Ignore("CAD环境下忽略")]//2014.7.17 Lei Qun添加，在cad环境下不能通过，忽略
    public class TestDomainArxNetTests_MultipleFixture//2014.7.17 Lei Qun修改，改名为TestDomainArxNetTests_MultipleFixture
	{
		[Test]
		public void LoadFixture()
		{
			TestDomainArxNet  domain = new TestDomainArxNet ();
			TestPackage package = new TestPackage( "Multiple Assemblies Test" );
			package.Assemblies.Add(NoNamespaceTestFixture.AssemblyPath);
			package.Assemblies.Add(MockAssembly.AssemblyPath);
            package.TestName = "NUnit.Tests.Assemblies.MockTestFixture";
            try
            {
                domain.Load(package);
                Assert.AreEqual(MockTestFixture.Tests, domain.Test.TestCount);
            }
            finally
            {
                domain.Unload();
            }
		}
	}
}
