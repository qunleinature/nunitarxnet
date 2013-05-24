// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.5.25：
//  新增人工测试
// ****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using NUnit.Framework;

namespace NUnit.CommandRunner.ArxNet.Tests
{
    [TestFixture]
    public class CommandRunnerArxNetTest
    {
        //public static void Init()
        [Test]
        public void RunnerArxNetInit()
        {
            NUnit.CommandRunner.ArxNet.RunnerArxNet.Init();
            Console.WriteLine("RunnerArxNet初始化");
            Console.WriteLine("Console输出流已经设为EditorWriter");
        }

        //public static void CleanUp()
        [Test]
        public void RunnerArxNetCleanUp()
        {
            NUnit.CommandRunner.ArxNet.RunnerArxNet.Init();
            NUnit.CommandRunner.ArxNet.RunnerArxNet.CleanUp();
            Console.WriteLine("RunnerArxNet清除");
            Console.WriteLine("Console输出流已恢复");
        }
    }
}
