using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NUnit.Util;
using NUnit.Util.ArxNet;

using Com.Utility.UnitTest;

namespace NUnit.Gui.ArxNet.Tests
{
    public class NUnitFormArxNetTests
    {
        NUnitFormArxNet nUnitFormArxNet = null;

        //Construction
		//public NUnitFormArxNet( GuiOptions guiOptions ) : base("NUnit")
        [Test]
        public void Constructor()
        {
            GuiOptions expectedGuiOptions = new GuiOptions(new string[0]);
            nUnitFormArxNet = new NUnitFormArxNet(expectedGuiOptions);
            Assert.That(nUnitFormArxNet, Is.Not.Null);
            //private GuiOptions guiOptions;
            GuiOptions actualGuiOptions = UnitTestHelper.GetNonPublicField(nUnitFormArxNet, "guiOptions") as GuiOptions;
            Assert.That(actualGuiOptions, Is.EqualTo(expectedGuiOptions));
            //private RecentFiles recentFilesService;
            RecentFiles expectedRecentFilesService = ServicesArxNet.RecentFiles;
            RecentFiles actualRecentFilesService = UnitTestHelper.GetNonPublicField(nUnitFormArxNet, "recentFilesService") as RecentFiles;
            Assert.That(actualRecentFilesService, Is.EqualTo(expectedRecentFilesService));
            //private ISettings userSettings;
            ISettings expectedUserSettings = ServicesArxNet.UserSettings;
            ISettings actualUserSettings = UnitTestHelper.GetNonPublicField(nUnitFormArxNet, "userSettings") as ISettings;
            Assert.That(actualUserSettings, Is.EqualTo(expectedUserSettings));
            //private NUnitPresenterArxNet presenter = null;
            NUnitPresenterArxNet actualPresenter = UnitTestHelper.GetNonPublicField(nUnitFormArxNet, "presenter") as NUnitPresenterArxNet;
            Assert.That(actualPresenter.Form, Is.EqualTo(nUnitFormArxNet));
            TestLoaderArxNet actualLoader = UnitTestHelper.GetNonPublicField(actualPresenter, "loader") as TestLoaderArxNet;
            //private TestLoaderArxNet TestLoader
            TestLoaderArxNet expectedLoader = UnitTestHelper.GetNonPublicProperty(nUnitFormArxNet, "TestLoader") as TestLoaderArxNet;
            Assert.That(actualLoader, Is.EqualTo(expectedLoader));
        }
        
    }
}
