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
//   4.改ProjectEditorSettingsPage为ProjectEditorSettingsPageArxNet
//   5.改TestLoaderSettingsPage为TestLoaderSettingsPageArxNet
//   6.改AssemblyReloadSettingsPage为AssemblyReloadSettingsPageArxNet
//   7.改RuntimeSelectionSettingsPage为RuntimeSelectionSettingsPageArxNet
//   8.改AdvancedLoaderSettingsPage为AdvancedLoaderSettingsPageArxNet
//   9.改VisualStudioSettingsPage为VisualStudioSettingsPageArxNet
//   10.改InternalTraceSettingsPage为InternalTraceSettingsPageArxNet
// 2013.6.7
//   1.改在nunit2.6.2基础
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
                new ProjectEditorSettingsPageArxNet("Gui.Project Editor"),
                new TestLoaderSettingsPageArxNet("Test Loader.Assembly Isolation"),
				new AssemblyReloadSettingsPageArxNet("Test Loader.Assembly Reload"),
                new RuntimeSelectionSettingsPageArxNet("Test Loader.Runtime Selection"),
				new AdvancedLoaderSettingsPageArxNet("Test Loader.Advanced"),
				new VisualStudioSettingsPageArxNet("IDE Support.Visual Studio"),
                new InternalTraceSettingsPageArxNet("Advanced Settings.Internal Trace"));
		}
#else
		public static void Display( Form owner )
		{
			TabbedSettingsDialogArxNet.Display( owner,
				new GuiSettingsPageArxNet("General"),
				new TreeSettingsPageArxNet("Tree"),
				new TestResultSettingsPageArxNet("Results"),
				new TextOutputSettingsPageArxNet("Text Output"),
				new TestLoaderSettingsPageArxNet("Test Load"),
				new AssemblyReloadSettingsPageArxNet("Reload"),
				new VisualStudioSettingsPageArxNet("Visual Studio"));
		}
#endif

        private OptionsDialogArxNet() { }
	}
}
