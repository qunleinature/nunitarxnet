// ****************************************************************
// Copyright 2011, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2012.8.24修改
// 2012.12.20修改
// 2012.12.25修改: 
// 1.对build29938除bug
// 2.单元测试改
// 2012.12.27修改
// 1.单元测试改
// 2012.12.29修改
// 1.单元测试改
// 2012.12.30修改
// 1.单元测试改
// 2012.12.31修改
// 1.单元测试改
// 2013.1.1修改
// 1.单元测试NUnit.Gui.ArxNet.Tests.NUnitPresenterArxNetTests.SaveProject改
// 2.单元测试NUnit.Gui.ArxNet.Tests.NUnitPresenterArxNetTests.SaveProjectAs改
// 2013.6.1修改：
// 1.Services已经改ServicesArxNet
// 2013.6.6修改：
// 1.已改TestLoader为TestLoaderArxNet
// 2.改成在NUnit2.6.2基础
// 3.NUnitForm已改成NUnitFormArxNet
// ****************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Util;
using NUnit.UiKit;
using NUnit.Gui;
using NUnit.Util.ArxNet;

using Autodesk.AutoCAD.Runtime;
//using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using FormsApplication = System.Windows.Forms.Application;
using CADApplication = Autodesk.AutoCAD.ApplicationServices.Application;
using SystemException = System.Exception;
using CADException = Autodesk.AutoCAD.Runtime.Exception;

namespace NUnit.Gui.ArxNet
{
	/// <summary>
	/// NUnitPresenter does all file opening and closing that
	/// involves interacting with the user.
    /// 
    /// NOTE: This class originated as the static class
    /// TestLoaderUI and is slowly being converted to a
    /// true presenter. Current limitations include:
    /// 
    /// 1. At this time, the presenter is created by
    /// the form and interacts with it directly, rather
    /// than through an interface. 
    /// 
    /// 2. Many functions, which should properly be in
    /// the presenter, remain in the form.
    /// 
    /// 3. The presenter creates dialogs itself, which
    /// limits testability.
	/// </summary>
    public class NUnitPresenterArxNet
    {
        #region Instance Variables

        private NUnitFormArxNet form = null;
        private TestLoaderArxNet loader = null;

        // Our nunit project watcher
        private FileWatcher projectWatcher = null;

        #endregion

        #region Constructor

        // TODO: Use an interface for view and model
        public NUnitPresenterArxNet(NUnitFormArxNet form, TestLoaderArxNet loader)
        {
            this.form = form;
            this.loader = loader;
        }

        #endregion

        #region Public Properties

        public NUnitFormArxNet Form
        {
            get { return form; }
        }

        #endregion

        #region Public Methods

        #region New Project Methods

        public void NewProject()
        {
            try//2012-12-30单元测试加
            {
                if (loader == null) return;

                if (loader.IsProjectLoaded)
                    CloseProject();

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "New Test Project";
            dlg.Filter = "NUnit Test Project (*.nunit)|*.nunit|All Files (*.*)|*.*";
                dlg.FileName = ServicesArxNet.ProjectService.GenerateProjectName();
            dlg.DefaultExt = "nunit";
            dlg.ValidateNames = true;
            dlg.OverwritePrompt = true;

            if (dlg.ShowDialog(Form) == DialogResult.OK)
                loader.NewProject(dlg.FileName);
        }
            /*2012-12-30单元测试加*/
            catch (CADException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Get the New Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Get the New Project\n" + exception.Message);
            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Get the New Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Get the New Project\n" + exception.Message);
            }
            /*2012-12-30单元测试加*/

        }

        #endregion

        #region Open Methods

        public void OpenProject()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			System.ComponentModel.ISite site = Form == null ? null : Form.Site;
			if ( site != null ) dlg.Site = site;
			dlg.Title = "Open Project";
			
