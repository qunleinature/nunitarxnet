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
            GuiOptionsArxNet expectedGuiOptions = new GuiOptionsArxNet(new string[0]);
            nUnitFormArxNet = new NUnitFormArxNet(expectedGuiOptions);
            object[] args = new object[] { true };
            UnitTestHelper.CallNonPublicMethod(nUnitFormArxNet, "Dispose", args);
        }

        [Test]
        public void ShowModalDialog()
        {
            SettingsServiceArxNet settingsService = new SettingsServiceArxNet();
            ServiceManagerArxNet.Services.AddService(settingsService);
            ServiceManagerArxNet.Services.AddService(new DomainManagerArxNet());
            ServiceManagerArxNet.Services.AddService(new RecentFilesService());
            ServiceManagerArxNet.Services.AddService(new ProjectService());
            ServiceManagerArxNet.Services.AddService(new TestLoaderArxNet(new GuiTestEventDispatcherArxNet()));
            ServiceManagerArxNet.Services.AddService(new AddinRegistry());
            ServiceManagerArxNet.Services.AddService(new AddinManager());
            ServiceManagerArxNet.Services.AddService(new TestAgency());
            ServiceManagerArxNet.Services.InitializeServices();
            AppContainer c = new AppContainer();
            AmbientProperties ambient = new AmbientProperties();
            c.Services.AddService(typeof(AmbientProperties), ambient);            
            GuiOptionsArxNet guiOptions = new GuiOptionsArxNet(new string[0]);            
            nUnitFormArxNet = new NUnitFormArxNet(guiOptions);
            c.Add(nUnitFormArxNet);
            nUnitFormArxNet.ShowDialog();
        }

        [Test]
        public void ShowModelessDialog()
        {
            SettingsServiceArxNet settingsService = new SettingsServiceArxNet();
            ServiceManagerArxNet.Services.AddService(settingsService);
            ServiceManagerArxNet.Services.AddService(new DomainManagerArxNet());
            ServiceManagerArxNet.Services.AddService(new RecentFilesService());
            ServiceManagerArxNet.Services.AddService(new ProjectService());
            ServiceManagerArxNet.Services.AddService(new TestLoaderArxNet(new GuiTestEventDispatcherArxNet()));
            ServiceManagerArxNet.Services.AddService(new AddinRegistry());
            ServiceManagerArxNet.Services.AddService(new AddinManager());
            ServiceManagerArxNet.Services.AddService(new TestAgency());
            ServiceManagerArxNet.Services.InitializeServices();
            AppContainer c = new AppContainer();
            AmbientProperties ambient = new AmbientProperties();
            c.Services.AddService(typeof(AmbientProperties), ambient);
            GuiOptionsArxNet guiOptions = new GuiOptionsArxNet(new string[0]);
            nUnitFormArxNet = new NUnitFormArxNet(guiOptions);
            c.Add(nUnitFormArxNet);
            nUnitFormArxNet.Show();
        }
    }
}
