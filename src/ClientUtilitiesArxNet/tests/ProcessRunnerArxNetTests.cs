// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
//  2013.7.7
//   1.在NUnit2.6.2基础
//   2.测试NUnit.Util.ArxNet.ProcessRunnerArxNet类
// 2013.7.18
//   1.测试未通过,nunit-agent.exe发生异常：可能在CAD环境下不能新建一个进程运行nunit-agent.exe？
// ****************************************************************

using System.Diagnostics;
using System.IO;
using NUnit.Core;
using NUnit.Core.Tests;
using NUnit.Framework;
using NUnit.Tests.Assemblies;

namespace NUnit.Util.ArxNet.Tests
{
	/// <summary>
	/// Summary description for ProcessRunnerTests.
	/// </summary>
    [TestFixture, Timeout(30000)]
    [Platform(Exclude = "Mono", Reason = "Process Start not working correctly")]
    public class ProcessRunnerArxNetTests : BasicRunnerTests
    {
        private ProcessRunnerArxNet myRunner;

        protected override TestRunner CreateRunner(int runnerID)
        {
            myRunner = new ProcessRunnerArxNet(runnerID);
            return myRunner;
        }

        protected override void DestroyRunner()
        {
            if (myRunner != null)
            {
                myRunner.Unload();
                myRunner.Dispose();
            }
        }

        [Test]
        public void  TestProcessIsReused()
        {
            TestPackage package = new TestPackage(MockAssembly.AssemblyPath);
            myRunner.Load(package);
            int processId = ((TestAssemblyInfo)myRunner.AssemblyInfo[0]).ProcessId;
            Assert.AreNotEqual(Process.GetCurrentProcess().Id, processId, "Not in separate process");
            myRunner.Unload();
            myRunner.Load(package);
            Assert.AreEqual(processId, ((TestAssemblyInfo)myRunner.AssemblyInfo[0]).ProcessId, "Reloaded in different process");
        }
    }
}