			if ( VisualStudioSupport )
			{
                dlg.Filter =
					"Projects & Assemblies(*.nunit,*.csproj,*.vbproj,*.vjsproj, *.vcproj,*.sln,*.dll,*.exe )|*.nunit;*.csproj;*.vjsproj;*.vbproj;*.vcproj;*.sln;*.dll;*.exe|" +
					"All Project Types (*.nunit,*.csproj,*.vbproj,*.vjsproj,*.vcproj,*.sln)|*.nunit;*.csproj;*.vjsproj;*.vbproj;*.vcproj;*.sln|" +
                    "Test Projects (*.nunit)|*.nunit|" +
                    "Solutions (*.sln)|*.sln|" +
                    "C# Projects (*.csproj)|*.csproj|" +
                    "J# Projects (*.vjsproj)|*.vjsproj|" +
                    "VB Projects (*.vbproj)|*.vbproj|" +
                    "C++ Projects (*.vcproj)|*.vcproj|" +
                    "Assemblies (*.dll,*.exe)|*.dll;*.exe";
			}
			else
			{
                dlg.Filter =
                    "Projects & Assemblies(*.nunit,*.dll,*.exe)|*.nunit;*.dll;*.exe|" +
                    "Test Projects (*.nunit)|*.nunit|" +
                    "Assemblies (*.dll,*.exe)|*.dll;*.exe";
			}

			dlg.FilterIndex = 1;
			dlg.FileName = "";

			if ( dlg.ShowDialog( Form ) == DialogResult.OK ) 
				OpenProject( dlg.FileName );
		}

        public void WatchProject(string projectPath)
        {
            this.projectWatcher = new FileWatcher(projectPath, 100);

            this.projectWatcher.Changed += new FileChangedHandler(OnTestProjectChanged);
            this.projectWatcher.Start();
        }

        public void RemoveWatcher()
        {
            if (projectWatcher != null)
            {
                projectWatcher.Stop();
                projectWatcher.Dispose();
                projectWatcher = null;
            }
        }

        private void OnTestProjectChanged(string filePath)
        {
            string message = filePath + Environment.NewLine + Environment.NewLine +
                "This file has been modified outside of NUnit." + Environment.NewLine +
                "Do you want to reload it?";

            if (Form.MessageDisplay.Ask(message) == DialogResult.Yes)
                ReloadProject();
        }

        public void OpenProject(string testFileName, string configName, string testName)
		{
            try//2012-12-31单元测试加
            {
                if (loader == null) return;//2012-12-31单元测试加

			if ( loader.IsProjectLoaded && SaveProjectIfDirty() == DialogResult.Cancel )
				return;

			loader.LoadProject( testFileName, configName );
			if ( loader.IsProjectLoaded )
			{	
				NUnitProject testProject = loader.TestProject;

                    if (testProject == null) return;//2012-12-31单元测试加

				if ( testProject.Configs.Count == 0 )
                    Form.MessageDisplay.Info("Loaded project contains no configuration data");
				else if ( testProject.ActiveConfig == null )
                    Form.MessageDisplay.Info("Loaded project has no active configuration");
				else if ( testProject.ActiveConfig.Assemblies.Count == 0 )
                    Form.MessageDisplay.Info("Active configuration contains no assemblies");
				else
					loader.LoadTest( testName );
			}
		}
            /*2012-12-31单元测试加*/
            catch (CADException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Open the Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Open the Project\n" + exception.Message);
            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Open the Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Open the Project\n" + exception.Message);
            }
            /*2012-12-31单元测试加*/
        }

		public void OpenProject( string testFileName )
		{
			OpenProject( testFileName, null, null );
		}

//		public static void OpenResults( Form owner )
//		{
//			OpenFileDialog dlg = new OpenFileDialog();
//			System.ComponentModel.ISite site = owner == null ? null : owner.Site;
//			if ( site != null ) dlg.Site = site;
//			dlg.Title = "Open Test Results";
//
//			dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
//			dlg.FilterIndex = 1;
//			dlg.FileName = "";
//
//			if ( dlg.ShowDialog( owner ) == DialogResult.OK ) 
//				OpenProject( owner, dlg.FileName );
        //		}

        #endregion

        #region Close Methods

