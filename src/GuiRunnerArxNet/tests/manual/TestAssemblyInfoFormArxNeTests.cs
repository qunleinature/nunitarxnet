using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NUnit.Framework;
using NUnit.Util;
using NUnit.Util.ArxNet;
using NUnit.UiKit;
using NUnit.UiKit.ArxNet;

namespace NUnit.Gui.ArxNet.Tests
{
    [TestFixture]
    public class TestAssemblyInfoFormArxNeTests
    {
        [Test]
        public void ShowDialog()
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
            NUnitFormArxNet owner = new NUnitFormArxNet(guiOptions);
            c.Add(owner);
            owner.Show();
            new TestAssemblyInfoFormArxNet().ShowDialog();
        }        
    }
}
