// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.5.28修改
//      1.nunit2.6.2基础上修改
//      2.改ProjectService为ProjectServiceArxNet
//  2014.9.24：
//      在NUnit2.6.3基础上修改
//  2015.2.4：
//      在NUnit2.6.4基础上修改
// ****************************************************************

using System;
using System.IO;

namespace NUnit.Util.ArxNet.Tests
{
	/// <summary>
	/// Summary description for NUnitProjectXml.
	/// </summary>
	public class NUnitProjectArxNetXml
	{
		public static readonly string EmptyProject = "<NUnitProject />";
		
		public static readonly string EmptyConfigs = 
			"<NUnitProject>" + System.Environment.NewLine +
			"  <Settings activeconfig=\"Debug\" />" + System.Environment.NewLine +
			"  <Config name=\"Debug\" binpathtype=\"Auto\" />" + System.Environment.NewLine +
			"  <Config name=\"Release\" binpathtype=\"Auto\" />" + System.Environment.NewLine +
			"</NUnitProject>";
		
		public static readonly string NormalProject =
			"<NUnitProject>" + System.Environment.NewLine +
			"  <Settings activeconfig=\"Debug\" />" + System.Environment.NewLine +
			"  <Config name=\"Debug\" appbase=\"bin" + Path.DirectorySeparatorChar + "debug\" binpathtype=\"Auto\">" + System.Environment.NewLine +
			"    <assembly path=\"assembly1.dll\" />" + System.Environment.NewLine +
			"    <assembly path=\"assembly2.dll\" />" + System.Environment.NewLine +
			"  </Config>" + System.Environment.NewLine +
			"  <Config name=\"Release\" appbase=\"bin" + Path.DirectorySeparatorChar + "release\" binpathtype=\"Auto\">" + System.Environment.NewLine +
			"    <assembly path=\"assembly1.dll\" />" + System.Environment.NewLine +
			"    <assembly path=\"assembly2.dll\" />" + System.Environment.NewLine +
			"  </Config>" + System.Environment.NewLine +
			"</NUnitProject>";
		
		public static readonly string ManualBinPathProject =
			"<NUnitProject>" + System.Environment.NewLine +
			"  <Settings activeconfig=\"Debug\" />" + System.Environment.NewLine +
			"  <Config name=\"Debug\" binpath=\"bin_path_value\"  /> " + System.Environment.NewLine +
			"</NUnitProject>";
	}
}
