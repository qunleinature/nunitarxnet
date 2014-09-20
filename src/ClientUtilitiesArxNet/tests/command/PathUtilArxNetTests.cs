// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.9.19：
//  利用cad命令直接测试
// ****************************************************************

using System;
using System.IO;
using NUnit.Framework;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.PathUtilArxNetTestsCommands))]
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.PathUtilArxNetTests_WindowsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class PathUtilArxNetTestsCommands
    {
        //public void CheckDefaults()
        [CommandMethod("CheckDefaults")]
        public void CheckDefaults()
        {
            PathUtilArxNetTests tests = new PathUtilArxNetTests();
            tests.CheckDefaults();
        }
    }

    public class PathUtilArxNetTests_WindowsCommands
    {        
        public PathUtilArxNetTests_WindowsCommands()
        {
            PathUtilArxNetTests_Windows.SetUpUnixSeparators();
        }

        ~PathUtilArxNetTests_WindowsCommands()
        {
            PathUtilArxNetTests_Windows.RestoreDefaultSeparators();
        }

        //public void IsAssemblyFileType()
        [CommandMethod("IsAssemblyFileType")]
        public void IsAssemblyFileType()
        {
            PathUtilArxNetTests_Windows tests = new PathUtilArxNetTests_Windows();
            tests.IsAssemblyFileType();
        }

        //public void Canonicalize()
        [CommandMethod("Canonicalize")]
        public void Canonicalize()
        {
            PathUtilArxNetTests_Windows tests = new PathUtilArxNetTests_Windows();
            tests.Canonicalize();
        }
    }
}
