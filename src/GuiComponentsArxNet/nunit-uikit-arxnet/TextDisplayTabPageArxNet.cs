// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.5.31修改：
//      1.在nunit2.6.2基础上修改
//      2.NUnit.UiKit.TextDisplayTabPage改为NUnit.UiKit.ArxNet.TextDisplayTabPageArxNet类
//      3.改TextBoxDisplay为TextBoxDisplayArxNet
//  2013.6.1修改：
//      1.改TextDisplayTabSettings为TextDisplayTabSettingsArxNet
//  2015.2.9：
//      在NUnit2.6.4基础上修改
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
