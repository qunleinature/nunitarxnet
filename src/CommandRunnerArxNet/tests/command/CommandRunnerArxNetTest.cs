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

        //public void SuccessFixture()
        [CommandMethod("SuccessFixture")]
        public void SuccessFixture()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.SuccessFixture();
            tests.CleanUp();
        }

        //public void XmlResult()
        [CommandMethod("XmlResult")]
        public void XmlResult()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.XmlResult();
            tests.CleanUp();
        }

        //public void InvalidFixture()
        [CommandMethod("InvalidFixture")]
        public void InvalidFixture()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.InvalidFixture();
            tests.CleanUp();
        }

        //public void AssemblyNotFound()
        [CommandMethod("AssemblyNotFound")]
        public void AssemblyNotFound()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.AssemblyNotFound();
            tests.CleanUp();
        }

        //public void OneOfTwoAssembliesNotFound()
        [CommandMethod("OneOfTwoAssembliesNotFound")]
        public void OneOfTwoAssembliesNotFound()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.OneOfTwoAssembliesNotFound();
            tests.CleanUp();
        }

        //public void XmlToConsole()
        [CommandMethod("XmlToConsole")]
        public void XmlToConsole()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.XmlToConsole();
            tests.CleanUp();
        }

        //public void Bug1073539Test()
        [CommandMethod("Bug1073539Test")]
        public void Bug1073539Test()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.Bug1073539Test();
            tests.CleanUp();
        }

        //public void Bug1311644Test()
        [CommandMethod("Bug1311644Test")]
        public void Bug1311644Test()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.Bug1311644Test();
            tests.CleanUp();
        }

        //public void CanRunWithoutTestDomain()
        [CommandMethod("CanRunWithoutTestDomain")]
        public void CanRunWithoutTestDomain()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.CanRunWithoutTestDomain();
            tests.CleanUp();
        }

        //public void CanRunWithSingleTestDomain()
        [CommandMethod("CanRunWithSingleTestDomain")]
        public void CanRunWithSingleTestDomain()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.CanRunWithSingleTestDomain();
            tests.CleanUp();
        }

        //public void CanRunWithMultipleTestDomains()
        [CommandMethod("CanRunWithMultipleTestDomains")]
        public void CanRunWithMultipleTestDomains()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.CanRunWithMultipleTestDomains();
            tests.CleanUp();
        }

        //public void CanRunWithoutTestDomain_NoThread()
        [CommandMethod("CanRunWithoutTestDomain_NoThread")]
        public void CanRunWithoutTestDomain_NoThread()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.CanRunWithoutTestDomain_NoThread();
            tests.CleanUp();
        }

        //public void CanRunWithSingleTestDomain_NoThread()
        [CommandMethod("CanRunWithSingleTestDomain_NoThread")]
        public void CanRunWithSingleTestDomain_NoThread()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.CanRunWithSingleTestDomain_NoThread();
            tests.CleanUp();
        }

        //public void CanRunWithMultipleTestDomains_NoThread()
        [CommandMethod("CanRunWithMultipleTestDomains_NoThread")]
        public void CanRunWithMultipleTestDomains_NoThread()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.CanRunWithMultipleTestDomains_NoThread();
            tests.CleanUp();
        }

        //public void CanSpecifyBasePathAndPrivateBinPath()
        [CommandMethod("CanSpecifyBasePathAndPrivateBinPath")]
        public void CanSpecifyBasePathAndPrivateBinPath()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.CanSpecifyBasePathAndPrivateBinPath();
            tests.CleanUp();
        }

        //public void DoesNotFailWithEmptyRunList()
        [CommandMethod("DoesNotFailWithEmptyRunList")]
        public void DoesNotFailWithEmptyRunList()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.DoesNotFailWithEmptyRunList();
            tests.CleanUp();
        }

        //public void DoesNotFailIfRunListHasEmptyLines()
        [CommandMethod("DoesNotFailIfRunListHasEmptyLines")]
        public void DoesNotFailIfRunListHasEmptyLines()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.DoesNotFailIfRunListHasEmptyLines();
            tests.CleanUp();
        }

        //public void FailsGracefullyIfRunListPointsToNonExistingFile()
        [CommandMethod("FailsGracefullyIfRunListPointsToNonExistingFile")]
        public void FailsGracefullyIfRunListPointsToNonExistingFile()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.FailsGracefullyIfRunListPointsToNonExistingFile();
            tests.CleanUp();
        }


        //public void FailsGracefullyIfRunListPointsToNonExistingDirectory()
        [CommandMethod("FailsGracefullyIfRunListPointsToNonExistingDirectory")]
        public void FailsGracefullyIfRunListPointsToNonExistingDirectory()
        {
            CommandRunnerArxNetTest tests = new CommandRunnerArxNetTest();
            tests.Init();
            tests.FailsGracefullyIfRunListPointsToNonExistingDirectory();
            tests.CleanUp();
        } 
    }
}
