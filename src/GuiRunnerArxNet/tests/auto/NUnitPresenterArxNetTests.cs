using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Util.ArxNet;
using NUnit.Gui;
using NUnit.Gui.ArxNet;

using Com.Utility.UnitTest;

namespace NUnit.Gui.ArxNet.Tests
{
    [TestFixture]
    public class NUnitPresenterArxNetTests
    {
        //public NUnitFormArxNet Form        
        //Constructor
        //public NUnitPresenterArxNet(NUnitFormArxNet form, TestLoaderArxNet loader)
        [Test]
        public void ConstructorAndForm()
        {
            NUnitFormArxNet expectedForm = new NUnitFormArxNet(new GuiOptions(new string[0]));
            TestLoaderArxNet expectedLoader = new TestLoaderArxNet();
            NUnitPresenterArxNet nUnitPresenterArxNet = new NUnitPresenterArxNet(expectedForm, expectedLoader);            
            Assert.That(nUnitPresenterArxNet.Form, Is.EqualTo(expectedForm));
            //private TestLoaderArxNet loader = null;
            TestLoaderArxNet actualLoader = UnitTestHelper.GetNonPublicField(nUnitPresenterArxNet, "loader") as TestLoaderArxNet;
            Assert.That(actualLoader, Is.EqualTo(expectedLoader));
        }

        

    }
}
