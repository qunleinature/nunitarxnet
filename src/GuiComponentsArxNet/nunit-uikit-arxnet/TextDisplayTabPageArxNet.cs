// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// 2013.5.31�޸ģ�
//  1.��nunit2.6.2�������޸�
//  2.NUnit.UiKit.TextDisplayTabPage��ΪNUnit.UiKit.ArxNet.TextDisplayTabPageArxNet��
//  3.��TextBoxDisplayΪTextBoxDisplayArxNet
// 2013.6.1�޸ģ�
//  1.��TextDisplayTabSettingsΪTextDisplayTabSettingsArxNet
//  2.��TextDisplayΪTextDisplayArxNet
// ****************************************************************

using System;
using System.IO;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Util;

namespace NUnit.UiKit.ArxNet
{
	/// <summary>
	/// Summary description for TextDisplayTabPage.
	/// </summary>
	public class TextDisplayTabPageArxNet : TabPage
	{
		private TextBoxDisplayArxNet display;

		public TextDisplayTabPageArxNet()
		{
			this.display = new TextBoxDisplayArxNet();
			this.display.Dock = DockStyle.Fill;		
			this.Controls.Add( display );
		}

		public System.Drawing.Font DisplayFont
		{
			get { return display.Font; }
			set { display.Font = value; }
		}

		public TextDisplayTabPageArxNet( TextDisplayTabSettingsArxNet.TabInfo tabInfo ) : this()
		{
			this.Name = tabInfo.Name;
			this.Text = tabInfo.Title;
			this.Display.Content = tabInfo.Content;
		}

		public TextDisplayArxNet Display
		{
			get { return this.display; }
		}
	}
}