        public DialogResult CloseProject()
        {
            try
            {
            DialogResult result = SaveProjectIfDirty();

                //2012-12-29单元测试加
                if (loader == null) return DialogResult.No;

            if (result != DialogResult.Cancel)
                loader.UnloadProject();

            return result;
        }
            /*2012-12-29单元测试加*/
            catch (CADException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Close the Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Close the Project\n" + exception.Message);
                return DialogResult.No;
            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Close the Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Close the Project\n" + exception.Message);
                return DialogResult.No;
            }
            /*2012-12-29单元测试加*/
        }

        #endregion

        #region Add Methods

        public void AddToProject()
		{
			AddToProject( null );
		}
        // TODO: Not used?
		public void AddToProject( string configName )
		{
            try
            {
                /*2012-12-25单元测试改*/
                if (loader == null) return;
                if (loader.TestProject == null) return;
                if (loader.TestProject.Configs == null) return;
                /*2012-12-25单元测试改*/

			    ProjectConfig config = configName == null
				    ? loader.TestProject.ActiveConfig
				    : loader.TestProject.Configs[configName];

                    /*2012-12-25单元测试改*/
                    if (config == null) return;
                    if (config.Assemblies == null) return;
                    /*2012-12-25单元测试改*/

			    OpenFileDialog dlg = new OpenFileDialog();
			    dlg.Title = "Add Assemblies To Project";
			    dlg.InitialDirectory = config.BasePath;

			    if ( VisualStudioSupport )
				    dlg.Filter =
					    "Projects & Assemblies(*.csproj,*.vbproj,*.vjsproj, *.vcproj,*.dll,*.exe )|*.csproj;*.vjsproj;*.vbproj;*.vcproj;*.dll;*.exe|" +
					    "Visual Studio Projects (*.csproj,*.vjsproj,*.vbproj,*.vcproj)|*.csproj;*.vjsproj;*.vbproj;*.vcproj|" +
					    "C# Projects (*.csproj)|*.csproj|" +
					    "J# Projects (*.vjsproj)|*.vjsproj|" +
					    "VB Projects (*.vbproj)|*.vbproj|" +
					    "C++ Projects (*.vcproj)|*.vcproj|" +
					    "Assemblies (*.dll,*.exe)|*.dll;*.exe";
			    else
				    dlg.Filter = "Assemblies (*.dll,*.exe)|*.dll;*.exe";

			    dlg.FilterIndex = 1;
			    dlg.FileName = "";

			    if ( dlg.ShowDialog( Form ) != DialogResult.OK )
				    return;

                if (PathUtils.IsAssemblyFileType(dlg.FileName))
                {
                    config.Assemblies.Add(dlg.FileName);
                    return;
                }
                else if (VSProject.IsProjectFile(dlg.FileName))
                {

                    VSProject vsProject = new VSProject(dlg.FileName);
                    MessageBoxButtons buttons;
                    string msg;

                    if (configName != null && vsProject.Configs.Contains(configName))
                    {
                        msg = "The project being added may contain multiple configurations;\r\r" +
                            "Select\tYes to add all configurations found.\r" +
                            "\tNo to add only the " + configName + " configuration.\r" +
                            "\tCancel to exit without modifying the project.";
                        buttons = MessageBoxButtons.YesNoCancel;
                    }
                    else
                    {
                        msg = "The project being added may contain multiple configurations;\r\r" +
                            "Select\tOK to add all configurations found.\r" +
                            "\tCancel to exit without modifying the project.";
                        buttons = MessageBoxButtons.OKCancel;
                    }

                    DialogResult result = Form.MessageDisplay.Ask(msg, buttons);
                    if (result == DialogResult.Yes || result == DialogResult.OK)
                    {
                        loader.TestProject.Add(vsProject);
                        return;
                    }
                    else if (result == DialogResult.No)
                    {
                        /*2012-12-26单元测试加*/
                        if (vsProject == null) return;
                        if (vsProject.Configs == null) return;

                        VSProjectConfig vsConfig = vsProject.Configs[configName];

                        if (vsConfig == null) return;
                        if (vsConfig.Assemblies == null) return;
                        /*2012-12-26单元测试加*/

                        foreach (string assembly in /*vsProject.Configs[configName].Assemblies*/vsConfig.Assemblies)//2012-12-26单元测试改
                            config.Assemblies.Add(assembly);
                        return;
                    }
                }
                
            }
            /*2012-12-29单元测试加*/
            catch (CADException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Invalid VS Project", exception);
                else
                    CADApplication.ShowAlertDialog("Invalid VS Project\n" + exception.Message);
            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Invalid VS Project", exception);
                else
                    CADApplication.ShowAlertDialog("Invalid VS Project\n" + exception.Message);
            }
            /*2012-12-29单元测试加*/
        }

