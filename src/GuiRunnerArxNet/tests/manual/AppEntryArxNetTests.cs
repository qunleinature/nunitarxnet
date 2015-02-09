// ****************************************************************
// Copyright 2015, Lei Qun
//  2012年12月24日，雷群修改:
//      1.添加测试项目：nunit-gui-arxnet.tests.manual（需人工交互）
//      2.测试类：AppEntryArxNetTests
//  2014.10.5：
//      在NUnit2.6.3基础上调试
//  2015.2.9：
//      在NUnit2.6.4基础上调试
// ****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

using NUnit.Framework;
using NUnit.UiKit;

using NUnit.Gui.ArxNet;
using NUnit.Core.ArxNet;

using Com.Utility.UnitTest;

namespace NUnit.Gui.ArxNet.Tests
{
    [TestFixture]
    public class AppEntryArxNetTests
    {
        //private static IMessageDisplay MessageDisplay
        [Test]
        public void MessageDisplay()
        {
            IMessageDisplay messageDisplay = UnitTestHelper.GetNonPublicStaticProperty(typeof(AppEntryArxNet), "MessageDisplay") as IMessageDisplay;
            Assert.That(messageDisplay, Is.Not.Null);
            messageDisplay.Display("NUnit.Gui.ArxNet.Tests.MessageDisplay");
        }

        //public static void Init()
        [Test]
        public void AppEntryArxNetInit()
        {
            AppEntryArxNet.Init();
        }

        //public static void CleanUp()
        [Test]
        public void AppEntryArxNetCleanUp()
        {
            AppEntryArxNet.Init();
            AppEntryArxNet.CleanUp();
        }

        //public int Main(string[] args)
        [Test]
        public void AppEntryArxNetRun()
        {            
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;            
            
            PromptStringOptions opt = new PromptStringOptions("args:");
            opt.AllowSpaces = true;
            PromptResult res = ed.GetString(opt);
            string[] args = new string[0];
            switch (res.Status)
            {
                case PromptStatus.OK:
                    if (res.StringResult.Trim() != "")
                    {
                        args = res.StringResult.Split(' ');
                    }
                    break;
                default:
                    break;
            }

            AppEntryArxNet.Init();
            int result = AppEntryArxNet.Main(args);
            Assert.That(result, Is.EqualTo(0));
        }
    }
}
