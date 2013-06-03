// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// 2013.5.30�޸ģ�
//  1.��nunit2.6.2�������޸�
//  2.NUnit.UiKit.TestSuiteTreeView��ΪNUnit.UiKit.ArxNet.TestSuiteTreeViewArxNet��
//  3.��TestSuiteTreeViewΪTestSuiteTreeViewArxNet
//  4.��TestSuiteTreeNodeΪTestSuiteTreeNodeArxNet
// ****************************************************************

using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using NUnit.Core;
using NUnit.Core.Filters;

namespace NUnit.UiKit.ArxNet
{
	/// <summary>
	/// The VisualState class holds the latest visual state for a project.
	/// </summary>
	[Serializable]
	public class VisualStateArxNet
	{
		#region Fields
		[XmlAttribute]
		public bool ShowCheckBoxes;

		public string TopNode;

		public string SelectedNode;

		public string SelectedCategories;

		public bool ExcludeCategories;

		[XmlArrayItem("Node")]
		public List<VisualTreeNode> Nodes;
		#endregion

		#region Static Methods
		public static string GetVisualStateFileName( string testFileName )
		{
			if ( testFileName == null )
				return "VisualState.xml";

			string baseName = testFileName;
			if ( baseName.EndsWith( ".nunit" ) )
				baseName = baseName.Substring( 0, baseName.Length - 6 );
			
			return baseName + ".VisualState.xml";
		}

		public static VisualStateArxNet LoadFrom( string fileName )
		{
			using ( StreamReader reader = new StreamReader( fileName ) )
			{
				return LoadFrom( reader );
			}
		}

		public static VisualStateArxNet LoadFrom( TextReader reader )
		{
			XmlSerializer serializer = new XmlSerializer( typeof( VisualStateArxNet) );
			return (VisualStateArxNet)serializer.Deserialize( reader );
		}
		#endregion

		#region Constructors
		public VisualStateArxNet() { }

		public VisualStateArxNet( TestSuiteTreeViewArxNet treeView )
		{
			this.ShowCheckBoxes = treeView.CheckBoxes;
			this.TopNode = ((TestSuiteTreeNodeArxNet)treeView.TopNode).Test.TestName.UniqueName;
			this.SelectedNode = ((TestSuiteTreeNodeArxNet)treeView.SelectedNode).Test.TestName.UniqueName;
			this.Nodes = new List<VisualTreeNode>();
            ProcessTreeNodes( (TestSuiteTreeNodeArxNet)treeView.Nodes[0] );

			if ( !treeView.CategoryFilter.IsEmpty )
			{
				ITestFilter filter = treeView.CategoryFilter;
				if ( filter is NotFilter )
				{
					filter = ((NotFilter)filter).BaseFilter;
					this.ExcludeCategories = true;
				}

				this.SelectedCategories = filter.ToString();
			}
		}

        private void ProcessTreeNodes(TestSuiteTreeNodeArxNet node)
        {
            if (IsInteresting(node))
                this.Nodes.Add(new VisualTreeNode(node));

            foreach (TestSuiteTreeNodeArxNet childNode in node.Nodes)
                ProcessTreeNodes(childNode);
        }

        private bool IsInteresting(TestSuiteTreeNodeArxNet node)
        {
            return node.IsExpanded || node.Checked;
        }
		#endregion

		#region Instance Methods

		public void Save( string fileName )
		{
			using ( StreamWriter writer = new StreamWriter( fileName ) )
			{
				Save( writer );
			}
		}

		public void Save( TextWriter writer )
		{
			XmlSerializer serializer = new XmlSerializer( GetType() );
			serializer.Serialize( writer, this );
		}

        public void Restore(TestSuiteTreeViewArxNet treeView)
        {
            treeView.CheckBoxes = this.ShowCheckBoxes;

            foreach (VisualTreeNode visualNode in this.Nodes)
            {
                TestSuiteTreeNodeArxNet treeNode = treeView[visualNode.UniqueName];
                if (treeNode != null)
                {
                    if (treeNode.IsExpanded != visualNode.Expanded)
                        treeNode.Toggle();

                    treeNode.Checked = visualNode.Checked;
                }
            }

            if (this.SelectedNode != null)
            {
                TestSuiteTreeNodeArxNet treeNode = treeView[this.SelectedNode];
                if (treeNode != null)
                    treeView.SelectedNode = treeNode;
            }

            if (this.TopNode != null)
            {
                TestSuiteTreeNodeArxNet treeNode = treeView[this.TopNode];
                if (treeNode != null)
                    treeView.TopNode = treeNode;
            }

            if (this.SelectedCategories != null)
            {
                TestFilter filter = new CategoryFilter(this.SelectedCategories.Split(new char[] { ',' }));
                if (this.ExcludeCategories)
                    filter = new NotFilter(filter);
                treeView.CategoryFilter = filter;
            }

            treeView.Select();
        }

		#endregion
	}

	[Serializable]
	public class VisualTreeNode
	{
		[XmlAttribute]
		public string UniqueName;

		[XmlAttribute,System.ComponentModel.DefaultValue(false)]
		public bool Expanded;

		[XmlAttribute,System.ComponentModel.DefaultValue(false)]
		public bool Checked;

		[XmlArrayItem("Node")]
		public VisualTreeNode[] Nodes;

		public VisualTreeNode() { }

		public VisualTreeNode( TestSuiteTreeNodeArxNet treeNode )
		{
			this.UniqueName = treeNode.Test.TestName.UniqueName;
			this.Expanded = treeNode.IsExpanded;
			this.Checked = treeNode.Checked;
		}
    }
}