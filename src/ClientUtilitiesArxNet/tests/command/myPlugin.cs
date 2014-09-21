// (C) Copyright 2014 by  
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: ExtensionApplication(typeof(NUnit.Util.ArxNet.Tests.MyPlugin))]

namespace NUnit.Util.ArxNet.Tests
{

    // This class is instantiated by AutoCAD once and kept alive for the 
    // duration of the session. If you don't do any one time initialization 
    // then you should remove this class.
    public class MyPlugin : IExtensionApplication
    {
        private ServiceManagerArxNetSetUpFixture m_fixture = null;

        void IExtensionApplication.Initialize()
        {
            // Add one time initialization here
            // One common scenario is to setup a callback function here that 
            // unmanaged code can call. 
            // To do this:
            // 1. Export a function from unmanaged code that takes a function
            //    pointer and stores the passed in value in a global variable.
            // 2. Call this exported function in this function passing delegate.
            // 3. When unmanaged code needs the services of this managed module
            //    you simply call acrxLoadApp() and by the time acrxLoadApp 
            //    returns  global function pointer is initialized to point to
            //    the C# delegate.
            // For more info see: 
            // http://msdn2.microsoft.com/en-US/library/5zwkzwf4(VS.80).aspx
            // http://msdn2.microsoft.com/en-us/library/44ey4b32(VS.80).aspx
            // http://msdn2.microsoft.com/en-US/library/7esfatk4.aspx
            // as well as some of the existing AutoCAD managed apps.

            // Initialize your plug-in application here
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed;
            if (doc != null)
            {
                ed = doc.Editor;

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.DomainManagerArxNetTestsCommands:");
                ed.WriteMessage("\n\tGetPrivateBinPath");
                ed.WriteMessage("\n\tGetCommonAppBase_OneElement");
                ed.WriteMessage("\n\tGetCommonAppBase_TwoElements_SameDirectory");
                ed.WriteMessage("\n\tGetCommonAppBase_TwoElements_DifferentDirectories");
                ed.WriteMessage("\n\tGetCommonAppBase_ThreeElements_DiferentDirectories");
                ed.WriteMessage("\n\tUnloadUnloadedDomain");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.NUnitProjectArxNetLoadCommands:");
                ed.WriteMessage("\n\tLoadEmptyProject");
                ed.WriteMessage("\n\tLoadEmptyConfigs");
                ed.WriteMessage("\n\tLoadNormalProject");
                ed.WriteMessage("\n\tLoadProjectWithManualBinPath");
                ed.WriteMessage("\n\tFromAssembly");
                ed.WriteMessage("\n\tSaveClearsAssemblyWrapper");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.NUnitProjectArxNetSaveCommands:");
                ed.WriteMessage("\n\tSaveEmptyProject");
                ed.WriteMessage("\n\tSaveEmptyConfigs");
                ed.WriteMessage("\n\tSaveNormalProject");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.NUnitProjectArxNetTestsCommands:");
                ed.WriteMessage("\n\tIsProjectFile");
                ed.WriteMessage("\n\tNewProjectIsEmpty");
                ed.WriteMessage("\n\tNewProjectIsNotDirty");
                ed.WriteMessage("\n\tNewProjectDefaultPath");
                ed.WriteMessage("\n\tNewProjectNotLoadable");
                ed.WriteMessage("\n\tSaveMakesProjectNotDirty");
                ed.WriteMessage("\n\tSaveSetsProjectPath");
                ed.WriteMessage("\n\tDefaultApplicationBase");
                ed.WriteMessage("\n\tDefaultConfigurationFile");
                ed.WriteMessage("\n\tConfigurationFileFromAssembly");
                ed.WriteMessage("\n\tConfigurationFileFromAssemblies");
                ed.WriteMessage("\n\tDefaultProjectName");
                ed.WriteMessage("\n\tLoadMakesProjectNotDirty");
                ed.WriteMessage("\n\tCanSetAppBase");
                ed.WriteMessage("\n\tCanAddConfigs");
                ed.WriteMessage("\n\tCanSetActiveConfig");
                ed.WriteMessage("\n\tCanAddAssemblies");
                ed.WriteMessage("\n\tAddConfigMakesProjectDirty");
                ed.WriteMessage("\n\tRenameConfigMakesProjectDirty");
                ed.WriteMessage("\n\tDefaultActiveConfig");
                ed.WriteMessage("\n\tRenameActiveConfig");
                ed.WriteMessage("\n\tRemoveActiveConfig");
                ed.WriteMessage("\n\tSettingActiveConfigMakesProjectDirty");
                ed.WriteMessage("\n\tSaveAndLoadEmptyProject");
                ed.WriteMessage("\n\tSaveAndLoadEmptyConfigs");
                ed.WriteMessage("\n\tSaveAndLoadConfigsWithAssemblies");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.PathUtilArxNetTestsCommands:");
                ed.WriteMessage("\n\tCheckDefaults");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.PathUtilArxNetTests_WindowsCommands:");
                ed.WriteMessage("\n\tPathUtilArxNetTests_WindowsCommands.IsAssemblyFileType");
                ed.WriteMessage("\n\tPathUtilArxNetTests_WindowsCommands.Canonicalize");
                ed.WriteMessage("\n\tPathUtilArxNetTests_WindowsCommands.SamePath");
                ed.WriteMessage("\n\tPathUtilArxNetTests_WindowsCommands.SamePathOrUnder");
                ed.WriteMessage("\n\tPathUtilArxNetTests_WindowsCommands.PathFromUri");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.PathUtilArxNetTests_UnixCommands:");
                ed.WriteMessage("\n\tPathUtilArxNetTests_UnixCommands.IsAssemblyFileType");
                ed.WriteMessage("\n\tPathUtilArxNetTests_UnixCommands.Canonicalize");
                ed.WriteMessage("\n\tRelativePath");
                ed.WriteMessage("\n\tPathUtilArxNetTests_UnixCommands.SamePath");
                ed.WriteMessage("\n\tPathUtilArxNetTests_UnixCommands.SamePathOrUnder");
                ed.WriteMessage("\n\tPathUtilArxNetTests_UnixCommands.PathFromUri");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.ProcessRunnerArxNetTestsCommands:");
                ed.WriteMessage("\n\tTestProcessIsReused--测试未通过,致命错误：可能在CAD环境下进程错误");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.RecentFilesArxNetTestsCommands:");
                ed.WriteMessage("\n\tCountDefault");
                ed.WriteMessage("\n\tCountOverMax");
                ed.WriteMessage("\n\tCountUnderMin");
                ed.WriteMessage("\n\tCountAtMax");
                ed.WriteMessage("\n\tCountAtMin");
                ed.WriteMessage("\n\tEmptyList");
            }

            m_fixture = new ServiceManagerArxNetSetUpFixture();
            m_fixture.CreateServicesForTestDomain();
        }

        void IExtensionApplication.Terminate()
        {
            // Do plug-in application clean up here
            m_fixture.ClearServices();
            m_fixture = null;
        }

    }

}
