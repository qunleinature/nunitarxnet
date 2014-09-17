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
