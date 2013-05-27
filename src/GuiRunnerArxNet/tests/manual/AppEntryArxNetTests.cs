// ****************************************************************
//2012年12月24日，雷群修改:
//  1.添加测试项目：nunit-gui-arxnet.tests.manua（需人工交互）
//  2.测试类：AppEntryArxNetTests
// ****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Gui.ArxNet;
using NUnit.UiKit;

using Com.Utility.UnitTest;

namespace NUnit.Gui.ArxNet.Tests
{
    [TestFixture]
    public class AppEntryArxNetTests
    {
        //public int Main(string[] args)
        [Test]
        public void Main()
        {
            string[] args = new string[0];
            AppEntryArxNet.Init();
            int result = AppEntryArxNet.Main(args);
            //AppEntryArxNet.CleanUp();
            Assert.That(result, Is.EqualTo(0));
        }

        //private static IMessageDisplay MessageDisplay
        [Test]
        public void MessageDisplay()
        {
            IMessageDisplay messageDisplay = UnitTestHelper.GetNonPublicStaticProperty(typeof(AppEntryArxNet), "MessageDisplay") as IMessageDisplay;
            Assert.That(messageDisplay, Is.Not.Null);
            messageDisplay.Display("NUnit.Gui.ArxNet.Tests.MessageDisplay");
        }
    }
}
