// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.9.22：
//  1.利用cad命令直接测试
//  2.测试未通过,可能是在CAD环境下不支持程序域下的测试？
// ****************************************************************

using System;
using NUnit.Framework;
using NUnit.Core;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(NUnit.Util.ArxNet.Tests.RemoteTestResultArxNetTestCommands))]

namespace NUnit.Util.ArxNet.Tests
{
    public class RemoteTestResultArxNetTestCommands
    {
        //public void ResultStillValidAfterDomainUnload()
        [CommandMethod("ResultStillValidAfterDomainUnload")]
        public void ResultStillValidAfterDomainUnload()
        {
            RemoteTestResultArxNetTest tests = new RemoteTestResultArxNetTest();
            tests.CreateRunner();
            tests.ResultStillValidAfterDomainUnload();
            tests.UnloadRunner();
        }

        //public void AppDomainUnloadedBug()
        [CommandMethod("AppDomainUnloadedBug")]
        public void AppDomainUnloadedBug()
        {
            RemoteTestResultArxNetTest tests = new RemoteTestResultArxNetTest();
            tests.CreateRunner();
            tests.AppDomainUnloadedBug();
            tests.UnloadRunner();
        }
    }
}
