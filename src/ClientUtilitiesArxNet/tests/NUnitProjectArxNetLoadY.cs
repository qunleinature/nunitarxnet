// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.5.28修改
//   1.nunit2.6.2基础上修改
//   2.改ProjectService为ProjectServiceArxNet
//   3.改NUnitProjectXml为NUnitProjectArxNetXml
// ****************************************************************

using System.IO;
using NUnit.Framework;
using NUnit.TestUtilities;

namespace NUnit.Util.ArxNet.Tests
{
	// TODO: Some of these tests are really tests of VSProject and should be moved there.

	[TestFixture]
	public class NUnitProjectArxNetLoad
	{
		static readonly string xmlfile = Path.Combine(Path.GetTempPath(), "test.nunit");
        static readonly string mockDll = NUnit.Tests.Assemblies.MockAssembly.AssemblyPath;

		private ProjectServiceArxNet projectService;
		private NUnitProject project;

		[SetUp]
		public void SetUp()
		{
			projectService = new ProjectServiceArxNet();
			project = projectService.EmptyProject();
		}

		[TearDown]
		public void TearDown()
		{
			if ( File.Exists( xmlfile ) )
				File.Delete( xmlfile );
		}

		// Write a string out to our xml file and then load project from it
		private void LoadProject( string source )
		{
			StreamWriter writer = new StreamWriter( xmlfile );
			writer.Write( source );
			writer.Close();

			project.ProjectPath = Path.GetFullPath( xmlfile );
			project.Load();
		}

		[Test]
		public void LoadEmptyProject()
		{
			LoadProject( NUnitProjectArxNetXml.EmptyProject );
			Assert.AreEqual( 0, project.Configs.Count );
		}

		[Test]
		public void LoadEmptyConfigs()
		{
			LoadProject( NUnitProjectArxNetXml.EmptyConfigs );
			Assert.AreEqual( 2, project.Configs.Count );
			Assert.IsTrue( project.Configs.Contains( "Debug") );
			Assert.IsTrue( project.Configs.Contains( "Release") );
		}

		[Test]
		public void LoadNormalProject()
		{
			LoadProject( NUnitProjectArxNetXml.NormalProject );
			Assert.AreEqual( 2, project.Configs.Count );

            string tempPath = Path.GetTempPath();

			ProjectConfig config1 = project.Configs["Debug"];
			Assert.AreEqual( 2, config1.Assemblies.Count );
			Assert.AreEqual( Path.Combine(tempPath, @"bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly1.dll" ), config1.Assemblies[0] );
			Assert.AreEqual( Path.Combine(tempPath, @"bin" + Path.DirectorySeparatorChar + "debug" + Path.DirectorySeparatorChar + "assembly2.dll" ), config1.Assemblies[1] );

			ProjectConfig config2 = project.Configs["Release"];
			Assert.AreEqual( 2, config2.Assemblies.Count );
			Assert.AreEqual( Path.Combine(tempPath, @"bin" + Path.DirectorySeparatorChar + "release" + Path.DirectorySeparatorChar + "assembly1.dll" ), config2.Assemblies[0] );
			Assert.AreEqual( Path.Combine(tempPath, @"bin" + Path.DirectorySeparatorChar + "release" + Path.DirectorySeparatorChar + "assembly2.dll" ), config2.Assemblies[1] );
		}

		[Test]
		public void LoadProjectWithManualBinPath()
		{
			LoadProject( NUnitProjectArxNetXml.ManualBinPathProject );
			Assert.AreEqual( 1, project.Configs.Count );
			ProjectConfig config1 = project.Configs["Debug"];
			Assert.AreEqual( "bin_path_value", config1.PrivateBinPath );
		}

		[Test]
		public void FromAssembly()
		{
			NUnitProject project = projectService.WrapAssembly(mockDll);
			Assert.AreEqual( "Default", project.ActiveConfigName );
			Assert.SamePath( mockDll, project.ActiveConfig.Assemblies[0] );
			Assert.IsTrue( project.IsLoadable, "Not loadable" );
			Assert.IsTrue( project.IsAssemblyWrapper, "Not wrapper" );
			Assert.IsFalse( project.IsDirty, "Not dirty" );
		}

		[Test]
		public void SaveClearsAssemblyWrapper()
		{
			NUnitProject project = projectService.WrapAssembly(mockDll);
			project.Save( xmlfile );
			Assert.IsFalse( project.IsAssemblyWrapper,
				"Changed project should no longer be wrapper");
		}
	}
}
