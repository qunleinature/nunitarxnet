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
//   3.测试未通过，可能是在CAD环境下不支持程序域下的测试？
//  2014.7.17
//   1.测试未通过，可能是在CAD环境下不支持程序域下的测试？
//   2.TestDomainArxNetFixture测试类加上IgnoreAttribute
//  2014.7.18
//   1.TestDomainRunnerTests改为TestDomainRunnerArxNetTests
//   2.TestDomainTests改为TestDomainArxNetTests
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.9.24：
//  在NUnit2.6.3基础上修改
// ****************************************************************

using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.Assemblies;

namespace NUnit.Util.ArxNet.Tests
{
	[TestFixture]
    [Ignore("CAD环境下忽略")]//2014.7.17 Lei Qun添加，在cad环境下不能通过，忽略
	public class TestDomainArxNetFixture
	{
		private static TestDomainArxNet  testDomain; 
		private static ITest loadedTest;
        private static readonly string mockDll = MockAssembly.AssemblyPath;

		[TestFixtureSetUp]
		public static void MakeAppDomain()
		{
			testDomain = new TestDomainArxNet();
			testDomain.Load( new TestPackage(mockDll));
			loadedTest = testDomain.Test;
		}

		[TestFixtureTearDown]
		public static void UnloadTestDomain()
		{
            if ( testDomain != null )
                testDomain.Unload();
			loadedTest = null;
			testDomain = null;
		}
			
		[Test]
		public void AssemblyIsLoadedCorrectly()
		{
			Assert.IsNotNull(loadedTest, "Test not loaded");
			Assert.AreEqual(MockAssembly.Tests, loadedTest.TestCount );
		}

        [Test, Platform("Linux,Net", Reason = "get_SetupInformation() fails on Windows+Mono")]
		public void AppDomainIsSetUpCorrectly()
		{
			AppDomain domain = testDomain.AppDomain;
			AppDomainSetup setup = testDomain.AppDomain.SetupInformation;

            Assert.That(setup.ApplicationName, Is.StringStarting("Tests_"));
			Assert.That(setup.ApplicationBase, Is.SamePath(Path.GetDirectoryName(mockDll)), "ApplicationBase");
			Assert.That( 
                Path.GetFileName( setup.ConfigurationFile ),
                Is.EqualTo("mock-assembly.dll.config").IgnoreCase,
                "ConfigurationFile");
			Assert.AreEqual( null, setup.PrivateBinPath, "PrivateBinPath" );
			Assert.That(setup.ShadowCopyDirectories, Is.SamePath(Path.GetDirectoryName(mockDll)), "ShadowCopyDirectories" );

			Assert.That(domain.BaseDirectory, Is.SamePath(Path.GetDirectoryName(mockDll)), "BaseDirectory" );
            Assert.That(domain.FriendlyName, 
                Is.EqualTo("test-domain-mock-assembly.dll").IgnoreCase, "FriendlyName");
			Assert.IsTrue( testDomain.AppDomain.ShadowCopyFiles, "ShadowCopyFiles" );
		}	

		[Test]
		public void CanRunMockAssemblyTests()
		{
			TestResult result = testDomain.Run( NullListener.NULL, TestFilter.Empty, false, LoggingThreshold.Off );
			Assert.IsNotNull(result);

            ResultSummarizer summarizer = new ResultSummarizer(result);
            Assert.AreEqual(MockAssembly.TestsRun, summarizer.TestsRun, "TestsRun");
            Assert.AreEqual(MockAssembly.Ignored, summarizer.Ignored, "Ignored");
            Assert.AreEqual(MockAssembly.Errors, summarizer.Errors, "Errors");
            Assert.AreEqual(MockAssembly.Failures, summarizer.Failures, "Failures");
        }
	}

	[TestFixture]
    [Ignore("CAD环境下忽略")]//2014.7.18 Lei Qun添加，在cad环境下不能通过，忽略
    public class TestDomainRunnerArxNetTests : NUnit.Core.Tests.BasicRunnerTests//2017.7.18 Lei Qun修改，改为TestDomainRunnerArxNetTests
	{
		protected override TestRunner CreateRunner(int runnerID)
		{
			return new TestDomainArxNet (runnerID);
		}
	}

	[TestFixture]
    [Ignore("CAD环境下忽略")]//2014.7.18 Lei Qun添加，在cad环境下不能通过，忽略
    public class TestDomainArxNetTests//2017.7.18 Lei Qun修改，改为TestDomainArxNetTests
	{ 
		private TestDomainArxNet  testDomain;
        private static readonly string mockDll = MockAssembly.AssemblyPath;

