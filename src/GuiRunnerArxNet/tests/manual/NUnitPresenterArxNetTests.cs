using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using FormsApplication = System.Windows.Forms.Application;
using CADApplication = Autodesk.AutoCAD.ApplicationServices.Application;

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
                NUnitFormArxNet form = new NUnitFormArxNet(new GuiOptions(new string[0]));
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

        //public void AddAssembly(string configName)
        [Test]
        [Category("AddAssembly")]
        public void AddAssembly()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.AddAssembly();
            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            ProjectConfig config = loader.TestProject.ActiveConfig;
            if (config.Assemblies !=null && config.Assemblies.Count > 0)
            {
                string assembly = config.Assemblies[config.Assemblies.Count - 1];
                CADApplication.ShowAlertDialog("添加的程序集为：" + assembly);
            }
            else
                CADApplication.ShowAlertDialog("没添加程序集!");
        }

        [Test]
        [Category("AddAssembly")]
        public void AddAssembly_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.AddAssembly();
        }

        //public void AddToProject(string configName)
        [Test]
        [Category("AddToProject")]
        public void AddToProject()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            ServicesArxNet.UserSettings.SaveSetting("Options.TestLoader.VisualStudioSupport", true);
            nUnitPresenterArxNet.AddToProject("Debug");

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            NUnitProject project = loader.TestProject;
            ProjectConfig config = project.Configs[project.Configs.Count - 1];
            if (config.Assemblies !=null && config.Assemblies.Count > 0)
            {
                string assembly = config.Assemblies[config.Assemblies.Count - 1];
                CADApplication.ShowAlertDialog("最后一个程序集为：" + assembly);
            }
            else
                CADApplication.ShowAlertDialog("没添加程序集!");

            CADApplication.ShowAlertDialog("最后一个项目文件：" + config.ConfigurationFilePath);
        }

        [Test]
        [Category("AddToProject")]
        public void AddToProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.AddToProject();
        }

        //public void AddVSProject()
        [Test]
        [Category("AddVSProject")]
        public void AddVSProject()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();            
            nUnitPresenterArxNet.AddVSProject();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            NUnitProject project = loader.TestProject;
            ProjectConfig config = project.Configs[project.Configs.Count - 1];
            CADApplication.ShowAlertDialog("添加的VS项目文件：" + config.ConfigurationFilePath);

        }

        [Test]
        [Category("AddVSProject")]
        public void AddVSProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.AddVSProject();
        }

        //private DialogResult SaveProjectIfDirty()
        [Test]
        [Category("SaveProjectIfDirty")]
        public void SaveProjectIfDirty()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.AddAssembly();
            DialogResult result = (DialogResult)UnitTestHelper.CallNonPublicMethod(nUnitPresenterArxNet, "SaveProjectIfDirty", null);
            CADApplication.ShowAlertDialog("DialogResult：" + result);
        }

        [Test]
        [Category("SaveProjectIfDirty")]
        public void SaveProjectIfDirty_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            DialogResult result = (DialogResult)UnitTestHelper.CallNonPublicMethod(nUnitPresenterArxNet, "SaveProjectIfDirty", null);
            CADApplication.ShowAlertDialog("DialogResult：" + result);
        }

        //public DialogResult CloseProject()
        [Test]
        [Category("CloseProject")]
        public void CloseProject()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.AddAssembly();
            DialogResult result = (DialogResult)nUnitPresenterArxNet.CloseProject();
            CADApplication.ShowAlertDialog("DialogResult：" + result);
        }

        [Test]
        [Category("CloseProject")]
        public void CloseProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            DialogResult result = (DialogResult)nUnitPresenterArxNet.CloseProject();
            CADApplication.ShowAlertDialog("DialogResult：" + result);
        }

        //public void EditProject()
        [Test]
        [Category("EditProject")]
        public void EditProject()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.AddAssembly();
            nUnitPresenterArxNet.EditProject();
        }

        [Test]
        [Category("EditProject")]
        public void EditProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.EditProject();
        }

        //public void NewProject()
        [Test]
        [Category("NewProject")]
        public void NewProject()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
        }

        [Test]
        [Category("NewProject")]
        public void NewProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.NewProject();
        }

        // public void OpenProject()
        [Test]
        [Category("OpenProject")]
        public void OpenProject()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.OpenProject();
        }

        [Test]
        [Category("OpenProject")]
        public void OpenProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.OpenProject();
        }

        //public void ReloadProject()
        [Test]
        [Category("ReloadProject")]
        public void ReloadProject()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.ReloadProject();
        }

        [Test]
        [Category("ReloadProject")]
        public void ReloadProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.ReloadProject();
        }

        //public void SaveLastResult()
        [Test]
        [Category("SaveLastResult")]
        public void SaveLastResult()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.AddAssembly();            
            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            loader.LoadTest();
            loader.RunTests();

            nUnitPresenterArxNet.SaveLastResult();
        }

        [Test]
        [Category("SaveLastResult")]
        public void SaveLastResult_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.SaveLastResult();
        }

        //public void SaveProject()
        [Test]
        [Category("SaveProject")]
        public void SaveProject()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.SaveProject();
        }

        [Test]
        [Category("SaveProject")]
        public void SaveProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.SaveProject();
        }
    }
}
