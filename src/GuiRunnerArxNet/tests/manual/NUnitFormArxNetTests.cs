using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

using NUnit.Framework;
using NUnit.Util;
using NUnit.Util.ArxNet;

using Com.Utility.UnitTest;

namespace NUnit.Gui.ArxNet.Tests
{
    public class NUnitFormArxNetTests
    {
        NUnitFormArxNet nUnitFormArxNet = null;

        //protected override void Dispose( bool disposing )
        [Test]
        public void Dispose()
        {
            GuiOptions expectedGuiOptions = new GuiOptions(new string[0]);
            nUnitFormArxNet = new NUnitFormArxNet(expectedGuiOptions);
            object[] args = new object[] { true };
            UnitTestHelper.CallNonPublicMethod(nUnitFormArxNet, "Dispose", args);
        }
    }
}
