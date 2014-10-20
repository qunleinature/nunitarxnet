// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun 
// 2012.12.21修改:改Services为ServicesArxNet
// 2013.1.8修改：
//  1.NUnit.Gui.ArxNet.Tests.NUnitFormArxNetTests.ShowModalDialog测试改
// 2013.5.28修改：
//  1.在nunit2.6.2基础上修改
//  2.ErrorDisplay改为ErrorDisplayArxNet
//  3.NotRunTree改为NotRunTreeArxNet
// 2013.5.31修改：
//  1.改TextDisplayTabPage为TextDisplayTabPageArxNet
// 2013.6.1修改：
//  1.改TextOutputSettingsPage为TextOutputSettingsPageArxNet
//  2.改TextDisplayTabSettings为TextDisplayTabSettingsArxNet
// 2013.6.9
//  1.已改SimpleSettingsDialogArxNet
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
//  2014.10.20：
//      在NUnit2.6.3基础上修改
// ****************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using NUnit.Util;
using NUnit.Core;
using NUnit.UiKit;
using NUnit.Util.ArxNet;
using NUnit.Gui.ArxNet;

using CP.Windows.Forms;
using System.Diagnostics;

namespace NUnit.UiKit.ArxNet
{
	/// <summary>
	/// Summary description for ResultTabs.
	/// </summary>
	public class ResultTabsArxNet : System.Windows.Forms.UserControl, TestObserver
	{
		static Logger log = InternalTrace.GetLogger(typeof(ResultTabsArxNet));

		private ISettings settings;
		private bool updating = false;
		private TextDisplayController displayController;

		private MenuItem tabsMenu;
		private MenuItem errorsTabMenuItem;
		private MenuItem notRunTabMenuItem;
		private MenuItem menuSeparator;
		private MenuItem textOutputMenuItem;

		private System.Windows.Forms.TabPage errorTab;
		private NUnit.UiKit.ArxNet.ErrorDisplayArxNet errorDisplay;
		private System.Windows.Forms.TabPage notRunTab;
		private NUnit.UiKit.ArxNet.NotRunTreeArxNet notRunTree;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.MenuItem copyDetailMenuItem;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ResultTabsArxNet()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.tabsMenu = new MenuItem();
			this.errorsTabMenuItem = new System.Windows.Forms.MenuItem();
			this.notRunTabMenuItem = new System.Windows.Forms.MenuItem();
			this.menuSeparator = new System.Windows.Forms.MenuItem();
			this.textOutputMenuItem = new System.Windows.Forms.MenuItem();

			this.tabsMenu.MergeType = MenuMerge.Add;
			this.tabsMenu.MergeOrder = 1;
			this.tabsMenu.MenuItems.AddRange(
				new System.Windows.Forms.MenuItem[] 
				{
					this.errorsTabMenuItem,
					this.notRunTabMenuItem,
					this.menuSeparator,
					this.textOutputMenuItem,
				} );
			this.tabsMenu.Text = "&Result Tabs";
			this.tabsMenu.Visible = true;

			this.errorsTabMenuItem.Index = 0;
			this.errorsTabMenuItem.Text = "&Errors && Failures";
			this.errorsTabMenuItem.Click += new System.EventHandler(this.errorsTabMenuItem_Click);

			this.notRunTabMenuItem.Index = 1;
			this.notRunTabMenuItem.Text = "Tests &Not Run";
			this.notRunTabMenuItem.Click += new System.EventHandler(this.notRunTabMenuItem_Click);

			this.menuSeparator.Index = 2;
			this.menuSeparator.Text = "-";
			
			this.textOutputMenuItem.Index = 3;
			this.textOutputMenuItem.Text = "Text &Output...";
			this.textOutputMenuItem.Click += new EventHandler(textOutputMenuItem_Click);

