// ****************************************************************
// Copyright 2012, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.9.22：
//  1.利用cad命令直接测试
// ****************************************************************

using System;
using NUnit.Core;
using NUnit.Framework;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.RuntimeFrameworkSelectorArxNetTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class RuntimeFrameworkSelectorArxNetTestsCommands
    {
        internal RuntimeFramework[] frameworks = new RuntimeFramework[] { 
            RuntimeFramework.Parse("net-1.0"), 
            RuntimeFramework.Parse("net-1.1"), 
            RuntimeFramework.Parse("net-2.0"),
            RuntimeFramework.Parse("net-4.0"),
            RuntimeFramework.Parse("mono-1.0"),
            RuntimeFramework.Parse("mono-2.0"),
            RuntimeFramework.Parse("v1.1"),
            RuntimeFramework.Parse("v2.0"),
            RuntimeFramework.Parse("v4.0")
            // TODO: Figure out a way to run these
            //RuntimeFramework.Parse("net"),
            //RuntimeFramework.Parse("mono"),
            //RuntimeFramework.Parse("any")
        };

        //public void RequestForSpecificFrameworkIsHonored(RuntimeFramework requestedFramework)
        [CommandMethod("RequestForSpecificFrameworkIsHonored")]
        public void RequestForSpecificFrameworkIsHonored()
        {
            RuntimeFrameworkSelectorArxNetTests tests = new RuntimeFrameworkSelectorArxNetTests();
            foreach (RuntimeFramework requestedFramework in frameworks)
            {
                try
                {
                    tests.RequestForSpecificFrameworkIsHonored(requestedFramework);
                }
                catch (NUnit.Framework.InconclusiveException){}
            }            
        }

        //public void RequestForSpecificVersionIsHonored(RuntimeFramework requestedFramework)
        [CommandMethod("RequestForSpecificVersionIsHonored")]
        public void RequestForSpecificVersionIsHonored()
        {
            RuntimeFrameworkSelectorArxNetTests tests = new RuntimeFrameworkSelectorArxNetTests();
            foreach (RuntimeFramework requestedFramework in frameworks)
            {
                try
                {
                    tests.RequestForSpecificVersionIsHonored(requestedFramework);
                }
                catch (NUnit.Framework.InconclusiveException) { }
            }
        }
    }
}
