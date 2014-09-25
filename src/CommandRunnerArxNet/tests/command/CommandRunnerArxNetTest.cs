// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
//  2014.9.26：
//      利用cad命令直接测试
// ****************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections;

using NUnit.Framework;
using NUnit.Core;
using NUnit.TestData.ConsoleRunnerTest;
using NUnit.Tests.Assemblies;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.CommandRunner.ArxNet.Tests.CommandRunnerArxNetTestCommands))]

namespace NUnit.CommandRunner.ArxNet.Tests
{
    public class CommandRunnerArxNetTestCommands
    {
        //public void FailureFixture()
        [CommandMethod("FailureFixture")]
        public void FailureFixture()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.FailureFixture();
            tests.CleanUp();
        }

        //public void MultiFailureFixture()
        [CommandMethod("MultiFailureFixture")]
        public void MultiFailureFixture()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.MultiFailureFixture();
            tests.CleanUp();
        }
    }
}
