// ****************************************************************
// Copyright 2010, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.5.30修改：
//  1.在nunit2.6.2基础上修改
//  2.NUnit.Gui.SettingsPages.RuntimeSelectionSettingsPage改为NUnit.Gui.ArxNet.SettingsPagesArxNet.RuntimeSelectionSettingsPageArxNet类
//  3.改SettingsPage为SettingsPageArxNet
// ****************************************************************

using System;
using NUnit.Core;

namespace NUnit.Gui.ArxNet.SettingsPagesArxNet
{
    public partial class RuntimeSelectionSettingsPageArxNet : NUnit.UiKit.ArxNet.SettingsPageArxNet
    {
        private static readonly string RUNTIME_SELECTION_ENABLED =
            "Options.TestLoader.RuntimeSelectionEnabled";

        public RuntimeSelectionSettingsPageArxNet(string key) : base(key)
        {
            InitializeComponent();
        }

        public override void LoadSettings()
        {
            runtimeSelectionCheckBox.Checked = settings.GetSetting(RUNTIME_SELECTION_ENABLED, true);
        }

        public override void ApplySettings()
        {
            settings.SaveSetting(RUNTIME_SELECTION_ENABLED, runtimeSelectionCheckBox.Checked);
        }
    }
}