		[SetUp]
		public void SetUp()
		{
			testDomain = new TestDomainArxNet ();
		}

		[TearDown]
		public void TearDown()
		{
			testDomain.Unload();
		}

		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void FileNotFound()
		{
			testDomain.Load( new TestPackage( "/xxxx.dll" ) );
		}

		[Test]
		public void InvalidTestFixture()
		{
			TestPackage package = new TestPackage(mockDll);
			package.TestName = "NUnit.Tests.Assemblies.Bogus";
			Assert.IsFalse( testDomain.Load( package ) );
		}

		// Doesn't work under .NET 2.0 Beta 2
		//[Test]
		//[ExpectedException(typeof(BadImageFormatException))]
		public void FileFoundButNotValidAssembly()
		{
			string badfile = Path.GetFullPath("x.dll");
			try
			{
				StreamWriter sw = new StreamWriter( badfile );
				//StreamWriter sw = file.AppendText();

				sw.WriteLine("This is a new entry to add to the file");
				sw.WriteLine("This is yet another line to add...");
				sw.Flush();
				sw.Close();
				testDomain.Load( new TestPackage( badfile ) );
			}
			finally
			{
				if ( File.Exists( badfile ) )
					File.Delete( badfile );
			}

		}

		[Test]
		public void SpecificTestFixture()
		{
			TestPackage package = new TestPackage(mockDll);
			package.TestName = "NUnit.Tests.Assemblies.MockTestFixture";
			testDomain.Load( package );

			TestResult result = testDomain.Run( NullListener.NULL, TestFilter.Empty, false, LoggingThreshold.Off );

            ResultSummarizer summarizer = new ResultSummarizer(result);
            Assert.AreEqual(MockTestFixture.TestsRun, summarizer.TestsRun, "TestsRun");
            Assert.AreEqual(MockTestFixture.Ignored, summarizer.Ignored, "Ignored");
            Assert.AreEqual(MockTestFixture.Errors, summarizer.Errors, "Errors");
            Assert.AreEqual(MockTestFixture.Failures, summarizer.Failures, "Failures");
        }

        [Test, Platform("Linux,Net", Reason = "get_SetupInformation() fails on Windows+Mono")]
		public void ConfigFileOverrideIsHonored()
		{
			TestPackage package = new TestPackage( "MyProject.nunit" );
			package.Assemblies.Add(mockDll);
			package.ConfigurationFile = "override.config";

			testDomain.Load( package );

			Assert.AreEqual( "override.config", 
				Path.GetFileName( testDomain.AppDomain.SetupInformation.ConfigurationFile ) );
		}

		[Test]
		public void BasePathOverrideIsHonored()
		{
			TestPackage package = new TestPackage( "MyProject.nunit" );
			package.Assemblies.Add( MockAssembly.AssemblyPath );
			package.BasePath = Path.GetDirectoryName( Environment.CurrentDirectory );
			package.PrivateBinPath = Path.GetFileName( Environment.CurrentDirectory );

			testDomain.Load( package );

			Assert.That(testDomain.AppDomain.BaseDirectory, Is.SamePath(package.BasePath));
		}

        [Test, Platform("Linux,Net", Reason = "get_SetupInformation() fails on Windows+Mono")]
		public void BinPathOverrideIsHonored()
		{
			TestPackage package = new TestPackage( "MyProject.nunit" );
			package.Assemblies.Add( MockAssembly.AssemblyPath );
			package.PrivateBinPath = "dummy;junk";

			testDomain.Load( package );

			Assert.AreEqual( "dummy;junk", 
				testDomain.AppDomain.SetupInformation.PrivateBinPath );
		}

		// Turning off shadow copy only works when done for the primary app domain
		// So this test can only work if it's already off
		// This doesn't seem to be documented anywhere
		[Test, Platform(Exclude="mono-1.0", Reason="Test hangs under the 1.0 profile")]
		public void TurnOffShadowCopy()
		{
			TestPackage package = new TestPackage(mockDll);
			package.Settings["ShadowCopyFiles"] = false;
			testDomain.Load( package );
			Assert.IsFalse( testDomain.AppDomain.ShadowCopyFiles );
					
			// Prove that shadow copy is really off
//			string location = "NOT_FOUND";
//			foreach( Assembly assembly in testDomain.AppDomain.GetAssemblies() )
//			{
//				if ( assembly.FullName.StartsWith( "mock-assembly" ) )
//				{
//					location = Path.GetDirectoryName( assembly.Location );
//					break;
//				}
//			}
//		
//			StringAssert.StartsWith( AppDomain.CurrentDomain.BaseDirectory.ToLower(), location.ToLower() );
		}
	}
}
