// ****************************************************************
// Copyright 2009, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
//  2014.9.25：
//      利用cad命令直接测试
// ****************************************************************

using System;
using System.Diagnostics;
using NUnit.Framework;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.RemoteTestAgentArxNetTestsCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class RemoteTestAgentArxNetTestsCommands
    {
        //public void AgentReturnsProcessId()
        [CommandMethod("AgentReturnsProcessId")]
        public void AgentReturnsProcessId()
        {
            RemoteTestAgentArxNetTests tests = new RemoteTestAgentArxNetTests();
            tests.AgentReturnsProcessId();
        }
    }
}
