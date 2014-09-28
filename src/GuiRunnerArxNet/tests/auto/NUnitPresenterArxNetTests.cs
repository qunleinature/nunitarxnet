// Copyright 2014, Lei Qun
//  2014.9.28：
//      在NUnit2.6.3基础上调试
// ****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NUnit.Framework;
using NUnit.Util.ArxNet;
using NUnit.Gui;
using NUnit.Gui.ArxNet;
using NUnit.Core;
using NUnit.Util;
using NUnit.UiKit;

using Com.Utility.UnitTest;

namespace NUnit.Gui.ArxNet.Tests
{
    [TestFixture]
    public class NUnitPresenterArxNetTests
    {
        NUnitPresenterArxNet nUnitPresenterArxNet = null;

        private NUnitPresenterArxNet NewPresenter(bool form_loader_null)
        {
            NUnitPresenterArxNet presenter = null;

            if (form_loader_null)
            {
                presenter = new NUnitPresenterArxNet(null, null);
            }
            else
            {
                NUnitFormArxNet form = new NUnitFormArxNet(new GuiOptionsArxNet(new string[0]));
                TestLoaderArxNet loader = new TestLoaderArxNet();
                presenter = new NUnitPresenterArxNet(form, loader);
            }

            return presenter;
        }

        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void TearDown()
        {

        }

        //public NUnitFormArxNet Form        
        //Constructor
        //public NUnitPresenterArxNet(NUnitFormArxNet form, TestLoaderArxNet loader)
        [Test]
        public void ConstructorAndForm()
        {
            NUnitFormArxNet expectedForm = new NUnitFormArxNet(new GuiOptionsArxNet(new string[0]));
            TestLoaderArxNet expectedLoader = new TestLoaderArxNet();
            nUnitPresenterArxNet = new NUnitPresenterArxNet(expectedForm, expectedLoader);            
            Assert.That(nUnitPresenterArxNet.Form, Is.EqualTo(expectedForm));
            //private TestLoaderArxNet loader = null;
            TestLoaderArxNet actualLoader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            Assert.That(actualLoader, Is.EqualTo(expectedLoader));
        }

        //public void RemoveWatcher()
        //public void WatchProject(string projectPath)
        [Test]
        public void WatchRemoveWatcher()
        {
            FileWatcher projectWatcher = null;

            nUnitPresenterArxNet = NewPresenter(false);
            //private TestLoaderArxNet loader = null;
            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            loader.NewProject();

            nUnitPresenterArxNet.WatchProject(loader.TestProject.ProjectPath);
            //private FileWatcher projectWatcher = null;
            projectWatcher = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "projectWatcher") as FileWatcher;
            Assert.That(projectWatcher, Is.Not.Null);
            //private string filePath;
            string filePath = UnitTestHelper.GetNonPublicField(projectWatcher, "filePath") as string;
            Assert.That(filePath, Is.SamePath(loader.TestProject.ProjectPath));
            //private FileSystemWatcher watcher;
            FileSystemWatcher watcher = UnitTestHelper.GetNonPublicField(projectWatcher, "watcher") as FileSystemWatcher;
            Assert.That(watcher.EnableRaisingEvents, Is.True);

            nUnitPresenterArxNet.RemoveWatcher();
            //private FileWatcher projectWatcher = null;
            projectWatcher = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "projectWatcher") as FileWatcher;
            Assert.That(projectWatcher, Is.Null);
        }

    }
}
