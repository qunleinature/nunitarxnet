// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.5.29修改
//   1.nunit2.6.2基础上修改
//   2.改ProjectService为ProjectServiceArxNet
//   3.改NUnitProjectXml为NUnitProjectArxNetXml
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.9.24：
//  在NUnit2.6.3基础上修改
// ****************************************************************

using System;
using System.Text;
using System.Xml;
using System.IO;
using NUnit.Framework;

namespace NUnit.Util.ArxNet.Tests
{
	[TestFixture]
	public class NUnitProjectArxNetSave
	{
		static readonly string xmlfile = Path.Combine(Path.GetTempPath(), "test.nunit");

		private NUnitProject project;

		[SetUp]
		public void SetUp()
		{
			project = new ProjectService().EmptyProject();
		}

		[TearDown]
		public void TearDown()
		{
			if ( File.Exists( xmlfile ) )
				File.Delete( xmlfile );
		}

		private void CheckContents( string expected )
		{
			StreamReader reader = new StreamReader( xmlfile );
			string contents = reader.ReadToEnd();
			reader.Close();
			Assert.AreEqual( expected, contents );
		}

		[Test]
		public void SaveEmptyProject()
		{
			project.Save( xmlfile );

			CheckContents( NUnitProjectArxNetXml.EmptyProject );
		}

		[Test]
		public void SaveEmptyConfigs()
		{
			project.Configs.Add( "Debug" );
			project.Configs.Add( "Release" );

			project.Save( xmlfile );

			CheckContents( NUnitProjectArxNetXml.EmptyConfigs );			
		}

		[Test]
		public void SaveNormalProject()
		{
            string tempPath = Path.GetTempPath();

			ProjectConfig config1 = new ProjectConfig( "Debug" );
			config1.BasePath = "bin" + Path.DirectorySeparatorChar + "debug";
			config1.Assemblies.Add( Path.Combine(tempPath, "bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly1.dll" ) );
			config1.Assemblies.Add( Path.Combine(tempPath, "bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly2.dll" ) );

			ProjectConfig config2 = new ProjectConfig( "Release" );
			config2.BasePath = "bin" + Path.DirectorySeparatorChar + "release";
			config2.Assemblies.Add( Path.Combine(tempPath, "bin" + Path.DirectorySeparatorChar + "release" + Path.DirectorySeparatorChar + "assembly1.dll" ) );
			config2.Assemblies.Add( Path.Combine(tempPath, "bin" + Path.DirectorySeparatorChar + "release" + Path.DirectorySeparatorChar + "assembly2.dll" ) );

			project.Configs.Add( config1 );
			project.Configs.Add( config2 );

			project.Save( xmlfile );

			CheckContents( NUnitProjectArxNetXml.NormalProject );
		}
	}
}
