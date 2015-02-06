// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2014.9.21：
//  1.利用cad命令直接测试
//  2015.2.6：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.RecentFilesArxNetTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    using System;
    using System.Collections;
    using Microsoft.Win32;

    using NUnit.Framework;

    public class RecentFilesArxNetTestsCommands
    {
        //public void CountDefault()
        [CommandMethod("CountDefault")]
        public void CountDefault()
        {
            RecentFilesArxNetTests tests = new RecentFilesArxNetTests();
            tests.SetUp();
            tests.CountDefault();
        }

        //public void CountOverMax()
        [CommandMethod("CountOverMax")]
        public void CountOverMax()
        {
            RecentFilesArxNetTests tests = new RecentFilesArxNetTests();
            tests.SetUp();
            tests.CountOverMax();
        }

        //public void CountUnderMin()
        [CommandMethod("CountUnderMin")]
        public void CountUnderMin()
        {
            RecentFilesArxNetTests tests = new RecentFilesArxNetTests();
            tests.SetUp();
            tests.CountUnderMin();
        }

        //public void CountAtMax()
        [CommandMethod("CountAtMax")]
        public void CountAtMax()
        {
            RecentFilesArxNetTests tests = new RecentFilesArxNetTests();
            tests.SetUp();
            tests.CountAtMax();
        }

        //public void CountAtMin()
        [CommandMethod("CountAtMin")]
        public void CountAtMin()
        {
            RecentFilesArxNetTests tests = new RecentFilesArxNetTests();
            tests.SetUp();
            tests.CountAtMin();
        }

        //public void EmptyList()
        [CommandMethod("EmptyList")]
        public void EmptyList()
        {
            RecentFilesArxNetTests tests = new RecentFilesArxNetTests();
            tests.SetUp();
            tests.EmptyList();
        }
    }
}
