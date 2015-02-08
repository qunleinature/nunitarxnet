// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2014.9.23：
//      1.利用cad命令直接测试
//      2.测试未通过，可能是在CAD环境下不支持程序域下的测试
//  2015.2.8：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System.IO;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.Assemblies;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.TestDomainArxNetTests_MultipleCommands))]
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.TestDomainArxNetTests_MultipleFixtureCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class TestDomainArxNetTests_MultipleCommands
    {
        //public void BuildSuite()
        [CommandMethod("BuildSuite")]
        public void BuildSuite()
        {
            TestDomainArxNetTests_Multiple tests = new TestDomainArxNetTests_Multiple();
            tests.Init();
            tests.BuildSuite();
            tests.UnloadTestDomain();
        }

        //public void RootNode()
        [CommandMethod("RootNode")]
        public void RootNode()
        {
            TestDomainArxNetTests_Multiple tests = new TestDomainArxNetTests_Multiple();
            tests.Init();
            tests.RootNode();
            tests.UnloadTestDomain();
        }

        //public void AssemblyNodes()
        [CommandMethod("AssemblyNodes")]
        public void AssemblyNodes()
        {
            TestDomainArxNetTests_Multiple tests = new TestDomainArxNetTests_Multiple();
            tests.Init();
            tests.AssemblyNodes();
            tests.UnloadTestDomain();
        }

        //public void TestCaseCount()
        [CommandMethod("TestCaseCount")]
        public void TestCaseCount()
        {
            TestDomainArxNetTests_Multiple tests = new TestDomainArxNetTests_Multiple();
            tests.Init();
            tests.TestCaseCount();
            tests.UnloadTestDomain();
        }

        //public void RunMultipleAssemblies()
        [CommandMethod("RunMultipleAssemblies")]
        public void RunMultipleAssemblies()
        {
            TestDomainArxNetTests_Multiple tests = new TestDomainArxNetTests_Multiple();
            tests.Init();
            tests.RunMultipleAssemblies();
            tests.UnloadTestDomain();
        }
    }

    public class TestDomainArxNetTests_MultipleFixtureCommands
    {
        //public void LoadFixture()
        [CommandMethod("LoadFixture")]
        public void LoadFixture()
        {
            TestDomainArxNetTests_MultipleFixture tests = new TestDomainArxNetTests_MultipleFixture();
            tests.LoadFixture();
        }
    }
}
