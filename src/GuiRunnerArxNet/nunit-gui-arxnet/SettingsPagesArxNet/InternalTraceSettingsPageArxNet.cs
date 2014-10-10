// ****************************************************************
// Copyright 2010, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.5.30修改：
//  1.在nunit2.6.2基础上修改
//  2.NUnit.Gui.SettingsPages.InternalTraceSettingsPage改为NUnit.Gui.ArxNet.SettingsPagesArxNet.InternalTraceSettingsPageArxNet类
//  3.改SettingsPage为SettingsPageArxNet
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
//  2014.10.10：
//      在NUnit2.6.3基础上修改
// ****************************************************************

using System;
using System.IO;
using NUnit.Core;

namespace NUnit.Gui.ArxNet.SettingsPagesArxNet
{
    public partial class InternalTraceSettingsPageArxNet : NUnit.UiKit.ArxNet.SettingsPageArxNet
    {
        public InternalTraceSettingsPageArxNet(string key) : base(key)
        {
            InitializeComponent();
        }

        public override void LoadSettings()
        {
            traceLevelComboBox.SelectedIndex = (int)(InternalTraceLevel)settings.GetSetting("Options.InternalTraceLevel", InternalTraceLevel.Default);
            logDirectoryLabel.Text = NUnitConfiguration.LogDirectory;
        }

        public override void ApplySettings()
        {
            InternalTraceLevel level = (InternalTraceLevel)traceLevelComboBox.SelectedIndex;
            settings.SaveSetting("Options.InternalTraceLevel", level);
            InternalTrace.Level = level;
        }
    }
}
