// ****************************************************************
// Copyright 2012, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.5.28修改
//   1.nunit2.6.2基础上修改
//   2.改DefaultTestRunnerFactory为DefaultTestRunnerFactoryArxNet
//   3.改ProcessRunner为ProcessRunnerArxNet
// 2013.7.31
//   1.改TestDomainArxNet
// ****************************************************************

using System;
using NUnit.Core;
using NUnit.Framework;

namespace NUnit.Util.ArxNet.Tests
{
    [TestFixture]
    public class TestRunnerFactoryArxNetTests
    {
        private RuntimeFramework currentFramework = RuntimeFramework.CurrentFramework;
        private string testDll = "/test.dll";
        private DefaultTestRunnerFactoryArxNet factory;
        private TestPackage package;

        [SetUp]
        public void Init()
        {
            factory = new DefaultTestRunnerFactoryArxNet();
            package = new TestPackage(testDll);
        }

        [Test]
        public void SameFrameworkUsesTestDomain()
        {
            package.Settings["RuntimeFramework"] = currentFramework;
            //Assert.That( factory.MakeTestRunner(package), Is.TypeOf(typeof(TestDomainArxNet)));
            Assert.That(factory.MakeTestRunner(package), Is.TypeOf(typeof(RemoteTestRunner)));//CAD环境下测试包是单进程、无应用域
        }

#if CLR_2_0 || CLR_4_0
        [Test]
        public void DifferentRuntimeUsesProcessRunner()
        {
            RuntimeType runtime = currentFramework.Runtime == RuntimeType.Net
                ? RuntimeType.Mono : RuntimeType.Net;
            package.Settings["RuntimeFramework"] = new RuntimeFramework(runtime, currentFramework.ClrVersion);
            //Assert.That(factory.MakeTestRunner(package), Is.TypeOf(typeof(ProcessRunnerArxNet)));
            Assert.That(factory.MakeTestRunner(package), Is.TypeOf(typeof(RemoteTestRunner)));//CAD环境下测试包是单进程、无应用域
        }

        [Test]
        public void DifferentVersionUsesProcessRunner()
        {
            int major = currentFramework.ClrVersion.Major == 2 ? 4 : 2;
            package.Settings["RuntimeFramework"] = new RuntimeFramework(currentFramework.Runtime, new Version(major,0));
            //Assert.That(factory.MakeTestRunner(package), Is.TypeOf(typeof(ProcessRunnerArxNet)));
            Assert.That(factory.MakeTestRunner(package), Is.TypeOf(typeof(RemoteTestRunner)));//CAD环境下测试包是单进程、无应用域
        }
#endif
    }
}
