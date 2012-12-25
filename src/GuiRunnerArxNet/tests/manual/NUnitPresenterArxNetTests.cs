using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using NUnit.Framework;
using NUnit.Util.ArxNet;
using NUnit.Gui;
using NUnit.Gui.ArxNet;
using NUnit.Core;
using NUnit.Util;
using NUnit.UiKit;
using NUnit.Util.ArxNet;

using Com.Utility.UnitTest;

namespace NUnit.Gui.ArxNet.Tests
{
    [TestFixture]
    public class NUnitPresenterArxNetTests
    {
        //public void AddAssembly(string configName)
        [Test]
        [Category("AddAssembly")]
        public void AddAssembly()
        {
            NUnitFormArxNet form = new NUnitFormArxNet(new GuiOptions(new string[0]));
            TestLoaderArxNet loader = new TestLoaderArxNet();
            NUnitPresenterArxNet nUnitPresenterArxNet = new NUnitPresenterArxNet(form, loader);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.AddAssembly();
            loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            ProjectConfig config = loader.TestProject.ActiveConfig;
            if (config.Assemblies !=null && config.Assemblies.Count > 0)
            {
                string assembly = config.Assemblies[config.Assemblies.Count - 1];
                Application.ShowAlertDialog("添加的程序集为" + assembly);
            }
            else
                Application.ShowAlertDialog("没添加程序集");
        }

        [Test]
        [Category("AddAssembly")]
        public void AddAssembly_form_loader_null()
        {
            NUnitPresenterArxNet nUnitPresenterArxNet = new NUnitPresenterArxNet(null, null);
            nUnitPresenterArxNet.AddAssembly();
        }

        //public void AddToProject(string configName)
        [Test]
        public void AddToProject()
        {
            NUnitFormArxNet form = new NUnitFormArxNet(new GuiOptions(new string[0]));
            TestLoaderArxNet loader = new TestLoaderArxNet();
            NUnitPresenterArxNet nUnitPresenterArxNet = new NUnitPresenterArxNet(form, loader);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.AddToProject();

        }
    }
}
