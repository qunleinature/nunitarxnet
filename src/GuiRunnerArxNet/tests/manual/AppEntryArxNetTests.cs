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
            AppEntryArxNet appEntryArxNet = new AppEntryArxNet();
            appEntryArxNet.Main(args);
        }
    }
}
