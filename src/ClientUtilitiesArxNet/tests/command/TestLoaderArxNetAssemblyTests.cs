// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.9.23：
//  1.利用cad命令直接测试
// ****************************************************************

using System;
using System.IO;
using System.Threading;
using System.Reflection;

using NUnit.Core;
using NUnit.Framework;
using NUnit.Tests.Assemblies;
using NUnit.Util;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.TestLoaderArxNetAssemblyTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class TestLoaderArxNetAssemblyTestsCommands
    {
        //public void LoadProject()
        [CommandMethod("LoadProject")]
        public void LoadProject()
        {
            TestLoaderArxNetAssemblyTests tests = new TestLoaderArxNetAssemblyTests();
            tests.SetUp();
            tests.LoadProject();
            tests.TearDown();
        }

        //public void UnloadProject()
        [CommandMethod("UnloadProject")]
        public void UnloadProject()
        {
            TestLoaderArxNetAssemblyTests tests = new TestLoaderArxNetAssemblyTests();
            tests.SetUp();
            tests.UnloadProject();
            tests.TearDown();
        }

        //public void LoadTest()
        [CommandMethod("LoadTest")]
        public void LoadTest()
        {
            TestLoaderArxNetAssemblyTests tests = new TestLoaderArxNetAssemblyTests();
            tests.SetUp();
            tests.LoadTest();
            tests.TearDown();
        }
    }
}
