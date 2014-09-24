// ****************************************************************
// Copyright 2012, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.9.23：
//  1.利用cad命令直接测试
// ****************************************************************

using System;
using NUnit.Core;
using NUnit.Framework;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.TestRunnerFactoryArxNetTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class TestRunnerFactoryArxNetTestsCommands
    {
        //public void SameFrameworkUsesTestDomain()
        [CommandMethod("SameFrameworkUsesTestDomain")]
        public void SameFrameworkUsesTestDomain()
        {
            TestRunnerFactoryArxNetTests tests = new TestRunnerFactoryArxNetTests();
            tests.Init();
            tests.SameFrameworkUsesTestDomain();
        }

        //public void DifferentRuntimeUsesProcessRunner()
        [CommandMethod("DifferentRuntimeUsesProcessRunner")]
        public void DifferentRuntimeUsesProcessRunner()
        {
            TestRunnerFactoryArxNetTests tests = new TestRunnerFactoryArxNetTests();
            tests.Init();
            tests.DifferentRuntimeUsesProcessRunner();
        }

        //public void DifferentVersionUsesProcessRunner()
        [CommandMethod("DifferentVersionUsesProcessRunner")]
        public void DifferentVersionUsesProcessRunner()
        {
            TestRunnerFactoryArxNetTests tests = new TestRunnerFactoryArxNetTests();
            tests.Init();
            tests.DifferentVersionUsesProcessRunner();
        }
    }
}
