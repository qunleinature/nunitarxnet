// ****************************************************************
// Copyright 2012, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
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

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.DomainManagerArxNetTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class DomainManagerArxNetTestsCommands
    {
        //public void GetPrivateBinPath()
        [CommandMethod("GetPrivateBinPath")]
        public void GetPrivateBinPath()
        {
            DomainManagerArxNetTests tests = new DomainManagerArxNetTests();
            tests.GetPrivateBinPath();
        }

        //public void GetCommonAppBase_OneElement()
        [CommandMethod("GetCommonAppBase_OneElement")]
        public void GetCommonAppBase_OneElement()
        {
            DomainManagerArxNetTests tests = new DomainManagerArxNetTests();
            tests.GetCommonAppBase_OneElement();
        }

        //public void GetCommonAppBase_TwoElements_SameDirectory()
        [CommandMethod("GetCommonAppBase_TwoElements_SameDirectory")]
        public void GetCommonAppBase_TwoElements_SameDirectory()
        {
            DomainManagerArxNetTests tests = new DomainManagerArxNetTests();
            tests.GetCommonAppBase_TwoElements_SameDirectory();
        }

        //public void GetCommonAppBase_TwoElements_DifferentDirectories()
        [CommandMethod("GetCommonAppBase_TwoElements_DifferentDirectories")]
        public void GetCommonAppBase_TwoElements_DifferentDirectories()
        {
            DomainManagerArxNetTests tests = new DomainManagerArxNetTests();
            tests.GetCommonAppBase_TwoElements_DifferentDirectories();
        }

        //public void GetCommonAppBase_ThreeElements_DiferentDirectories()
        [CommandMethod("GetCommonAppBase_ThreeElements_DiferentDirectories")]
        public void GetCommonAppBase_ThreeElements_DiferentDirectories()
        {
            DomainManagerArxNetTests tests = new DomainManagerArxNetTests();
            tests.GetCommonAppBase_ThreeElements_DiferentDirectories();
        }

        //public void UnloadUnloadedDomain()
        [CommandMethod("UnloadUnloadedDomain")]
        public void UnloadUnloadedDomain()
        {
            DomainManagerArxNetTests tests = new DomainManagerArxNetTests();
            tests.UnloadUnloadedDomain();
        }
    }
}
