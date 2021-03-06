﻿// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2012, Lei Qun
// 2012.12.19修改
// ****************************************************************

using System;
using System.IO;
using System.Threading;
using System.Reflection;

using NUnit.Core;
using NUnit.Core.Extensibility;
using NUnit.Framework;
using NUnit.Tests.Assemblies;
using NUnit.Util;
using NUnit.Util.ArxNet;
using NUnit.UiKit;

namespace NUnit.Util.ArxNet.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class TestLoaderArxNetAssemblyTests
    {
        private readonly string assembly = MockAssembly.AssemblyPath;
        private readonly string badFile = "/x.dll";

        private TestLoaderArxNet loader;
        private TestEventCatcher catcher;

        private void LoadTest(string assembly)
        {
            loader.LoadProject(assembly);
            if (loader.IsProjectLoaded && loader.TestProject.IsLoadable)
                loader.LoadTest();
        }

        [SetUp]
        public void SetUp()
        {
            loader = new TestLoaderArxNet();
            catcher = new TestEventCatcher(loader.Events);
        }

        [TearDown]
        public void TearDown()
        {
            if (loader.IsTestLoaded)
                loader.UnloadTest();

            if (loader.IsProjectLoaded)
                loader.UnloadProject();
        }

        [Test]
        public void LoadProject()
        {
            loader.LoadProject(assembly);
            Assert.IsTrue(loader.IsProjectLoaded, "Project not loaded");
            Assert.IsFalse(loader.IsTestLoaded, "Test should not be loaded");
            Assert.AreEqual(2, catcher.Events.Count);
            Assert.AreEqual(TestAction.ProjectLoading, ((TestEventArgs)catcher.Events[0]).Action);
            Assert.AreEqual(TestAction.ProjectLoaded, ((TestEventArgs)catcher.Events[1]).Action);
        }

        [Test]
        public void UnloadProject()
        {
            loader.LoadProject(assembly);
            loader.UnloadProject();
            Assert.IsFalse(loader.IsProjectLoaded, "Project not unloaded");
            Assert.IsFalse(loader.IsTestLoaded, "Test not unloaded");
            Assert.AreEqual(4, catcher.Events.Count);
            Assert.AreEqual(TestAction.ProjectUnloading, ((TestEventArgs)catcher.Events[2]).Action);
            Assert.AreEqual(TestAction.ProjectUnloaded, ((TestEventArgs)catcher.Events[3]).Action);
        }

        [Test]
        public void LoadTest()
        {
            LoadTest(assembly);

            Type type;
            TestRunner testRunner;
            FieldInfo field;
            object value;
            RemoteTestRunner remoteTestRunner;
            //在CAD环境下，测试须是单线程
            //TestLoaderArxNet:private TestRunner testRunner = null;
            type = loader.GetType();
            testRunner = null;
            field = type.GetField("testRunner", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
            value = null;
            if (field != null) value = field.GetValue(loader);
            if (value != null) testRunner = value as TestRunner;
            Assert.AreEqual(typeof(RemoteTestRunner), testRunner.GetType());
            //ProxyTestRunner:private TestRunner testRunner;
            remoteTestRunner = testRunner as RemoteTestRunner;
            type = testRunner.GetType().BaseType;
            testRunner = null;
            field = type.GetField("testRunner", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
            value = null;
            if (field != null) value = field.GetValue(remoteTestRunner);
            if (value != null) testRunner = value as TestRunner;
            Assert.AreEqual(typeof(SimpleTestRunner), testRunner.GetType());

            Assert.IsTrue(loader.IsProjectLoaded, "Project not loaded");
            Assert.IsTrue(loader.IsTestLoaded, "Test not loaded");
            Assert.AreEqual(4, catcher.Events.Count);
            Assert.AreEqual(TestAction.TestLoading, ((TestEventArgs)catcher.Events[2]).Action);
            Assert.AreEqual(TestAction.TestLoaded, ((TestEventArgs)catcher.Events[3]).Action);
            Assert.AreEqual(MockAssembly.Tests, ((TestEventArgs)catcher.Events[3]).TestCount);
        }

        [Test]
        public void UnloadTest()
        {
            LoadTest(assembly);
            loader.UnloadTest();
            Assert.AreEqual(6, catcher.Events.Count);
            Assert.AreEqual(TestAction.TestUnloading, ((TestEventArgs)catcher.Events[4]).Action);
            Assert.AreEqual(TestAction.TestUnloaded, ((TestEventArgs)catcher.Events[5]).Action);
        }

        //?还未了解ReloadTest功能，暂未实现测试
        //新的测试
        //public void ReloadTest(RuntimeFramework framework)、new public void ReloadTest()
        [Test]
        public void ReloadTest()
        {
            /*
            loader.LoadProject(assembly);
            if (loader.IsProjectLoaded && loader.TestProject.IsLoadable)
                loader.ReloadTest();
            
            Type type;
            TestRunner testRunner;
            FieldInfo field;
            object value;
            RemoteTestRunner remoteTestRunner;
            //在CAD环境下，测试须是单线程
            //TestLoader:private TestRunner testRunner = null;
            type = loader.GetType();
            testRunner = null;
            field = type.GetField("testRunner", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
            value = null;
            if (field != null) value = field.GetValue(loader);
            if (value != null) testRunner = value as TestRunner;
            Assert.AreEqual(typeof(RemoteTestRunner), testRunner.GetType());
            //ProxyTestRunner:private TestRunner testRunner;
            remoteTestRunner = testRunner as RemoteTestRunner;
            type = testRunner.GetType().BaseType;
            testRunner = null;
            field = type.GetField("testRunner", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
            value = null;
            if (field != null) value = field.GetValue(remoteTestRunner);
            if (value != null) testRunner = value as TestRunner;
            Assert.AreEqual(typeof(SimpleTestRunner), testRunner.GetType());

            loader.RunTests();

            Assert.AreEqual(TestAction.ProjectLoading, ((TestEventArgs)catcher.Events[0]).Action);
            Assert.AreEqual(TestAction.ProjectLoaded, ((TestEventArgs)catcher.Events[1]).Action);
            Assert.AreEqual(TestAction.TestLoading, ((TestEventArgs)catcher.Events[2]).Action);
            Assert.AreEqual(TestAction.TestLoaded, ((TestEventArgs)catcher.Events[3]).Action);
            Assert.AreEqual(TestAction.RunStarting, ((TestEventArgs)catcher.Events[4]).Action);

            int eventCount = 4 + 2 * (MockAssembly.Nodes - MockAssembly.Explicit);
            if (eventCount != catcher.Events.Count)
                foreach (TestEventArgs e in catcher.Events)
                    Console.WriteLine(e.Action);
            Assert.AreEqual(eventCount, catcher.Events.Count);

            Assert.AreEqual(TestAction.RunFinished, ((TestEventArgs)catcher.Events[eventCount - 1]).Action);

            int nTests = 0;
            int nRun = 0;
            foreach (object o in catcher.Events)
            {
                TestEventArgs e = o as TestEventArgs;

                if (e != null && e.Action == TestAction.TestFinished)
                {
                    ++nTests;
                    if (e.Result.Executed)
                        ++nRun;
                }
            }
            Assert.AreEqual(MockAssembly.ResultCount, nTests);
            Assert.AreEqual(MockAssembly.TestsRun, nRun);
            */
        }

        [Test]
        public void FileNotFound()
        {
            LoadTest("xxxxx");
            Assert.IsFalse(loader.IsProjectLoaded, "Project should not load");
            Assert.IsFalse(loader.IsTestLoaded, "Test should not load");
            Assert.AreEqual(2, catcher.Events.Count);
            Assert.AreEqual(TestAction.ProjectLoadFailed, ((TestEventArgs)catcher.Events[1]).Action);
            Assert.AreEqual(typeof(FileNotFoundException), ((TestEventArgs)catcher.Events[1]).Exception.GetType());
        }

        // Doesn't work under .NET 2.0 Beta 2
        //[Test]
        public void InvalidAssembly()
        {
            FileInfo file = new FileInfo(badFile);
            try
            {
                StreamWriter sw = file.AppendText();
                sw.WriteLine("This is a new entry to add to the file");
                sw.WriteLine("This is yet another line to add...");
                sw.Flush();
                sw.Close();

                LoadTest(badFile);
                Assert.IsTrue(loader.IsProjectLoaded, "Project not loaded");
                Assert.IsFalse(loader.IsTestLoaded, "Test should not be loaded");
                Assert.AreEqual(4, catcher.Events.Count);
                Assert.AreEqual(TestAction.TestLoadFailed, ((TestEventArgs)catcher.Events[3]).Action);
                Assert.AreEqual(typeof(BadImageFormatException), ((TestEventArgs)catcher.Events[3]).Exception.GetType());
            }
            finally
            {
                if (file.Exists)
                    file.Delete();
            }
        }

        [Test]
        public void AssemblyWithNoTests()
        {
            LoadTest("nunit.framework.dll");
            Assert.IsTrue(loader.IsProjectLoaded, "Project not loaded");
            Assert.IsTrue(loader.IsTestLoaded, "Test not loaded");
            Assert.AreEqual(4, catcher.Events.Count);
            Assert.AreEqual(TestAction.TestLoaded, ((TestEventArgs)catcher.Events[3]).Action);
        }

        // TODO: Should wrapper project be unloaded on failure?

        [Test]
        public void RunTest()
        {                        
            //loader.ReloadOnRun = false;

            LoadTest(assembly);

            Type type;
            TestRunner testRunner;
            FieldInfo field;
            object value;
            RemoteTestRunner remoteTestRunner;
            //在CAD环境下，测试须是单线程
            //TestLoaderArxNet:private TestRunner testRunner = null;
            type = loader.GetType();
            testRunner = null;
            field = type.GetField("testRunner", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
            value = null;
            if (field != null) value = field.GetValue(loader);
            if (value != null) testRunner = value as TestRunner;
            Assert.AreEqual(typeof(RemoteTestRunner), testRunner.GetType());
            //ProxyTestRunner:private TestRunner testRunner;
            remoteTestRunner = testRunner as RemoteTestRunner;
            type = testRunner.GetType().BaseType;
            testRunner = null;
            field = type.GetField("testRunner", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
            value = null;
            if (field != null) value = field.GetValue(remoteTestRunner);
            if (value != null) testRunner = value as TestRunner;
            Assert.AreEqual(typeof(SimpleTestRunner), testRunner.GetType());

            loader.RunTests();
            /*do
            {
                // TODO: Find a more robust way of handling this
                Thread.Sleep(500);
            }
            while (!catcher.GotRunFinished);*/

            Assert.AreEqual(TestAction.ProjectLoading, ((TestEventArgs)catcher.Events[0]).Action);
            Assert.AreEqual(TestAction.ProjectLoaded, ((TestEventArgs)catcher.Events[1]).Action);
            Assert.AreEqual(TestAction.TestLoading, ((TestEventArgs)catcher.Events[2]).Action);
            Assert.AreEqual(TestAction.TestLoaded, ((TestEventArgs)catcher.Events[3]).Action);
            Assert.AreEqual(TestAction.RunStarting, ((TestEventArgs)catcher.Events[4]).Action);

            int eventCount = 4 /* for loading */+ 2 * (MockAssembly.Nodes - MockAssembly.Explicit);
            if (eventCount != catcher.Events.Count)
                foreach (TestEventArgs e in catcher.Events)
                    Console.WriteLine(e.Action);
            Assert.AreEqual(eventCount, catcher.Events.Count);

            Assert.AreEqual(TestAction.RunFinished, ((TestEventArgs)catcher.Events[eventCount - 1]).Action);

            int nTests = 0;
            int nRun = 0;
            foreach (object o in catcher.Events)
            {
                TestEventArgs e = o as TestEventArgs;

                if (e != null && e.Action == TestAction.TestFinished)
                {
                    ++nTests;
                    if (e.Result.Executed)
                        ++nRun;
                }
            }
            Assert.AreEqual(MockAssembly.ResultCount, nTests);
            Assert.AreEqual(MockAssembly.TestsRun, nRun);
        }

        #region temp

        //[Test]
        public void RunTestTry()
        {
            ServiceManager.Services.AddService(new TestLoaderArxNet(new GuiTestEventDispatcher()));
            ITestLoader loader1 = ServicesArxNet.TestLoader;             
            loader1.LoadProject(assembly);
            if (loader1.IsProjectLoaded && loader1.TestProject.IsLoadable)
                loader1.LoadTest();
            
            loader1.RunTests();
        }

        #endregion
    }
}