		public void AddAssembly()
		{
			AddAssembly( null );
		}

		public void AddAssembly( string configName )
		{
            try//build29938fix002
            {
                /*build29938fix002*/
                if (loader == null) return;
                if (loader.TestProject == null) return;
                if (loader.TestProject.Configs == null) return;
                /*build29938fix002*/

			ProjectConfig config = configName == null
				? loader.TestProject.ActiveConfig
				: loader.TestProject.Configs[configName];

                /*build29938fix002*/
                if (config == null) return;
                if (config.Assemblies == null) return;
                /*build29938fix002*/

			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "Add Assembly";
			dlg.InitialDirectory = config.BasePath;
            dlg.Filter = "Assemblies (*.dll,*.exe)|*.dll;*.exe";
			dlg.FilterIndex = 1;
			dlg.FileName = "";

            if (dlg.ShowDialog(Form) == DialogResult.OK)
                config.Assemblies.Add(dlg.FileName);
		}
            /*build29938fix002*/
            /*2012-12-29单元测试加*/
            catch (CADException exception)
            {
               if (Form != null)
                    Form.MessageDisplay.Error("Unable to Add the Assembly", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Add the Assembly\n" + exception.Message);
            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Add the Assembly", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Add the Assembly\n" + exception.Message);
            }
            /*2012-12-29单元测试加*/            
            /*build29938fix002*/
        }

		public void AddVSProject()
		{
            try
            {
                /*2012-12-单元测试加*/
                if (loader == null) return;
                if (loader.TestProject == null) return;
                /*2012-12-单元测试加*/

			    OpenFileDialog dlg = new OpenFileDialog();
			    dlg.Title = "Add Visual Studio Project";

			    dlg.Filter =
				    "All Project Types (*.csproj,*.vjsproj,*.vbproj,*.vcproj)|*.csproj;*.vjsproj;*.vbproj;*.vcproj|" +
				    "C# Projects (*.csproj)|*.csproj|" +
				    "J# Projects (*.vjsproj)|*.vjsproj|" +
				    "VB Projects (*.vbproj)|*.vbproj|" +
				    "C++ Projects (*.vcproj)|*.vcproj|" +
				    "All Files (*.*)|*.*";

			    dlg.FilterIndex = 1;
			    dlg.FileName = "";

			    if ( dlg.ShowDialog( Form ) == DialogResult.OK ) 
			    {				
				    VSProject vsProject = new VSProject( dlg.FileName );
				    loader.TestProject.Add( vsProject );
                }
            }
            /*2012-12-29单元测试加*/
            catch (CADException exception)
            {                
                if (Form != null)
                    Form.MessageDisplay.Error("Invalid VS Project", exception);
                else
                    CADApplication.ShowAlertDialog("Invalid VS Project\n" + exception.Message);
            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Invalid VS Project", exception);
                else
                    CADApplication.ShowAlertDialog("Invalid VS Project\n" + exception.Message);
            }
            /*2012-12-29单元测试加*/
        }

        #endregion

        #region Save Methods

