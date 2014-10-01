// Copyright 2014, Lei Qun
//  2014.9.28：
//      在NUnit2.6.3基础上调试
// ****************************************************************

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

        //public DialogResult CloseProject()
        [Test]
        [Category("CloseProject")]
        public void CloseProject()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.AddAssembly();
            DialogResult result = (DialogResult)nUnitPresenterArxNet.CloseProject();
            CADApplication.ShowAlertDialog("DialogResult:" + result);

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.10.1lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
            NUnitProject testProject = loader.TestProject;
            if (testProject == null)//2014.10.1lq加
            {
                CADApplication.ShowAlertDialog("TestProject:null");
                return;
            }
            CADApplication.ShowAlertDialog("CloseProject:" + testProject.ProjectPath);
        }

        [Test]
        [Category("CloseProject")]
        public void CloseProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            DialogResult result = (DialogResult)nUnitPresenterArxNet.CloseProject();
            CADApplication.ShowAlertDialog("DialogResult:" + result);

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.10.1lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
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
            if (loader == null)//2014.10.1lq改
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
            NUnitProject testProject = loader.TestProject;
            if (testProject == null)//2014.10.1lq改
            {
                CADApplication.ShowAlertDialog("TestProject:null");
                return;
            }
            ProjectConfig config = testProject.ActiveConfig;
            if (config.Assemblies != null && config.Assemblies.Count > 0)
            {
                string assembly = config.Assemblies[config.Assemblies.Count - 1];
                CADApplication.ShowAlertDialog("AddToProject:" + assembly);
            }
            else
                CADApplication.ShowAlertDialog("AddToProject:null");
            CADApplication.ShowAlertDialog("ConfigurationFile:" + config.ConfigurationFilePath);
        }

        [Test]
        [Category("AddToProject")]
        public void AddToProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.AddToProject();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.10.1lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
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
            if (loader == null)//2014.10.1lq改
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
            NUnitProject testProject = loader.TestProject;
            if (testProject == null)//2014.10.1lq改
            {
                CADApplication.ShowAlertDialog("TestProject:null");
                return;
            }
            ProjectConfig config = testProject.ActiveConfig;
            if (config.Assemblies != null && config.Assemblies.Count > 0)
            {
                string assembly = config.Assemblies[config.Assemblies.Count - 1];
                CADApplication.ShowAlertDialog("AddAssembly:" + assembly);
            }
            else
                CADApplication.ShowAlertDialog("AddAssembly:null");
        }

        [Test]
        [Category("AddAssembly")]
        public void AddAssembly_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.AddAssembly();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.10.1lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
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
            if (loader == null)//2014.10.1lq改
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
            NUnitProject testProject = loader.TestProject;
            if (testProject == null)//2014.10.1lq改
            {
                CADApplication.ShowAlertDialog("TestProject:null");
                return;
            }
            ProjectConfig config = testProject.ActiveConfig;
            if (config.Assemblies != null && config.Assemblies.Count > 0)
            {
                string str = "AddVSProject:";
                //string assembly = config.Assemblies[config.Assemblies.Count - 1];
                foreach (string assembly in config.Assemblies)
                {
                    str = str + "\n" + assembly;
                }
                CADApplication.ShowAlertDialog(str);
            }
            else
                CADApplication.ShowAlertDialog("AddVSProject:null");
            CADApplication.ShowAlertDialog("ConfigurationFile:" + config.ConfigurationFilePath);
        }

        [Test]
        [Category("AddVSProject")]
        public void AddVSProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.AddVSProject();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.10.1lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
        }

        //public void SaveProject()
        [Test]
        [Category("SaveProject")]
        public void SaveProject()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.SaveProject();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.9.29lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
            NUnitProject testProject = loader.TestProject;
            if (testProject == null)//2014.9.29lq加
            {
                CADApplication.ShowAlertDialog("TestProject:null");
                return;
            }
            CADApplication.ShowAlertDialog("SaveProject:" + testProject.ProjectPath);//2014.9.29lq加
        }

        [Test]
        [Category("SaveProject")]
        public void SaveProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.SaveProject();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.9.29lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
        }

        //public void SaveProjectAs()
        [Test]
        [Category("SaveProjectAs")]
        public void SaveProjectAs()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.SaveProjectAs();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.9.29lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
            NUnitProject testProject = loader.TestProject;
            if (testProject == null)//2014.9.29lq加
            {
                CADApplication.ShowAlertDialog("TestProject:null");
                return;
            }
            CADApplication.ShowAlertDialog("SaveProjectAs:" + testProject.ProjectPath);//2014.9.29lq加
        }

        [Test]
        [Category("SaveProjectAs")]
        public void SaveProjectAs_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.SaveProjectAs();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.9.29lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
        }

        //private DialogResult SaveProjectIfDirty()
        [Test]
        [Category("SaveProjectIfDirty")]
        public void SaveProjectIfDirty()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.AddAssembly();
            object[] args = null;//2014.9.29lq加
            DialogResult result = (DialogResult)UnitTestHelper.CallNonPublicMethod(nUnitPresenterArxNet, "SaveProjectIfDirty", args);//2014.9.29lq改
            
            CADApplication.ShowAlertDialog("DialogResult：" + result);
            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.9.30lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
            NUnitProject testProject = loader.TestProject;
            if (testProject == null)//2014.9.30lq加
            {
                CADApplication.ShowAlertDialog("TestProject:null");
                return;
            }
            CADApplication.ShowAlertDialog("SaveProjectIfDirty:" + testProject.ProjectPath);//2014.9.30lq加
        }

        [Test]
        [Category("SaveProjectIfDirty")]
        public void SaveProjectIfDirty_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            object[] args = null;//2014.9.29lq加
            DialogResult result = (DialogResult)UnitTestHelper.CallNonPublicMethod(nUnitPresenterArxNet, "SaveProjectIfDirty", args);//2014.9.29lq改
            
            CADApplication.ShowAlertDialog("DialogResult：" + result);
            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.9.30lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
        }

        //public void SaveLastResult()
        [Test]
        [Category("SaveLastResult")]
        public void SaveLastResult()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.AddAssembly();
            //loader.LoadTest();
            //loader.RunTests(null);
            nUnitPresenterArxNet.SaveLastResult();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.9.30lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
            NUnitProject testProject = loader.TestProject;
            if (testProject == null)//2014.9.30lq加
            {
                CADApplication.ShowAlertDialog("TestProject:null");
                return;
            }
            CADApplication.ShowAlertDialog("SaveLastResult:" + testProject.ProjectPath);//2014.9.30lq加

        }
        [Test]
        [Category("SaveLastResult")]
        public void SaveLastResult_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.SaveLastResult();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.9.29lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
        }

        //public void ReloadProject()
        [Test]
        [Category("ReloadProject")]
        public void ReloadProject()
        {
            nUnitPresenterArxNet = NewPresenter(false);
            nUnitPresenterArxNet.NewProject();
            nUnitPresenterArxNet.SaveProject();
            nUnitPresenterArxNet.ReloadProject();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.9.30lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
            NUnitProject testProject = loader.TestProject;
            if (testProject == null)//2014.9.30lq加
            {
                CADApplication.ShowAlertDialog("TestProject:null");
                return;
            }
            CADApplication.ShowAlertDialog("ReloadProject:" + testProject.ProjectPath);//2014.9.30lq加
        }

        [Test]
        [Category("ReloadProject")]
        public void ReloadProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.ReloadProject();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.9.30lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
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

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.10.1lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
            NUnitProject testProject = loader.TestProject;
            if (testProject == null)//2014.10.1lq加
            {
                CADApplication.ShowAlertDialog("TestProject:null");
                return;
            }
            CADApplication.ShowAlertDialog("EditProject:" + testProject.ProjectPath);//2014.10.1lq加
        }

        [Test]
        [Category("EditProject")]
        public void EditProject_form_loader_null()
        {
            nUnitPresenterArxNet = NewPresenter(true);
            nUnitPresenterArxNet.EditProject();

            TestLoaderArxNet loader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            if (loader == null)//2014.10.1lq加
            {
                CADApplication.ShowAlertDialog("loader:null");
                return;
            }
        }
    }
}
