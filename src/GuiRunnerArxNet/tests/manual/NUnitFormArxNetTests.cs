using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;

using NUnit.Framework;
using NUnit.Util;
using NUnit.Util.ArxNet;
using NUnit.UiKit;
using NUnit.UiKit.ArxNet;

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

        [Test]
        public void ShowModalDialog()
        {
            SettingsServiceArxNet settingsService = new SettingsServiceArxNet();
            ServiceManager.Services.AddService(settingsService);
            ServiceManager.Services.AddService(new DomainManager());
            ServiceManager.Services.AddService(new RecentFilesService());
            ServiceManager.Services.AddService(new ProjectService());
            ServiceManager.Services.AddService(new TestLoaderArxNet(new GuiTestEventDispatcherArxNet()));
            ServiceManager.Services.AddService(new AddinRegistry());
            ServiceManager.Services.AddService(new AddinManager());
            ServiceManager.Services.AddService(new TestAgency());
            ServiceManager.Services.InitializeServices();
            AppContainer c = new AppContainer();
            AmbientProperties ambient = new AmbientProperties();
            c.Services.AddService(typeof(AmbientProperties), ambient);            
            GuiOptions guiOptions = new GuiOptions(new string[0]);            
            nUnitFormArxNet = new NUnitFormArxNet(guiOptions);
            c.Add(nUnitFormArxNet);
            nUnitFormArxNet.ShowDialog();
        }
    }
}
