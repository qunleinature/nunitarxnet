// ****************************************************************
// Copyright 2002-2003, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2012年8月25日，雷群修改
//  2013.6.6
//      1.RecentFileMenuHandler改成RecentFileMenuHandlerArxNet
//      2.已在NUnit2.6.2基础
//  2014.10.3：
//      在NUnit2.6.3基础上修改
//  2015.2.9：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using System.Collections;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Util;

namespace NUnit.Gui.ArxNet.Tests
{
	[TestFixture]
	public class RecentFileMenuHandlerArxNetTests
	{
		private MenuItem menu;
		private RecentFiles files;
		private RecentFileMenuHandlerArxNet handler;
		
		[SetUp]
		public void SetUp()
		{
			menu = new MenuItem();
			files = new FakeRecentFiles();
			handler = new RecentFileMenuHandlerArxNet( menu, files );
            handler.CheckFilesExist = false;
        }

		[Test]
		public void DisableOnLoadWhenEmpty()
		{
			handler.Load();
			Assert.IsFalse( menu.Enabled );
		}

		[Test]
		public void EnableOnLoadWhenNotEmpty()
		{
			files.SetMostRecent( "Test" );
			handler.Load();
			Assert.IsTrue( menu.Enabled );
		}
		[Test]
		public void LoadMenuItems()
		{
			files.SetMostRecent( "Third" );
			files.SetMostRecent( "Second" );
			files.SetMostRecent( "First" );
			handler.Load();
			Assert.AreEqual( 3, menu.MenuItems.Count );
			Assert.AreEqual( "1 First", menu.MenuItems[0].Text );
		}

		private class FakeRecentFiles : RecentFiles
		{
			private RecentFilesCollection files = new RecentFilesCollection();
			private int maxFiles = 24;

			public int Count
			{
				get { return files.Count; }
			}

			public int MaxFiles
			{
				get { return maxFiles; }
				set { maxFiles = value; }
			}

			public void SetMostRecent( string fileName )
			{
				SetMostRecent( new RecentFileEntry( fileName ) );
			}

			public void SetMostRecent( RecentFileEntry entry )
			{
				files.Insert( 0, entry );
			}

			public RecentFilesCollection Entries
			{
				get { return files; }
			}

			public void Clear()
			{
				files.Clear();
			}

			public void Remove( string fileName )
			{
				files.Remove( fileName );
			}
		}
	
		// TODO: Need mock loader to test clicking
	}
}
