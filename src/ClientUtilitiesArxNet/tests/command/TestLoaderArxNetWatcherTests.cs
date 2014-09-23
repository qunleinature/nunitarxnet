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
using NUnit.Framework;
using NUnit.Tests.Assemblies;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.TestLoaderArxNetWatcherTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class TestLoaderArxNetWatcherTestsCommands
    {
        //public void LoadShouldStartWatcher()
        [CommandMethod("LoadShouldStartWatcher")]
        public void LoadShouldStartWatcher()
        {
            TestLoaderArxNetWatcherTests tests = new TestLoaderArxNetWatcherTests();
            tests.PreprareTestLoader();
            tests.LoadShouldStartWatcher();
            tests.CleanUpSettings();
        }

        //public void ReloadShouldStartWatcher()
        [CommandMethod("ReloadShouldStartWatcher")]
        public void ReloadShouldStartWatcher()
        {
            TestLoaderArxNetWatcherTests tests = new TestLoaderArxNetWatcherTests();
            tests.PreprareTestLoader();
            tests.ReloadShouldStartWatcher();
            tests.CleanUpSettings();
        }

        //public void UnloadShouldStopWatcherAndFreeResources()
        [CommandMethod("UnloadShouldStopWatcherAndFreeResources")]
        public void UnloadShouldStopWatcherAndFreeResources()
        {
            TestLoaderArxNetWatcherTests tests = new TestLoaderArxNetWatcherTests();
            tests.PreprareTestLoader();
            tests.UnloadShouldStopWatcherAndFreeResources();
            tests.CleanUpSettings();
        }

        //public void LoadShouldStartWatcherDependingOnSettings()
        [CommandMethod("LoadShouldStartWatcherDependingOnSettings")]
        public void LoadShouldStartWatcherDependingOnSettings()
        {
            TestLoaderArxNetWatcherTests tests = new TestLoaderArxNetWatcherTests();
            tests.PreprareTestLoader();
            tests.LoadShouldStartWatcherDependingOnSettings();
            tests.CleanUpSettings();
        }

        //public void ReloadShouldStartWatcherDependingOnSettings()
        [CommandMethod("ReloadShouldStartWatcherDependingOnSettings")]
        public void ReloadShouldStartWatcherDependingOnSettings()
        {
            TestLoaderArxNetWatcherTests tests = new TestLoaderArxNetWatcherTests();
            tests.PreprareTestLoader();
            tests.ReloadShouldStartWatcherDependingOnSettings();
            tests.CleanUpSettings();
        }
    }
}
