// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.9.17：
//  利用cad命令直接测试
// ****************************************************************

using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

using NUnit.Framework;
using NUnit.TestUtilities;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.NUnitProjectArxNetLoadCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class NUnitProjectArxNetLoadCommands
    {
        //public void LoadEmptyProject()
        [CommandMethod("LoadEmptyProject")]
        public void LoadEmptyProject()
        {
            NUnitProjectArxNetLoad load = new NUnitProjectArxNetLoad();
            load.SetUp();
            load.LoadEmptyProject();
            load.TearDown();
        }

        //public void LoadEmptyConfigs()
        [CommandMethod("LoadEmptyConfigs")]
        public void LoadEmptyConfigs()
        {
            NUnitProjectArxNetLoad load = new NUnitProjectArxNetLoad();
            load.SetUp();
            load.LoadEmptyConfigs();
            load.TearDown();
        }
        
        //public void LoadNormalProject()
        [CommandMethod("LoadNormalProject")]
        public void LoadNormalProject()
        {
            NUnitProjectArxNetLoad load = new NUnitProjectArxNetLoad();
            load.SetUp();
            load.LoadNormalProject();
            load.TearDown();
        }

        //public void LoadProjectWithManualBinPath()
        [CommandMethod("LoadProjectWithManualBinPath")]
        public void LoadProjectWithManualBinPath()
        {
            NUnitProjectArxNetLoad load = new NUnitProjectArxNetLoad();
            load.SetUp();
            load.LoadProjectWithManualBinPath();
            load.TearDown();
        }
        
        //public void FromAssembly()
        [CommandMethod("FromAssembly")]
        public void FromAssembly()
        {
            NUnitProjectArxNetLoad load = new NUnitProjectArxNetLoad();
            load.SetUp();
            load.FromAssembly();
            load.TearDown();
        }

        //public void SaveClearsAssemblyWrapper()
        [CommandMethod("SaveClearsAssemblyWrapper")]
        public void SaveClearsAssemblyWrapper()
        {
            NUnitProjectArxNetLoad load = new NUnitProjectArxNetLoad();
            load.SetUp();
            load.SaveClearsAssemblyWrapper();
            load.TearDown();
        }
    }
}
