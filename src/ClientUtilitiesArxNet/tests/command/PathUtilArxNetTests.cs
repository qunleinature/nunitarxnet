﻿// ****************************************************************
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
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.PathUtilArxNetTests_UnixCommands))]

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
        //public void IsAssemblyFileType()
        [CommandMethod("PathUtilArxNetTests_WindowsCommands", "IsAssemblyFileType", CommandFlags.Modal)]
        public void IsAssemblyFileType()
        {
            PathUtilArxNetTests_Windows.SetUpUnixSeparators();
            PathUtilArxNetTests_Windows tests = new PathUtilArxNetTests_Windows();
            tests.IsAssemblyFileType();
            PathUtilArxNetTests_Windows.RestoreDefaultSeparators();
        }

        //public void Canonicalize()
        [CommandMethod("Canonicalize")]
        public void Canonicalize()
        {
            PathUtilArxNetTests_Windows.SetUpUnixSeparators();
            PathUtilArxNetTests_Windows tests = new PathUtilArxNetTests_Windows();
            tests.Canonicalize();
            PathUtilArxNetTests_Windows.RestoreDefaultSeparators();
        }

        //public void SamePath()
        [CommandMethod("SamePath")]
        public void SamePath()
        {
            PathUtilArxNetTests_Windows.SetUpUnixSeparators();
            PathUtilArxNetTests_Windows tests = new PathUtilArxNetTests_Windows();
            tests.SamePath();
            PathUtilArxNetTests_Windows.RestoreDefaultSeparators();
        }

        //public void SamePathOrUnder()
        [CommandMethod("SamePathOrUnder")]
        public void SamePathOrUnder()
        {
            PathUtilArxNetTests_Windows.SetUpUnixSeparators();
            PathUtilArxNetTests_Windows tests = new PathUtilArxNetTests_Windows();
            tests.SamePathOrUnder();
            PathUtilArxNetTests_Windows.RestoreDefaultSeparators();
        }

        //public void PathFromUri()
        [CommandMethod("PathFromUri")]
        public void PathFromUri()
        {
            PathUtilArxNetTests_Windows.SetUpUnixSeparators();
            PathUtilArxNetTests_Windows tests = new PathUtilArxNetTests_Windows();
            tests.PathFromUri();
            PathUtilArxNetTests_Windows.RestoreDefaultSeparators();
        }
    }

    public class PathUtilArxNetTests_UnixCommands
    {
        //public void IsAssemblyFileType()
        [CommandMethod("PathUtilArxNetTests_UnixCommands", "IsAssemblyFileType", CommandFlags.Modal)]
        public void IsAssemblyFileType()
        {
            PathUtilArxNetTests_Unix.SetUpUnixSeparators();
            PathUtilArxNetTests_Unix tests = new PathUtilArxNetTests_Unix();
            tests.IsAssemblyFileType();
            PathUtilArxNetTests_Unix.RestoreDefaultSeparators();
        }
    }
}
