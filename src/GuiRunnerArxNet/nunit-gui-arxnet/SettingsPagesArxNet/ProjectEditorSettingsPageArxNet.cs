// ****************************************************************
// Copyright 2012, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.5.30修改：
//      1.在nunit2.6.2基础上修改
//      2.NUnit.Gui.SettingsPages.ProjectEditorSettingsPage改为NUnit.Gui.ArxNet.SettingsPagesArxNet.ProjectEditorSettingsPageArxNet类
//      3.改SettingsPage为SettingsPageArxNet
//  2014.10.10：
//      在NUnit2.6.3基础上修改
//  2015.2.6：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using NUnit.UiKit;
using NUnit.UiKit.ArxNet;

namespace NUnit.Gui.ArxNet.SettingsPagesArxNet
{
    public partial class ProjectEditorSettingsPageArxNet : SettingsPageArxNet
    {
        private static readonly string EDITOR_PATH_SETTING = "Options.ProjectEditor.EditorPath";

        public ProjectEditorSettingsPageArxNet(string key) : base(key)
        {
            InitializeComponent();
        }

        public override void LoadSettings()
        {
            string editorPath = (string)settings.GetSetting(EDITOR_PATH_SETTING);

            if (editorPath != null)
            {
                useOtherEditorRadioButton.Checked = true;
                editorPathTextBox.Text = editorPath;
            }
            else
            {
                useNUnitEditorRadioButton.Checked = true;
                editorPathTextBox.Text = "";
            }
        }

        public override void ApplySettings()
        {
            if (useNUnitEditorRadioButton.Checked)
                settings.RemoveSetting(EDITOR_PATH_SETTING);
            else
                settings.SaveSetting(EDITOR_PATH_SETTING, editorPathTextBox.Text);
        }

        private void editorPathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (editorPathTextBox.TextLength == 0)
                useNUnitEditorRadioButton.Checked = true;
            else
                useOtherEditorRadioButton.Checked = true;
        }

        private void editorPathBrowseButton_Click(object sender, EventArgs e)
        {
			OpenFileDialog dlg = new OpenFileDialog();
			if ( Site != null ) dlg.Site = Site;
			dlg.Title = "Select Project Editor";

			dlg.Filter = "Executable Files (*.exe)|*.exe";
			dlg.FilterIndex = 1;
			dlg.FileName = "";

			if ( dlg.ShowDialog( this ) == DialogResult.OK ) 
				editorPathTextBox.Text = dlg.FileName;
        }
    }
}
