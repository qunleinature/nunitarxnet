// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2012年8月25日，雷群修改
//  2014.10.3：
//      在NUnit2.6.3基础上修改
//  2015.2.9：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.TestUtilities;

namespace NUnit.Gui.ArxNet.Tests
{
	[TestFixture]	
	public class ExceptionDetailsFormTests : FormTester
	{
		[TestFixtureSetUp]
		public void CreateForm()
		{
			this.Form = new ExceptionDetailsForm( new Exception( "My message" ) );
		}

		[TestFixtureTearDown]
		public void CloseForm()
		{
			this.Form.Close();
		}

		[Test]
		public void ControlsExist()
		{
			AssertControlExists( "message", typeof( Label ) );
			AssertControlExists( "stackTrace", typeof( RichTextBox ) );
			AssertControlExists( "okButton", typeof( Button ) );
		}

		[Test]
		public void ControlsArePositionedCorrectly()
		{
			AssertControlsAreStackedVertically( "message", "stackTrace", "okButton" );
		}

		[Test]
		public void MessageDisplaysCorrectly()
		{
			this.Form.Show();
			Assert.AreEqual( "System.Exception: My message", GetText( "message" ) );
		}
	}
}
