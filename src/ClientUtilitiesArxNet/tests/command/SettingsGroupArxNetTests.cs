// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2014.9.22：
//      1.利用cad命令直接测试
//  2015.2.6：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using NUnit.Framework;
using Microsoft.Win32;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.SettingsGroupArxNetTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class SettingsGroupArxNetTestsCommands
    {
        //public void TopLevelSettings()
        [CommandMethod("TopLevelSettings")]
        public void TopLevelSettings()
        {
            SettingsGroupArxNetTests tests = new SettingsGroupArxNetTests();
            tests.BeforeEachTest();
            tests.TopLevelSettings();
            tests.AfterEachTest();
        }

        //public void SubGroupSettings()
        [CommandMethod("SubGroupSettings")]
        public void SubGroupSettings()
        {
            SettingsGroupArxNetTests tests = new SettingsGroupArxNetTests();
            tests.BeforeEachTest();
            tests.SubGroupSettings();
            tests.AfterEachTest();
        }

        //public void TypeSafeSettings()
        [CommandMethod("TypeSafeSettings")]
        public void TypeSafeSettings()
        {
            SettingsGroupArxNetTests tests = new SettingsGroupArxNetTests();
            tests.BeforeEachTest();
            tests.TypeSafeSettings();
            tests.AfterEachTest();
        }

        //public void DefaultSettings()
        [CommandMethod("DefaultSettings")]
        public void DefaultSettings()
        {
            SettingsGroupArxNetTests tests = new SettingsGroupArxNetTests();
            tests.BeforeEachTest();
            tests.DefaultSettings();
            tests.AfterEachTest();
        }

        //public void BadSetting()
        [CommandMethod("BadSetting")]
        public void BadSetting()
        {
            SettingsGroupArxNetTests tests = new SettingsGroupArxNetTests();
            tests.BeforeEachTest();
            tests.BadSetting();
            tests.AfterEachTest();
        }
    }
}