			displayController = new TextDisplayController(tabControl);
//			displayController.CreatePages();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl = new System.Windows.Forms.TabControl();
			this.errorTab = new System.Windows.Forms.TabPage();
			this.errorDisplay = new NUnit.UiKit.ArxNet.ErrorDisplayArxNet();
			this.notRunTab = new System.Windows.Forms.TabPage();
			this.notRunTree = new NUnit.UiKit.ArxNet.NotRunTreeArxNet();
			this.copyDetailMenuItem = new System.Windows.Forms.MenuItem();
			this.tabControl.SuspendLayout();
			this.errorTab.SuspendLayout();
			this.notRunTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabControl.Controls.Add(this.errorTab);
			this.tabControl.Controls.Add(this.notRunTab);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(488, 280);
			this.tabControl.TabIndex = 3;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// errorTab
			// 
			this.errorTab.Controls.Add(this.errorDisplay);
			this.errorTab.ForeColor = System.Drawing.SystemColors.ControlText;
			this.errorTab.Location = new System.Drawing.Point(4, 4);
			this.errorTab.Name = "errorTab";
			this.errorTab.Size = new System.Drawing.Size(480, 254);
			this.errorTab.TabIndex = 0;
			this.errorTab.Text = "Errors and Failures";
			// 
			// errorDisplay
			// 
			this.errorDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.errorDisplay.Location = new System.Drawing.Point(0, 0);
			this.errorDisplay.Name = "errorDisplay";
			this.errorDisplay.Size = new System.Drawing.Size(480, 254);
			this.errorDisplay.TabIndex = 0;
			// 
			// notRunTab
			// 
			this.notRunTab.Controls.Add(this.notRunTree);
			this.notRunTab.ForeColor = System.Drawing.SystemColors.ControlText;
			this.notRunTab.Location = new System.Drawing.Point(4, 4);
			this.notRunTab.Name = "notRunTab";
			this.notRunTab.Size = new System.Drawing.Size(480, 254);
			this.notRunTab.TabIndex = 1;
			this.notRunTab.Text = "Tests Not Run";
			this.notRunTab.Visible = false;
			// 
			// notRunTree
			// 
			this.notRunTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.notRunTree.ImageIndex = -1;
			this.notRunTree.Indent = 19;
			this.notRunTree.Location = new System.Drawing.Point(0, 0);
			this.notRunTree.Name = "notRunTree";
			this.notRunTree.SelectedImageIndex = -1;
			this.notRunTree.Size = new System.Drawing.Size(480, 254);
			this.notRunTree.TabIndex = 0;
			// 
			// copyDetailMenuItem
			// 
			this.copyDetailMenuItem.Index = -1;
			this.copyDetailMenuItem.Text = "Copy";
			// 
			// ResultTabs
			// 
			this.Controls.Add(this.tabControl);
			this.Name = "ResultTabs";
			this.Size = new System.Drawing.Size(488, 280);
			this.tabControl.ResumeLayout(false);
			this.errorTab.ResumeLayout(false);
			this.notRunTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

        public bool IsTracingEnabled
        {
            get { return displayController.IsTracingEnabled; }
        }

        public LoggingThreshold MaximumLogLevel
        {
            get { return displayController.MaximumLogLevel; }
        }
	
		public void Clear()
		{
			errorDisplay.Clear();
			notRunTree.Nodes.Clear();
			displayController.Clear();
		}

