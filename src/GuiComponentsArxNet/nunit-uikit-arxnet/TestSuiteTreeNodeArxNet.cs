// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.5.31修改：
//      1.在nunit2.6.2基础上修改
//      2.NUnit.UiKit.TestSuiteTreeNode改为NUnit.UiKit.ArxNet.TestSuiteTreeNodeArxNet类
//  2015.2.9：
//      在NUnit2.6.4基础上修改
// ****************************************************************

namespace NUnit.UiKit.ArxNet
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using NUnit.Core;
	using NUnit.Util;

    /// <summary>
	/// Type safe TreeNode for use in the TestSuiteTreeView. 
	/// NOTE: Hides some methods and properties of base class.
	/// </summary>
	public class TestSuiteTreeNodeArxNet : TreeNode
	{
		#region Instance variables and constant definitions

		/// <summary>
		/// The testcase or testsuite represented by this node
		/// </summary>
		private ITest test;

		/// <summary>
		/// The result from the last run of the test
		/// </summary>
		private TestResult result;

		/// <summary>
		/// Private field used for inclusion by category
		/// </summary>
		private bool included = true;

        private bool showFailedAssumptions = false;

		/// <summary>
		/// Image indices for various test states - the values 
		/// must match the indices of the image list used
		/// </summary>
		public const int InitIndex = 0;
		public const int SkippedIndex = 0; 
		public const int FailureIndex = 1;
		public const int SuccessIndex = 2;
		public const int IgnoredIndex = 3;
	    public const int InconclusiveIndex = 4;

		#endregion

		#region Constructors

		/// <summary>
		/// Construct a TestNode given a test
		/// </summary>
		public TestSuiteTreeNodeArxNet( TestInfo test ) : base(test.TestName.Name)
		{
			this.test = test;
			UpdateImageIndex();
		}

		/// <summary>
		/// Construct a TestNode given a TestResult
		/// </summary>
		public TestSuiteTreeNodeArxNet( TestResult result ) : base( result.Test.TestName.Name )
		{
			this.test = result.Test;
			this.result = result;
			UpdateImageIndex();
		}

		#endregion

		#region Properties	
		/// <summary>
		/// Test represented by this node
		/// </summary>
		public ITest Test
		{
			get { return this.test; }
			set	{ this.test = value; }
		}

		/// <summary>
		/// Test result for this node
		/// </summary>
		public TestResult Result
		{
			get { return this.result; }
			set 
			{ 
				this.result = value;
				UpdateImageIndex();
			}
		}

        /// <summary>
        /// Return true if the node has a result, otherwise false.
        /// </summary>
        public bool HasResult
        {
            get { return this.result != null; }
        }

		public string TestType
		{
			get { return test.TestType; }
		}

		public string StatusText
		{
			get
			{
				if ( result == null )
					return test.RunState.ToString();

				return result.ResultState.ToString();
			}
		}

		public bool Included
		{
			get { return included; }
			set
			{ 
				included = value;
				this.ForeColor = included ? SystemColors.WindowText : Color.LightBlue;
			}
		}

        public bool ShowFailedAssumptions
        {
            get { return showFailedAssumptions; }
            set
            {
                if (value != showFailedAssumptions)
                {
                    showFailedAssumptions = value;

                    if (HasInconclusiveResults)
                        RepopulateTheoryNode();
                }
            }
        }

        public bool HasInconclusiveResults
        {
            get
            {
                bool hasInconclusiveResults = false;
                if (Result != null)
                {
                    foreach (TestResult result in Result.Results)
                    {
                        hasInconclusiveResults |= result.ResultState == ResultState.Inconclusive;
                        if (hasInconclusiveResults)
                            break;
                    }
                }
                return hasInconclusiveResults;
            }
        }

		#endregion

		#region Methods

		/// <summary>
		/// UPdate the image index based on the result field
		/// </summary>
		public void UpdateImageIndex()
		{
			ImageIndex = SelectedImageIndex = CalcImageIndex();
		}

		/// <summary>
		/// Clear the result of this node and all its children
		/// </summary>
		public void ClearResults()
		{
			this.result = null;
			ImageIndex = SelectedImageIndex = CalcImageIndex();

			foreach(TestSuiteTreeNodeArxNet node in Nodes)
				node.ClearResults();
		}

        /// <summary>
        /// Gets the Theory node associated with the current
        /// node. If the current node is a Theory, then the
        /// current node is returned. Otherwise, if the current
        /// node is a test case under a theory node, then that
        /// node is returned. Otherwise, null is returned.
        /// </summary>
        /// <returns></returns>
        public TestSuiteTreeNodeArxNet GetTheoryNode()
        {
            if (this.Test.TestType == "Theory")
                return this;

            TestSuiteTreeNodeArxNet parent = this.Parent as TestSuiteTreeNodeArxNet;
            if (parent != null && parent.Test.TestType == "Theory")
                return parent;

            return null;
        }

        /// <summary>
        /// Regenerate the test cases under a theory, respecting
        /// the current setting for ShowFailedAssumptions
        /// </summary>
        public void RepopulateTheoryNode()
        {
            // Ignore if it's not a theory or if it has not been run yet
            if (this.Test.TestType == "Theory" && this.HasResult)
            {
                Nodes.Clear();

                foreach (TestResult result in Result.Results)
                    if (showFailedAssumptions || result.ResultState != ResultState.Inconclusive)
                        Nodes.Add(new TestSuiteTreeNodeArxNet(result));
            }
        }

        /// <summary>
		/// Calculate the image index based on the node contents
		/// </summary>
		/// <returns>Image index for this node</returns>
		private int CalcImageIndex()
		{
            if (this.result == null)
            {
                switch (this.test.RunState)
                {
                    case RunState.Ignored:
                        return IgnoredIndex;
                    case RunState.NotRunnable:
                        return FailureIndex;
                    default:
                        return InitIndex;
                }
            }
            else
            {
                switch (this.result.ResultState)
                {
                    case ResultState.Inconclusive:
                        return InconclusiveIndex;
                    case ResultState.Skipped:
                        return SkippedIndex;
                    case ResultState.NotRunnable:
                    case ResultState.Failure:
                    case ResultState.Error:
                    case ResultState.Cancelled:
                        return FailureIndex;
                    case ResultState.Ignored:
                        return IgnoredIndex;
                    case ResultState.Success:
                        int resultIndex = SuccessIndex;
                        foreach (TestSuiteTreeNodeArxNet node in this.Nodes)
                        {
                            if (node.ImageIndex == FailureIndex)
                                return FailureIndex; // Return FailureIndex if there is any failure
                            if (node.ImageIndex == IgnoredIndex)
                                resultIndex = IgnoredIndex; // Remember IgnoredIndex - we might still find a failure
                        }
                        return resultIndex;
                    default:
                        return InitIndex;
                }
            }
		}

		internal void Accept(TestSuiteTreeNodeVisitor visitor) 
		{
			visitor.Visit(this);
			foreach (TestSuiteTreeNodeArxNet node in this.Nodes) 
			{
				node.Accept(visitor);
			}
		}

		#endregion
	}

	public abstract class TestSuiteTreeNodeVisitor 
	{
		public abstract void Visit(TestSuiteTreeNodeArxNet node);
	}
}

