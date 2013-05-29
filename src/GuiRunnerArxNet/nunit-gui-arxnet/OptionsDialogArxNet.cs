// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2012, Lei Qun
// 2012.12.21修改:调用TabbedSettingsDialogArxNet类,TreeBasedSettingsDialogArxNet类
// 2013.5.29修改
//   1.改GuiSettingsPage为GuiSettingsPageArxNet
// 2013.5.30修改
//   1.改TextOutputSettingsPage为TextOutputSettingsPageArxNet
//   2.改TreeSettingsPage为TreeSettingsPageArxNet
//   3.改TestResultSettingsPage为TestResultSettingsPageArxNet
// ****************************************************************

#define TREE_BASED
using System;
using System.Windows.Forms;

using NUnit.Core;
using NUnit.Util;
using NUnit.UiKit;
using NUnit.Gui;
using NUnit.Gui.SettingsPages;

using NUnit.Gui.ArxNet.SettingsPagesArxNet;
using NUnit.Util.ArxNet;
using NUnit.UiKit.ArxNet;

namespace NUnit.Gui.ArxNet
{
	/// <summary>
	/// Summary description for OptionsDialog.
	/// </summary>
	public class OptionsDialogArxNet
	{
#if TREE_BASED
		public static void Display( Form owner )
		{
			TreeBasedSettingsDialogArxNet.Display( owner,
				new GuiSettingsPageArxNet("Gui.General"),
				new TreeSettingsPageArxNet("Gui.Tree Display"),
				new TestResultSettingsPageArxNet("Gui.Test Results"),
				new TextOutputSettingsPageArxNet("Gui.Text Output"),
                new ProjectEditorSettingsPage("Gui.Project Editor"),
                new TestLoaderSettingsPage("Test Loader.Assembly Isolation"),
				new AssemblyReloadSettingsPage("Test Loader.Assembly Reload"),
                new RuntimeSelectionSettingsPage("Test Loader.Runtime Selection"),
				new AdvancedLoaderSettingsPage("Test Loader.Advanced"),
				new VisualStudioSettingsPage("IDE Support.Visual Studio"),
                new InternalTraceSettingsPage("Advanced Settings.Internal Trace"));
		}
#else
		public static void Display( Form owner )
		{
			TabbedSettingsDialogArxNet.Display( owner,
				new GuiSettingsPageArxNet("General"),
				new TreeSettingsPageArxNet("Tree"),
				new TestResultSettingsPageArxNet("Results"),
				new TextOutputSettingsPageArxNet("Text Output"),
				new TestLoaderSettingsPage("Test Load"),
				new AssemblyReloadSettingsPage("Reload"),
				new VisualStudioSettingsPage("Visual Studio"));
		}
#endif

        private OptionsDialogArxNet() { }
	}
}
