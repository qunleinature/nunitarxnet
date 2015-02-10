// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun 
//  2013.5.31修改
//      已在NUnit2.6.2基础
//  2015.2.9：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using System.IO;
using NUnit.Framework;

namespace NUnit.UiKit.ArxNet.Tests
{
	/// <summary>
	/// Summary description for VisualStateTests.
	/// </summary>
	[TestFixture]
	public class VisualStateArxNetTests
	{
		[Test]
		public void SaveAndRestoreVisualState()
		{
			VisualStateArxNet state = new VisualStateArxNet();
			state.ShowCheckBoxes = true;
			state.TopNode = "ABC.Test.dll";
			state.SelectedNode = "NUnit.Tests.MyFixture.MyTest";
			state.SelectedCategories = "A,B,C";
			state.ExcludeCategories = true;

			StringWriter writer = new StringWriter();
			state.Save( writer );

			string output = writer.GetStringBuilder().ToString();

			StringReader reader = new StringReader( output );
			VisualStateArxNet newState = VisualStateArxNet.LoadFrom( reader );

			Assert.AreEqual( state.ShowCheckBoxes, newState.ShowCheckBoxes, "ShowCheckBoxes" );
			Assert.AreEqual( state.TopNode, newState.TopNode, "TopNode" );
			Assert.AreEqual( state.SelectedNode, newState.SelectedNode, "SelectedNode" );
			Assert.AreEqual( state.SelectedCategories, newState.SelectedCategories, "SelectedCategories" );
			Assert.AreEqual( state.ExcludeCategories, newState.ExcludeCategories, "ExcludeCategories" );
		}
	}
}