		public MenuItem TabsMenu
		{
			get { return tabsMenu; }
		}

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                try//2013-1-10:NUnit.Gui.ArxNet.Tests.NUnitFormArxNetTests.ShowModalDialog测试加
                {
                    if (ServicesArxNet.TestLoader == null) return;//2013-1-10:NUnit.Gui.ArxNet.Tests.NUnitFormArxNetTests.ShowModalDialog测试加

                    this.settings = ServicesArxNet.UserSettings;
                    TextDisplayTabSettingsArxNet tabSettings = new TextDisplayTabSettingsArxNet();
                    tabSettings.LoadSettings(settings);

                    UpdateTabPages();

                    Subscribe(ServicesArxNet.TestLoader.Events);
                    ServicesArxNet.UserSettings.Changed += new SettingsEventHandler(UserSettings_Changed);

                    ITestEvents events = ServicesArxNet.TestLoader.Events;
                    errorDisplay.Subscribe(events);
                    notRunTree.Subscribe(events);


                    base.OnLoad(e);
                }
                /*2013-1-10:NUnit.Gui.ArxNet.Tests.NUnitFormArxNetTests.ShowModalDialog测试加*/
                catch (SystemException exception)
                {
                    NUnitFormArxNet form = this.ParentForm as NUnitFormArxNet;
                    form.MessageDisplay.Error("ResultTabsArxNet unable to Load", exception);
                }
                /*2013-1-10:NUnit.Gui.ArxNet.Tests.NUnitFormArxNetTests.ShowModalDialog测试加*/
            }
        }

		private void UpdateTabPages()
		{
            if (settings == null) return;//2013-1-11:NUnit.Gui.ArxNet.Tests.NUnitFormArxNetTests.ShowModalDialog测试加

			errorsTabMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayErrorsTab", true );
			notRunTabMenuItem.Checked = settings.GetSetting( "Gui.ResultTabs.DisplayNotRunTab", true );

			log.Debug( "Updating tab pages" );
			updating = true;
			
			tabControl.TabPages.Clear();

			if ( errorsTabMenuItem.Checked )
				tabControl.TabPages.Add( errorTab );
			if ( notRunTabMenuItem.Checked )
				tabControl.TabPages.Add( notRunTab );
			
			displayController.UpdatePages();

			tabControl.SelectedIndex = settings.GetSetting( "Gui.ResultTabs.SelectedTab", 0 );

			updating = false;
		}

		private void UserSettings_Changed( object sender, SettingsEventArgs e )
		{
			if( e.SettingName.StartsWith( "Gui.ResultTabs.Display" ) ||
				e.SettingName == "Gui.TextOutput.TabList" || 
				e.SettingName.StartsWith( "Gui.TextOut" ) && e.SettingName.EndsWith( "Enabled" ) )
					UpdateTabPages();
		}

		private void errorsTabMenuItem_Click(object sender, System.EventArgs e)
		{
            if (settings == null) return;//2013-1-11:NUnit.Gui.ArxNet.Tests.NUnitFormArxNetTests.ShowModalDialog测试加

			settings.SaveSetting( "Gui.ResultTabs.DisplayErrorsTab", errorsTabMenuItem.Checked = !errorsTabMenuItem.Checked );
		}

		private void notRunTabMenuItem_Click(object sender, System.EventArgs e)
		{
            if (settings == null) return;//2013-1-11:NUnit.Gui.ArxNet.Tests.NUnitFormArxNetTests.ShowModalDialog测试加

			settings.SaveSetting( "Gui.ResultTabs.DisplayNotRunTab", notRunTabMenuItem.Checked = !notRunTabMenuItem.Checked );
		}

		private void textOutputMenuItem_Click(object sender, System.EventArgs e)
		{
			SimpleSettingsDialogArxNet.Display( this.FindForm(), new TextOutputSettingsPageArxNet("Text Output") );
		}

		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (settings == null) return;//2013-1-11:NUnit.Gui.ArxNet.Tests.NUnitFormArxNetTests.ShowModalDialog测试加

			if ( !updating )
			{
				int index = tabControl.SelectedIndex;
				if ( index >=0 && index < tabControl.TabCount )
					settings.SaveSetting( "Gui.ResultTabs.SelectedTab", index );
			}
		}

		#region TestObserver Members
		public void Subscribe(ITestEvents events)
		{
			events.TestLoaded += new TestEventHandler(OnTestLoaded);
			events.TestUnloaded += new TestEventHandler(OnTestUnloaded);
			events.TestReloaded += new TestEventHandler(OnTestReloaded);
			events.RunStarting += new TestEventHandler(OnRunStarting);
		}

		private void OnRunStarting(object sender, TestEventArgs args)
		{
			this.Clear();
		}

		private void OnTestLoaded(object sender, TestEventArgs args)
		{
			this.Clear();
		}

		private void OnTestUnloaded(object sender, TestEventArgs args)
		{
			this.Clear();
		}
		private void OnTestReloaded(object sender, TestEventArgs args)
		{
            if (settings == null) return;//2013-1-11:NUnit.Gui.ArxNet.Tests.NUnitFormArxNetTests.ShowModalDialog测试加

			if ( settings.GetSetting( "Options.TestLoader.ClearResultsOnReload", false ) )
				this.Clear();
		}
		#endregion

		private void tabControl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			bool selected = e.Index == tabControl.SelectedIndex;

			Font font = selected ? new Font( e.Font, FontStyle.Bold ) : e.Font;
			Brush backBrush = new SolidBrush( selected ? SystemColors.Control : SystemColors.Window );
			Brush foreBrush = new SolidBrush( SystemColors.ControlText );

			e.Graphics.FillRectangle( backBrush, e.Bounds );
			Rectangle r = e.Bounds;
			r.Y += 3; r.Height -= 3;
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			e.Graphics.DrawString( tabControl.TabPages[e.Index].Text, font, foreBrush, r, sf );

			foreBrush.Dispose();
			backBrush.Dispose();
			if ( selected )
				font.Dispose();
		}

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            tabControl.ItemSize = new Size(tabControl.ItemSize.Width, this.Font.Height + 7);
        }

		private class TextDisplayController
		{
			private TabControl tabControl;
			List<TextDisplayTabPageArxNet> tabPages = new List<TextDisplayTabPageArxNet>();

			public TextDisplayController(TabControl tabControl)
			{			
				this.tabControl = tabControl;
				ServicesArxNet.UserSettings.Changed += new SettingsEventHandler(UserSettings_Changed);
			}

            public bool IsTracingEnabled
            {
                get
                {
                    foreach (TextDisplayTabPageArxNet page in tabPages)
                        if (page.Display.Content.Trace)
                            return true;

                    return false;
                }
            }

            public LoggingThreshold MaximumLogLevel
            {
                get
                {
                    LoggingThreshold logLevel = LoggingThreshold.Off;

                    foreach (TextDisplayTabPageArxNet page in tabPages)
                    {
                        LoggingThreshold level = page.Display.Content.LogLevel;
                        if (level > logLevel)
                            logLevel = level;
                    }

                    return logLevel;
                }
            }

            public void Clear()
			{
				foreach( TextDisplayTabPageArxNet page in tabPages )
					page.Display.Clear();
			}

			public void UpdatePages()
			{
				TextDisplayTabSettingsArxNet tabSettings = new TextDisplayTabSettingsArxNet();
				tabSettings.LoadSettings();
                List <TextDisplayTabPageArxNet> oldPages = tabPages;
				tabPages = new List<TextDisplayTabPageArxNet>();
				Font displayFont = GetFixedFont();

				foreach( TextDisplayTabSettingsArxNet.TabInfo tabInfo in tabSettings.Tabs )
				{
					if ( tabInfo.Enabled )
					{
						TextDisplayTabPageArxNet thePage = null;
						foreach( TextDisplayTabPageArxNet page in oldPages )
							if ( page.Name == tabInfo.Name )
							{
								thePage = page;
								break;
							}

						if ( thePage == null )
						{
							thePage = new TextDisplayTabPageArxNet( tabInfo );
							thePage.Display.Subscribe(ServicesArxNet.TestLoader.Events);
						}

						thePage.DisplayFont = displayFont;

						tabPages.Add( thePage );
						tabControl.TabPages.Add( thePage );
					}
				}
			}

			private void UserSettings_Changed(object sender, SettingsEventArgs args)
			{
				string settingName = args.SettingName; 
				string prefix = "Gui.TextOutput.";

				if ( settingName == "Gui.FixedFont" )
				{
					Font displayFont = GetFixedFont();
					foreach( TextDisplayTabPageArxNet page in tabPages )
						page.DisplayFont = displayFont;
				}
				else
				if ( settingName.StartsWith( prefix ) )
				{
					string fieldName = settingName.Substring( prefix.Length );
					int dot = fieldName.IndexOf('.');
					if ( dot > 0 )
					{
						string tabName = fieldName.Substring( 0, dot );
						string propName = fieldName.Substring( dot + 1 );
						foreach( TextDisplayTabPageArxNet page in tabPages )
							if ( page.Name == tabName )
							{
								switch(propName)
								{
									case "Title":
										page.Text = (string)ServicesArxNet.UserSettings.GetSetting( settingName );
										break;
                                    case "Content":
                                        page.Display.Content.LoadSettings(tabName);
                                        break;
                                }
							}
					}
				}
			}

			private static Font GetFixedFont()
			{
				ISettings settings = ServicesArxNet.UserSettings;               

				return settings == null 
                    ? new Font(FontFamily.GenericMonospace, 8.0f) 
                    : settings.GetSetting("Gui.FixedFont", new Font(FontFamily.GenericMonospace, 8.0f));
			}
		}
	}
}
