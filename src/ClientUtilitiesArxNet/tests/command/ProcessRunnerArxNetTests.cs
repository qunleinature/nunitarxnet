// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2014.9.21：
//      1.利用cad命令直接测试
//      2.测试未通过,致命错误:可能在CAD环境下进程错误
//  2015.2.6：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System.Diagnostics;
using System.IO;
using NUnit.Core;
using NUnit.Core.Tests;
using NUnit.Framework;
using NUnit.Tests.Assemblies;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.ProcessRunnerArxNetTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class ProcessRunnerArxNetTestsCommands
    {
        //public void  TestProcessIsReused()
        [CommandMethod("TestProcessIsReused")]
        public void TestProcessIsReused()
        {
            ProcessRunnerArxNetTests tests = new ProcessRunnerArxNetTests();
            tests.TestProcessIsReused();
        }
    }
}
