﻿// ****************************************************************
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
using System.IO;
using System.Xml;
using System.Text;
using NUnit.Framework;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.NUnitProjectArxNetTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class NUnitProjectArxNetTestsCommands
    {
        //public void IsProjectFile()
        [CommandMethod("IsProjectFile")]
        public void IsProjectFile()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.IsProjectFile();
            tests.EraseFile();
        }

        //public void NewProjectIsEmpty()
        [CommandMethod("NewProjectIsEmpty")]
        public void NewProjectIsEmpty()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.NewProjectIsEmpty();
            tests.EraseFile();
        }

        //public void NewProjectIsNotDirty()
        [CommandMethod("NewProjectIsNotDirty")]
        public void NewProjectIsNotDirty()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.NewProjectIsNotDirty();
            tests.EraseFile();
        }

        //public void NewProjectDefaultPath()
        [CommandMethod("NewProjectDefaultPath")]
        public void NewProjectDefaultPath()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.NewProjectDefaultPath();
            tests.EraseFile();
        }

        //public void NewProjectNotLoadable()
        [CommandMethod("NewProjectNotLoadable")]
        public void NewProjectNotLoadable()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.NewProjectNotLoadable();
            tests.EraseFile();
        }

        //public void SaveMakesProjectNotDirty()
        [CommandMethod("SaveMakesProjectNotDirty")]
        public void SaveMakesProjectNotDirty()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.SaveMakesProjectNotDirty();
            tests.EraseFile();
        }

        //public void SaveSetsProjectPath()
        [CommandMethod("SaveSetsProjectPath")]
        public void SaveSetsProjectPath()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.SaveSetsProjectPath();
            tests.EraseFile();
        }

        //public void DefaultApplicationBase()
        [CommandMethod("DefaultApplicationBase")]
        public void DefaultApplicationBase()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.DefaultApplicationBase();
            tests.EraseFile();
        }

        //public void DefaultConfigurationFile()
        [CommandMethod("DefaultConfigurationFile")]
        public void DefaultConfigurationFile()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.DefaultConfigurationFile();
            tests.EraseFile();
        }

        //public void ConfigurationFileFromAssembly()
        [CommandMethod("ConfigurationFileFromAssembly")]
        public void ConfigurationFileFromAssembly()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.ConfigurationFileFromAssembly();
            tests.EraseFile();
        }

        //public void ConfigurationFileFromAssemblies()
        [CommandMethod("ConfigurationFileFromAssemblies")]
        public void ConfigurationFileFromAssemblies()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.ConfigurationFileFromAssemblies();
            tests.EraseFile();
        }

        //public void DefaultProjectName()
        [CommandMethod("DefaultProjectName")]
        public void DefaultProjectName()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.DefaultProjectName();
            tests.EraseFile();
        }

        //public void LoadMakesProjectNotDirty()
        [CommandMethod("LoadMakesProjectNotDirty")]
        public void LoadMakesProjectNotDirty()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.LoadMakesProjectNotDirty();
            tests.EraseFile();
        }

        //public void CanSetAppBase()
        [CommandMethod("CanSetAppBase")]
        public void CanSetAppBase()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.CanSetAppBase();
            tests.EraseFile();
        }

        //public void CanAddConfigs()
        [CommandMethod("CanAddConfigs")]
        public void CanAddConfigs()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.CanAddConfigs();
            tests.EraseFile();
        }

        //public void CanSetActiveConfig()
        [CommandMethod("CanSetActiveConfig")]
        public void CanSetActiveConfig()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.CanSetActiveConfig();
            tests.EraseFile();
        }

        //public void CanAddAssemblies()
        [CommandMethod("CanAddAssemblies")]
        public void CanAddAssemblies()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.CanAddAssemblies();
            tests.EraseFile();
        }

        //public void AddConfigMakesProjectDirty()
        [CommandMethod("AddConfigMakesProjectDirty")]
        public void AddConfigMakesProjectDirty()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.AddConfigMakesProjectDirty();
            tests.EraseFile();
        }

        //public void RenameConfigMakesProjectDirty()
        [CommandMethod("RenameConfigMakesProjectDirty")]
        public void RenameConfigMakesProjectDirty()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.RenameConfigMakesProjectDirty();
            tests.EraseFile();
        }

        //public void DefaultActiveConfig()
        [CommandMethod("DefaultActiveConfig")]
        public void DefaultActiveConfig()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.DefaultActiveConfig();
            tests.EraseFile();
        }

        //public void RenameActiveConfig()
        [CommandMethod("RenameActiveConfig")]
        public void RenameActiveConfig()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.RenameActiveConfig();
            tests.EraseFile();
        }

        //public void RemoveConfigMakesProjectDirty()
        [CommandMethod("RemoveConfigMakesProjectDirty")]
        public void RemoveConfigMakesProjectDirty()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.RemoveConfigMakesProjectDirty();
            tests.EraseFile();
        }

        //public void RemoveActiveConfig()
        [CommandMethod("RemoveActiveConfig")]
        public void RemoveActiveConfig()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.RemoveActiveConfig();
            tests.EraseFile();
        }

        //public void SettingActiveConfigMakesProjectDirty()
        [CommandMethod("SettingActiveConfigMakesProjectDirty")]
        public void SettingActiveConfigMakesProjectDirty()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.SettingActiveConfigMakesProjectDirty();
            tests.EraseFile();
        }

        //public void SaveAndLoadEmptyProject()
        [CommandMethod("SaveAndLoadEmptyProject")]
        public void SaveAndLoadEmptyProject()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.SaveAndLoadEmptyProject();
            tests.EraseFile();
        }

        //public void SaveAndLoadEmptyConfigs()
        [CommandMethod("SaveAndLoadEmptyConfigs")]
        public void SaveAndLoadEmptyConfigs()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.SaveAndLoadEmptyConfigs();
            tests.EraseFile();
        }

        //public void SaveAndLoadConfigsWithAssemblies()
        [CommandMethod("SaveAndLoadConfigsWithAssemblies")]
        public void SaveAndLoadConfigsWithAssemblies()
        {
            NUnitProjectArxNetTests tests = new NUnitProjectArxNetTests();
            tests.SetUp();
            tests.SaveAndLoadConfigsWithAssemblies();
            tests.EraseFile();
        }
    }
}