        public void SaveProject()
		{
            try//2013-1-1单元测试NUnit.Gui.ArxNet.Tests.NUnitPresenterArxNetTests.SaveProject加
            {
                if (loader == null) return;//2013-1-1单元测试加
                if (loader.TestProject == null) return;//2013-1-1单元测试加

			if ( Path.IsPathRooted( loader.TestProject.ProjectPath ) &&
				 NUnitProject.IsNUnitProjectFile( loader.TestProject.ProjectPath ) &&
				 CanWriteProjectFile( loader.TestProject.ProjectPath ) )
				loader.TestProject.Save();
			else
				SaveProjectAs();
		}
            /*2013-1-1单元测试加*/
            catch (CADException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Save Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Save Project\n" + exception.Message);

            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Save Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Save Project\n" + exception.Message);
            }
            /*2013-1-1单元测试加*/

        }

		public void SaveProjectAs()
		{
            try//2013-1-1单元测试NUnit.Gui.ArxNet.Tests.NUnitPresenterArxNetTests.SaveProjectAs加
            {
                if (loader == null) return;//2013-1-1单元测试加
                if (loader.TestProject == null) return;//2013-1-1单元测试加

			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Title = "Save Test Project";
			dlg.Filter = "NUnit Test Project (*.nunit)|*.nunit|All Files (*.*)|*.*";
			string path = NUnitProject.ProjectPathFromFile( loader.TestProject.ProjectPath );
			if ( CanWriteProjectFile( path ) )
				dlg.FileName = path;
			dlg.DefaultExt = "nunit";
			dlg.ValidateNames = true;
			dlg.OverwritePrompt = true;

			while( dlg.ShowDialog( Form ) == DialogResult.OK )
			{
				if ( !CanWriteProjectFile( dlg.FileName ) )
                    Form.MessageDisplay.Info(string.Format("File {0} is write-protected. Select another file name.", dlg.FileName));
				else
				{
					loader.TestProject.Save( dlg.FileName );
                    ReloadProject();
                    return;
				}
			}
        }
            /*2013-1-1单元测试加*/
            catch (CADException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Save Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Save Project\n" + exception.Message);
            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Save Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Save Project\n" + exception.Message);
            }
            /*2013-1-1单元测试加*/
        }

