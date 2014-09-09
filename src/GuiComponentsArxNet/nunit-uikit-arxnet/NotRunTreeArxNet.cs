// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// 2013.5.28修改：
//  1.在nunit2.6.2基础上修改
//  2.NUnit.UiKit.NotRunTree改为NUnit.UiKit.ArxNet.NotRunTreeArxNet类
//  3.Services改为ServicesArxNet
// ****************************************************************

using System;
using System.Windows.Forms;
using NUnit.Core;
using NUnit.Util;

using NUnit.Util.ArxNet;

namespace NUnit.UiKit.ArxNet
{
	/// <summary>
	/// Summary description for NotRunTree.
	/// </summary>
	public class NotRunTreeArxNet : TreeView, TestObserver
	{
		#region TestObserver Members and TestEventHandlers

		public void Subscribe(ITestEvents events)
		{
			events.TestLoaded += new TestEventHandler(ClearTreeNodes);
			events.TestUnloaded += new TestEventHandler(ClearTreeNodes);
			events.TestReloaded += new TestEventHandler(OnTestReloaded);
			events.RunStarting += new TestEventHandler(ClearTreeNodes);
			events.TestFinished += new TestEventHandler(OnTestFinished);
			events.SuiteFinished += new TestEventHandler(OnTestFinished);
		}

		private void OnTestFinished( object sender, TestEventArgs args )
		{
			TestResult result = args.Result;
			if ( result.ResultState == ResultState.Skipped || result.ResultState == ResultState.Ignored)
				this.AddNode( args.Result );
		}

		private void ClearTreeNodes(object sender, TestEventArgs args)
		{
			this.Nodes.Clear();
		}

		private void OnTestReloaded(object sender, TestEventArgs args)
		{
			if ( ServicesArxNet.UserSettings.GetSetting( "Options.TestLoader.ClearResultsOnReload", false ) )
				this.Nodes.Clear();
		}

		private void AddNode( TestResult result )
		{
			TreeNode node = new TreeNode(result.Name);
			TreeNode reasonNode = new TreeNode("Reason: " + result.Message);
			node.Nodes.Add(reasonNode);

			Nodes.Add( node );
		}
		#endregion
	}
}
