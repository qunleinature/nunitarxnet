// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.9.23：
//  1.利用cad命令直接测试
//  2.测试未通过，可能是在CAD环境下不支持程序域下的测试
// ****************************************************************

using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Tests.Assemblies;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.TestDomainArxNetFixtureCommands))]
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.TestDomainArxNetTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class TestDomainArxNetFixtureCommands
    {
        //public void AssemblyIsLoadedCorrectly()
        [CommandMethod("AssemblyIsLoadedCorrectly")]
        public void AssemblyIsLoadedCorrectly()
        {
            TestDomainArxNetFixture.MakeAppDomain();
            TestDomainArxNetFixture tests = new TestDomainArxNetFixture();
            tests.AssemblyIsLoadedCorrectly();
            TestDomainArxNetFixture.UnloadTestDomain();
        }

        //public void CanRunMockAssemblyTests()
        [CommandMethod("CanRunMockAssemblyTests")]
        public void CanRunMockAssemblyTests()
        {
            TestDomainArxNetFixture.MakeAppDomain();
            TestDomainArxNetFixture tests = new TestDomainArxNetFixture();
            tests.CanRunMockAssemblyTests();
            TestDomainArxNetFixture.UnloadTestDomain();
        }
    }

    public class TestDomainArxNetTestsCommands
    {
        //public void FileNotFound()
        [CommandMethod("TestDomainArxNetTestsCommands", "FileNotFound", CommandFlags.Modal)]
        public void FileNotFound()
        {
            TestDomainArxNetTests tests = new TestDomainArxNetTests();
            tests.SetUp();
            try
            {
                tests.FileNotFound();
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ed.WriteMessage("\n不是期望的FileNotFoundException异常");
            }
            catch (FileNotFoundException) { }
            tests.TearDown();
        }

        //public void InvalidTestFixture()
        [CommandMethod("InvalidTestFixture")]
        public void InvalidTestFixture()
        {
            TestDomainArxNetTests tests = new TestDomainArxNetTests();
            tests.SetUp();
            tests.InvalidTestFixture();
            tests.TearDown();
        }

        //public void FileFoundButNotValidAssembly()
        [CommandMethod("FileFoundButNotValidAssembly")]
        public void FileFoundButNotValidAssembly()
        {
            TestDomainArxNetTests tests = new TestDomainArxNetTests();
            tests.SetUp();
            try
            {
                tests.FileFoundButNotValidAssembly();
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ed.WriteMessage("\n不是期望的BadImageFormatException异常");
            }
            catch (BadImageFormatException) { }
            tests.TearDown();
        }

        //public void SpecificTestFixture()
        [CommandMethod("SpecificTestFixture")]
        public void SpecificTestFixture()
        {
            TestDomainArxNetTests tests = new TestDomainArxNetTests();
            tests.SetUp();
            tests.SpecificTestFixture();
            tests.TearDown();
        }

        //public void BasePathOverrideIsHonored()
        [CommandMethod("BasePathOverrideIsHonored")]
        public void BasePathOverrideIsHonored()
        {
            TestDomainArxNetTests tests = new TestDomainArxNetTests();
            tests.SetUp();
            tests.BasePathOverrideIsHonored();
            tests.TearDown();
        }
    }
}
