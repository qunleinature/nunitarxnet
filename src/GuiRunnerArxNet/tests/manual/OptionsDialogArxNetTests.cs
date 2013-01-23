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
    public class OptionsDialogArxNetTests
    {
        //public static void Display( Form owner )
        [Test]
        public void Display()
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
            GuiOptionsArxNet guiOptions = new GuiOptionsArxNet(new string[0]);
            NUnitFormArxNet owner = new NUnitFormArxNet(guiOptions);
            c.Add(owner);
            OptionsDialogArxNet.Display(owner);
        }
    }
}
