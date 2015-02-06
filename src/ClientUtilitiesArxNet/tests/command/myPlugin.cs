﻿// ****************************************************************
// Copyright 2015, Lei Qun
//  2015.2.6：
//      利用cad命令直接测试
// ****************************************************************


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

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.RemoteTestResultArxNetTestCommands:");
                ed.WriteMessage("\n\tResultStillValidAfterDomainUnload--测试未通过,可能是在CAD环境下不支持程序域下的测试");
                ed.WriteMessage("\n\tAppDomainUnloadedBug--测试未通过,可能是在CAD环境下不支持程序域下的测试");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.RuntimeFrameworkSelectorArxNetTestsCommands:");
                ed.WriteMessage("\n\tRequestForSpecificFrameworkIsHonored");
                ed.WriteMessage("\n\tRequestForSpecificVersionIsHonored");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.SettingsGroupArxNetTestsCommands:");
                ed.WriteMessage("\n\tTopLevelSettings");
                ed.WriteMessage("\n\tSubGroupSettings");
                ed.WriteMessage("\n\tTypeSafeSettings");
                ed.WriteMessage("\n\tDefaultSettings");
                ed.WriteMessage("\n\tBadSetting");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.RemoteTestAgentArxNetTestsCommands:");
                ed.WriteMessage("\n\tAgentReturnsProcessId");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.TestDomainArxNetFixtureCommands:");
                ed.WriteMessage("\n\tAssemblyIsLoadedCorrectly--测试未通过，可能是在CAD环境下不支持程序域下的测试");
                ed.WriteMessage("\n\tCanRunMockAssemblyTests--测试未通过，可能是在CAD环境下不支持程序域下的测试");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.TestDomainArxNetTestsCommands:");
                ed.WriteMessage("\n\tTestDomainArxNetTestsCommands.FileNotFound");
                ed.WriteMessage("\n\tInvalidTestFixture--测试未通过，可能是在CAD环境下不支持程序域下的测试");
                ed.WriteMessage("\n\tFileFoundButNotValidAssembly");
                ed.WriteMessage("\n\tSpecificTestFixture--测试未通过，可能是在CAD环境下不支持程序域下的测试");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.TestDomainArxNetTests_MultipleCommands:");
                ed.WriteMessage("\n\tBuildSuite--测试未通过，可能是在CAD环境下不支持程序域下的测试");
                ed.WriteMessage("\n\tRootNode--测试未通过，可能是在CAD环境下不支持程序域下的测试");
                ed.WriteMessage("\n\tAssemblyNodes--测试未通过，可能是在CAD环境下不支持程序域下的测试");
                ed.WriteMessage("\n\tTestCaseCount--测试未通过，可能是在CAD环境下不支持程序域下的测试");
                ed.WriteMessage("\n\tRunMultipleAssemblies--测试未通过，可能是在CAD环境下不支持程序域下的测试");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.TestDomainArxNetTests_MultipleFixtureCommands:");
                ed.WriteMessage("\n\tLoadFixture--测试未通过，可能是在CAD环境下不支持程序域下的测试");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.TestLoaderArxNetAssemblyTestsCommands:");
                ed.WriteMessage("\n\tLoadProject");
                ed.WriteMessage("\n\tUnloadProject");
                ed.WriteMessage("\n\tLoadTest");
                ed.WriteMessage("\n\tUnloadTest");
                ed.WriteMessage("\n\tTestLoaderArxNetAssemblyTestsCommands.FileNotFound");
                ed.WriteMessage("\n\tInvalidAssembly");
                ed.WriteMessage("\n\tAssemblyWithNoTests");
                ed.WriteMessage("\n\tRunTest");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.TestLoaderArxNetWatcherTestsCommands:");
                ed.WriteMessage("\n\tLoadShouldStartWatcher");
                ed.WriteMessage("\n\tReloadShouldStartWatcher");
                ed.WriteMessage("\n\tUnloadShouldStopWatcherAndFreeResources");
                ed.WriteMessage("\n\tLoadShouldStartWatcherDependingOnSettings");
                ed.WriteMessage("\n\tReloadShouldStartWatcherDependingOnSettings");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.TestRunnerFactoryArxNetTestsCommands:");
                ed.WriteMessage("\n\tSameFrameworkUsesTestDomain");
                ed.WriteMessage("\n\tDifferentRuntimeUsesProcessRunner");
                ed.WriteMessage("\n\tDifferentVersionUsesProcessRunner");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.VisualStudioConverterArxNetTestsCommands:");
                ed.WriteMessage("\n\tFromCSharpProject");
                ed.WriteMessage("\n\tFromVBProject");
                ed.WriteMessage("\n\tFromJsharpProject");
                ed.WriteMessage("\n\tFromCppProject");
                ed.WriteMessage("\n\tFromProjectWithHebrewFileIncluded");
                ed.WriteMessage("\n\tFromVSSolution2003");
                ed.WriteMessage("\n\tFromVSSolution2005");
                ed.WriteMessage("\n\tFromWebApplication");
                ed.WriteMessage("\n\tWithUnmanagedCpp");
                ed.WriteMessage("\n\tFromMakefileProject");
                ed.WriteMessage("\n\tFromSolutionWithDisabledProject");

                ed.WriteMessage("\nNUnit.Util.ArxNet.Tests.AgentReturnsProcessIdCommands:");
                ed.WriteMessage("\n\tAgentReturnsProcessId");
                
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
