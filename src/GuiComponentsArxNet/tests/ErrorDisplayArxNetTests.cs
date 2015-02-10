// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.5.28修改：
//      1.在nunit2.6.2基础上修改
//  2013.6.8
//      1.改ErrorDisplayArxNet
//  2013.6.8
//      1.改ErrorDisplayArxNet
//  2015.2.9：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.TestUtilities;

namespace NUnit.UiKit.ArxNet.Tests
{
	[TestFixture]
	public class ErrorDisplayArxNetTests : ControlTester
	{
		[TestFixtureSetUp]
		public void CreateForm()
		{
			this.Control = new ErrorDisplayArxNet();
		}

		[TestFixtureTearDown]
		public void CloseForm()
		{
			this.Control.Dispose();
		}

		[Test]
		public void ControlsExist()
		{
			AssertControlExists( "detailList", typeof( ListBox ) );
			AssertControlExists( "tabSplitter", typeof( Splitter ) );
			AssertControlExists( "errorBrowser", typeof( NUnit.UiException.Controls.ErrorBrowser ) );
		}

		[Test]
		public void ControlsArePositionedCorrectly()
		{
			AssertControlsAreStackedVertically( "detailList", "tabSplitter", "errorBrowser" );
		}
	}
}
