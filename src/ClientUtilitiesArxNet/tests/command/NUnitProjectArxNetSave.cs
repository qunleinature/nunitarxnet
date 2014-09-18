// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.9.18：
//  利用cad命令直接测试
// ****************************************************************

using System;
using System.Text;
using System.Xml;
using System.IO;
using NUnit.Framework;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.NUnitProjectArxNetSaveCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class NUnitProjectArxNetSaveCommands
    {
        //public void SaveEmptyProject()
        [CommandMethod("SaveEmptyProject")]
        public void SaveEmptyProject()
        {
            NUnitProjectArxNetSave save = new NUnitProjectArxNetSave();
            save.SetUp();
            save.SaveEmptyProject();
            save.TearDown();
        }

        //public void SaveEmptyConfigs()
        [CommandMethod("SaveEmptyConfigs")]
        public void SaveEmptyConfigs()
        {
            NUnitProjectArxNetSave save = new NUnitProjectArxNetSave();
            save.SetUp();
            save.SaveEmptyConfigs();
            save.TearDown();
        }

        //public void SaveNormalProject()
        [CommandMethod("SaveNormalProject")]
        public void SaveNormalProject()
        {
            NUnitProjectArxNetSave save = new NUnitProjectArxNetSave();
            save.SetUp();
            save.SaveNormalProject();
            save.TearDown();
        }
    }
}
