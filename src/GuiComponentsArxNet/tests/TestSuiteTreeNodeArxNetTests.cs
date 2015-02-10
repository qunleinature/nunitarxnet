// ****************************************************************
// Copyright 2002-2003, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun 
//  2013.5.31修改
//  2013.6.25
//      1.已改在NUnit2.6.2基础
//      2.已改TestSuiteTreeNodeArxNet
//  2015.2.9：
//      在NUnit2.6.4基础上修改
// ****************************************************************

namespace NUnit.UiKit.ArxNet.Tests
{
	using System;
	using NUnit.Core;
	using NUnit.Core.Builders;
	using NUnit.Framework;
	using NUnit.Util;
	using NUnit.Tests.Assemblies;
    using NUnit.TestUtilities;

	/// <summary>
	/// Summary description for TestSuiteTreeNodeTests.
	/// </summary>
	[TestFixture]
	public class TestSuiteTreeNodeArxNetTests
	{
		TestSuite testSuite;
		Test testFixture;
		Test testCase;

		[SetUp]
		public void SetUp()
		{
			testSuite = new TestSuite("MyTestSuite");
			testFixture = TestFixtureBuilder.BuildFrom( typeof( MockTestFixture ) );
			testSuite.Add( testFixture );

			testCase = TestFinder.Find("MockTest1", testFixture, false);
		}

		[Test]
		public void CanConstructFromTestSuite()
		{
			TestSuiteTreeNodeArxNet node = new TestSuiteTreeNodeArxNet( new TestInfo(testSuite) );
			Assert.AreEqual( "MyTestSuite", node.Text );
			Assert.AreEqual( "TestSuite", node.TestType );
        }

        [Test]
        public void CanConstructFromTestFixture()
        {
			TestSuiteTreeNodeArxNet node = new TestSuiteTreeNodeArxNet( new TestInfo(testFixture) );
			Assert.AreEqual( "MockTestFixture", node.Text );
			Assert.AreEqual( "TestFixture", node.TestType );
        }

        [Test]
        public void CanConstructFromTestCase()
        {
			TestSuiteTreeNodeArxNet node = new TestSuiteTreeNodeArxNet( new TestInfo(testCase) );
			Assert.AreEqual( "MockTest1", node.Text );
			Assert.AreEqual( "TestMethod", node.TestType );
		}

        [TestCase("MockTest1", TestSuiteTreeNodeArxNet.InitIndex)]
        [TestCase("MockTest4", TestSuiteTreeNodeArxNet.IgnoredIndex)]
        [TestCase("NotRunnableTest", TestSuiteTreeNodeArxNet.FailureIndex)]
        public void WhenResultIsNotSet_IndexReflectsRunState(string testName, int expectedIndex)
        {
            Test test = TestFinder.Find(testName, testFixture, false);
            TestSuiteTreeNodeArxNet node = new TestSuiteTreeNodeArxNet(new TestInfo(test));

            Assert.AreEqual(expectedIndex, node.ImageIndex);
            Assert.AreEqual(expectedIndex, node.SelectedImageIndex);
        }

        [TestCase(ResultState.Inconclusive, TestSuiteTreeNodeArxNet.InconclusiveIndex)]
        [TestCase(ResultState.NotRunnable, TestSuiteTreeNodeArxNet.FailureIndex)]
        [TestCase(ResultState.Skipped, TestSuiteTreeNodeArxNet.SkippedIndex)]
        [TestCase(ResultState.Ignored, TestSuiteTreeNodeArxNet.IgnoredIndex)]
        [TestCase(ResultState.Success, TestSuiteTreeNodeArxNet.SuccessIndex)]
        [TestCase(ResultState.Failure, TestSuiteTreeNodeArxNet.FailureIndex)]
        [TestCase(ResultState.Error, TestSuiteTreeNodeArxNet.FailureIndex)]
        [TestCase(ResultState.Cancelled, TestSuiteTreeNodeArxNet.FailureIndex)]
        public void WhenResultIsSet_IndexReflectsResultState(ResultState resultState, int expectedIndex)
        {
            TestSuiteTreeNodeArxNet node = new TestSuiteTreeNodeArxNet(new TestInfo(testCase));
            TestResult result = new TestResult(testCase);

            result.SetResult(resultState, null, null);
            node.Result = result;
            Assert.AreEqual(expectedIndex, node.ImageIndex);
            Assert.AreEqual(expectedIndex, node.SelectedImageIndex);
            Assert.AreEqual(resultState.ToString(), node.StatusText);
        }

        [TestCase("MockTest1", TestSuiteTreeNodeArxNet.InitIndex)]
        [TestCase("MockTest4", TestSuiteTreeNodeArxNet.IgnoredIndex)]
        [TestCase("NotRunnableTest", TestSuiteTreeNodeArxNet.FailureIndex)]
        public void WhenResultIsCleared_IndexReflectsRunState(string testName, int expectedIndex)
		{
            Test test = TestFinder.Find(testName, testFixture, false);
			TestResult result = new TestResult( test );
			result.Failure("message", "stacktrace");

			TestSuiteTreeNodeArxNet node = new TestSuiteTreeNodeArxNet( result );
			Assert.AreEqual( TestSuiteTreeNodeArxNet.FailureIndex, node.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNodeArxNet.FailureIndex, node.SelectedImageIndex );

			node.ClearResults();
			Assert.AreEqual( null, node.Result );
			Assert.AreEqual( expectedIndex, node.ImageIndex );
			Assert.AreEqual( expectedIndex, node.SelectedImageIndex );
		}
		
		[Test]
		public void WhenResultIsCleared_NestedResultsAreAlsoCleared()
		{
			TestResult testCaseResult = new TestResult( testCase );
			testCaseResult.Success();
			TestResult testSuiteResult = new TestResult( testFixture );
			testSuiteResult.AddResult( testCaseResult );
            testSuiteResult.Success();

			TestSuiteTreeNodeArxNet node1 = new TestSuiteTreeNodeArxNet( testSuiteResult );
			TestSuiteTreeNodeArxNet node2 = new TestSuiteTreeNodeArxNet( testCaseResult );
			node1.Nodes.Add( node2 );

			Assert.AreEqual( TestSuiteTreeNodeArxNet.SuccessIndex, node1.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNodeArxNet.SuccessIndex, node1.SelectedImageIndex );
			Assert.AreEqual( TestSuiteTreeNodeArxNet.SuccessIndex, node2.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNodeArxNet.SuccessIndex, node2.SelectedImageIndex );

			node1.ClearResults();

			Assert.AreEqual( TestSuiteTreeNodeArxNet.InitIndex, node1.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNodeArxNet.InitIndex, node1.SelectedImageIndex );
			Assert.AreEqual( TestSuiteTreeNodeArxNet.InitIndex, node2.ImageIndex );
			Assert.AreEqual( TestSuiteTreeNodeArxNet.InitIndex, node2.SelectedImageIndex );
		}
	}
}
