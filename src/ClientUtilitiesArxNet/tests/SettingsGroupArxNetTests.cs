// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2012, Lei Qun
// 2013.1.6–ﬁ∏ƒ£∫≤‚ ‘NUnit.Util.ArxNet.SettingsGroupArxNet¿‡
// ****************************************************************

using System;
using NUnit.Framework;
using Microsoft.Win32;

namespace NUnit.Util.ArxNet.Tests
{
	[TestFixture]
    public class SettingsGroupArxNetTests
	{
		private SettingsGroupArxNet testGroup;
        //private SettingsGroupArxNet testGroup_null = null;
        private SettingsGroupArxNet testGroup_storage_null;

		[SetUp]
		public void BeforeEachTest()
		{
			MemorySettingsStorage storage = new MemorySettingsStorage();
            testGroup = new SettingsGroupArxNet(storage);
            testGroup_storage_null = new SettingsGroupArxNet(null);
		}

		[TearDown]
		public void AfterEachTest()
		{
            testGroup_storage_null.Dispose();
			testGroup.Dispose();
		}

		[Test]
		public void TopLevelSettings()
		{
			testGroup.SaveSetting( "X", 5 );
			testGroup.SaveSetting( "NAME", "Charlie" );
			Assert.AreEqual( 5, testGroup.GetSetting( "X" ) );
			Assert.AreEqual( "Charlie", testGroup.GetSetting( "NAME" ) );

			testGroup.RemoveSetting( "X" );
			Assert.IsNull( testGroup.GetSetting( "X" ), "X not removed" );
			Assert.AreEqual( "Charlie", testGroup.GetSetting( "NAME" ) );

			testGroup.RemoveSetting( "NAME" );
			Assert.IsNull( testGroup.GetSetting( "NAME" ), "NAME not removed" );
		}

        [Test]
        public void TopLevelSettings_storage_null()
        {
            testGroup_storage_null.SaveSetting("X", 5);
            testGroup_storage_null.SaveSetting("NAME", "Charlie");
            Assert.IsNull(testGroup_storage_null.GetSetting("X"));
            Assert.IsNull(testGroup_storage_null.GetSetting("NAME"));

            testGroup_storage_null.RemoveSetting("X");
            Assert.IsNull(testGroup_storage_null.GetSetting("X"), "X not removed");
            Assert.IsNull(testGroup_storage_null.GetSetting("NAME"));

            testGroup_storage_null.RemoveSetting("NAME");
            Assert.IsNull(testGroup_storage_null.GetSetting("NAME"), "NAME not removed");
        }

		[Test]
		public void SubGroupSettings()
		{
			SettingsGroupArxNet subGroup = new SettingsGroupArxNet( testGroup.Storage );
			Assert.IsNotNull( subGroup );
			Assert.IsNotNull( subGroup.Storage );

			subGroup.SaveSetting( "X", 5 );
			subGroup.SaveSetting( "NAME", "Charlie" );
			Assert.AreEqual( 5, subGroup.GetSetting( "X" ) );
			Assert.AreEqual( "Charlie", subGroup.GetSetting( "NAME" ) );

			subGroup.RemoveSetting( "X" );
			Assert.IsNull( subGroup.GetSetting( "X" ), "X not removed" );
			Assert.AreEqual( "Charlie", subGroup.GetSetting( "NAME" ) );

			subGroup.RemoveSetting( "NAME" );
			Assert.IsNull( subGroup.GetSetting( "NAME" ), "NAME not removed" );
		}

        [Test]
        public void SubGroupSettings_storage_null()
        {
            SettingsGroupArxNet subGroup = new SettingsGroupArxNet(testGroup_storage_null.Storage);
            Assert.IsNotNull(subGroup);
            Assert.IsNull(subGroup.Storage);

            subGroup.SaveSetting("X", 5);
            subGroup.SaveSetting("NAME", "Charlie");
            Assert.IsNull(subGroup.GetSetting("X"));
            Assert.IsNull(subGroup.GetSetting("NAME"));

            subGroup.RemoveSetting("X");
            Assert.IsNull(subGroup.GetSetting("X"), "X not removed");
            Assert.IsNull(subGroup.GetSetting("NAME"));

            subGroup.RemoveSetting("NAME");
            Assert.IsNull(subGroup.GetSetting("NAME"), "NAME not removed");
        }

		[Test]
		public void TypeSafeSettings()
		{
			testGroup.SaveSetting( "X", 5);
			testGroup.SaveSetting( "Y", "17" );
			testGroup.SaveSetting( "NAME", "Charlie");

			Assert.AreEqual( 5, testGroup.GetSetting("X") );
			Assert.AreEqual( "17", testGroup.GetSetting( "Y" ) );
			Assert.AreEqual( "Charlie", testGroup.GetSetting( "NAME" ) );
		}

        [Test]
        public void TypeSafeSettings_storage_null()
        {
            testGroup_storage_null.SaveSetting("X", 5);
            testGroup_storage_null.SaveSetting("Y", "17");
            testGroup_storage_null.SaveSetting("NAME", "Charlie");

            Assert.IsNull(testGroup_storage_null.GetSetting("X"));
            Assert.IsNull(testGroup_storage_null.GetSetting("Y"));
            Assert.IsNull(testGroup_storage_null.GetSetting("NAME"));
        }

		[Test]
		public void DefaultSettings()
		{
			Assert.IsNull( testGroup.GetSetting( "X" ) );
			Assert.IsNull( testGroup.GetSetting( "NAME" ) );

			Assert.AreEqual( 5, testGroup.GetSetting( "X", 5 ) );
			Assert.AreEqual( 6, testGroup.GetSetting( "X", 6 ) );
			Assert.AreEqual( "7", testGroup.GetSetting( "X", "7" ) );

			Assert.AreEqual( "Charlie", testGroup.GetSetting( "NAME", "Charlie" ) );
			Assert.AreEqual( "Fred", testGroup.GetSetting( "NAME", "Fred" ) );
		}

        [Test]
        public void DefaultSettings_storage_null()
        {
            Assert.IsNull(testGroup_storage_null.GetSetting("X"));
            Assert.IsNull(testGroup_storage_null.GetSetting("NAME"));

            Assert.AreEqual(5, testGroup_storage_null.GetSetting("X", 5));
            Assert.AreEqual(6, testGroup_storage_null.GetSetting("X", 6));
            Assert.AreEqual("7", testGroup_storage_null.GetSetting("X", "7"));

            Assert.AreEqual("Charlie", testGroup_storage_null.GetSetting("NAME", "Charlie"));
            Assert.AreEqual("Fred", testGroup_storage_null.GetSetting("NAME", "Fred"));
        }

		[Test]
		public void BadSetting()
		{
			testGroup.SaveSetting( "X", "1y25" );
			Assert.AreEqual( 12, testGroup.GetSetting( "X", 12 ) );
		}

        [Test]
        public void BadSetting_storage_null()
        {
            testGroup_storage_null.SaveSetting("X", "1y25");
            Assert.AreEqual(12, testGroup.GetSetting("X", 12));
        }
	}
}