        private DialogResult SaveProjectIfDirty()
        {
            try//2012-12-29单元测试加
            {
                if (loader == null) return DialogResult.No;//2012-12-29单元测试加

                DialogResult result = DialogResult.OK;
            NUnitProject project = loader.TestProject;

                if (project == null) return DialogResult.No;//2012-12-29单元测试加

            if (project.IsDirty)
            {
                string msg = string.Format(
                    "Project {0} has been changed. Do you want to save changes?", project.Name);

                result = Form.MessageDisplay.Ask(msg, MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                    SaveProject();
            }

            return result;
        }
            /*2012-12-29单元测试加*/
            catch (CADException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Save the Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Save the Project\n" + exception.Message);

                return DialogResult.No;
            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Save the Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Save the Project\n" + exception.Message);

                return DialogResult.No;
            }
            /*2012-12-29单元测试加*/
        }

        public void SaveLastResult()
        {
            try
            {
                if (loader == null) return;//2013-1-1单元测试加
                if (loader.TestResult == null) return;//2013-1-1单元测试加

                //TODO: Save all results
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Title = "Save Test Results as XML";
                dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                dlg.FileName = "TestResult.xml";
                dlg.InitialDirectory = Path.GetDirectoryName(loader.TestFileName);
                dlg.DefaultExt = "xml";
                dlg.ValidateNames = true;
                dlg.OverwritePrompt = true;

                if (dlg.ShowDialog(Form) == DialogResult.OK)
                {
                    string fileName = dlg.FileName;

                    loader.SaveLastResult(fileName);

                    Form.MessageDisplay.Info(String.Format("Results saved as {0}", fileName));
                }
            }
            /*2012-12-29单元测试加*/
            catch (CADException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Save Results", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Save Results\n" + exception.Message);

            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Save Results", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Save Results\n" + exception.Message);
            }
            /*2012-12-29单元测试加*/
        }

        #endregion

        #region Reload Methods

        public void ReloadProject()
        {
            try//2012-12-31单元测试加
            {
                if (loader == null) return;//2012-12-31单元测试加

            NUnitProject project = loader.TestProject;

                if (project == null) return;//2012-12-31单元测试加

            bool wrapper = project.IsAssemblyWrapper;
            string projectPath = project.ProjectPath;
            string activeConfigName = project.ActiveConfigName;

            // Unload first to avoid message asking about saving
            loader.UnloadProject();

            if (wrapper)
                OpenProject(projectPath);
            else
                OpenProject(projectPath, activeConfigName, null);
        }
            /*2012-12-31单元测试加*/
            catch (CADException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Reload the Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Reload the Project\n" + exception.Message);
            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Reload the Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Reload the Project\n" + exception.Message);
            }
            /*2012-12-31单元测试加*/
        }

        #endregion

        #region Edit Project

        public void EditProject()
        {
            try//2012-12-30单元测试加
            {
                if (loader == null) return;//2012-12-30单元测试加

            NUnitProject project = loader.TestProject;

                if (project == null) return;

            string editorPath = GetProjectEditorPath();
            if (!File.Exists(editorPath))
            {
                string NL = Environment.NewLine;
                string message =
                    "Unable to locate the specified Project Editor:" + NL + NL + editorPath + NL + NL +
                        (ServicesArxNet.UserSettings.GetSetting("Options.ProjectEditor.EditorPath") == null
                        ? "Verify that nunit.editor.exe is properly installed in the NUnit bin directory."
                        : "Verify that you have set the path to the editor correctly.");

                    if (Form != null)
                        Form.MessageDisplay.Error(message);
                    else
                        CADApplication.ShowAlertDialog(message);

                return;
            }

            if (!NUnitProject.IsNUnitProjectFile(project.ProjectPath))
            {
                if (Form.MessageDisplay.Display(
                    "The project has not yet been saved. In order to edit the project, it must first be saved. Click OK to save the project or Cancel to exit.",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    project.Save();
                }
            }
            else if (!File.Exists(project.ProjectPath))
            {
                project.Save();
            }
            else if (project.IsDirty)
            {
                switch (Form.MessageDisplay.Ask(
                    "There are unsaved changes. Do you want to save them before running the editor?",
                    MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        project.Save();
                        break;

                    case DialogResult.Cancel:
                        return;
                }
            }

            // In case we tried to save project and failed
            if (NUnitProject.IsNUnitProjectFile(project.ProjectPath) && File.Exists(project.ProjectPath))
            {
                Process p = new Process();

                p.StartInfo.FileName = Quoted(editorPath);
                p.StartInfo.Arguments = Quoted(project.ProjectPath);
                p.Start();
            }
        }
            /*2012-12-30单元测试加*/
            catch (CADException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Edit the Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Edit the Project\n" + exception.Message);                
            }
            catch (SystemException exception)
            {
                if (Form != null)
                    Form.MessageDisplay.Error("Unable to Edit the Project", exception);
                else
                    CADApplication.ShowAlertDialog("Unable to Edit the Project\n" + exception.Message);                
            }
            /*2012-12-30单元测试加*/
        }

        #endregion

        #endregion

        #region Helper Properties and Methods

        private static bool VisualStudioSupport
        {
            get
            {
                return ServicesArxNet.UserSettings.GetSetting("Options.TestLoader.VisualStudioSupport", false);
            }
        }

        private static bool CanWriteProjectFile(string path)
        {
            return !File.Exists(path) ||
                (File.GetAttributes(path) & FileAttributes.ReadOnly) == 0;
        }

        private static string GetProjectEditorPath()
        {
            string editorPath = (string)ServicesArxNet.UserSettings.GetSetting("Options.ProjectEditor.EditorPath");
            if (editorPath == null)
                editorPath = Path.Combine(NUnitConfiguration.NUnitBinDirectory, "nunit-editor.exe");

            return editorPath;
        }

        private static string Quoted(string s)
        {
            return "\"" + s + "\"";
        }

        #endregion
    }
}
