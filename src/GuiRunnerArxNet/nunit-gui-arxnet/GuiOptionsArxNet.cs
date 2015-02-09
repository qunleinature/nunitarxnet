// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2015, Lei Qun
//  2013.1.23修改
//      1.修改GetHelpText方法
//  2013.5.28：
//      1.在NUnit2.6.2基础上修改
//  2014.9.28：
//      在NUnit2.6.3基础上修改
//  2015.2.9：
//      在NUnit2.6.4基础上修改
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun

// ****************************************************************

namespace NUnit.Gui.ArxNet
{
	using System;
	using System.Text;
	using Codeblast;

	public class GuiOptionsArxNet : CommandLineOptions
	{
		[Option(Description = "Fixture to test")]
		public string fixture;

		[Option(Description = "List of categories to include")]
		public string include;

		[Option(Description = "List of categories to exclude")]
		public string exclude;

		[Option(Description = "Project configuration to load")]
		public string config;

		[Option(Description = "Suppress loading of last project")]
		public bool noload;

		[Option(Description = "Automatically run the loaded project")]
		public bool run;

		[Option(Description = "Automatically run selected tests or all tests if none are selected")]
		public bool runselected;

		[Option(Description = "Create console display for viewing any unmanaged output")]
		public bool console;

		[Option(Description = "Language to use for the NUnit GUI")]
		public string lang;

		[Option(Description = "Erase any leftover cache files and exit")]
		public bool cleanup;

		[Option(Short="?", Description = "Display help")]
		public bool help = false;

		public GuiOptionsArxNet(String[] args) : base(args) { }

		private bool HasInclude 
		{
			get 
			{
				return include != null && include.Length != 0;
			}
		}

		private bool HasExclude 
		{
			get 
			{
				return exclude != null && exclude.Length != 0;
			}
		}

		public bool Validate()
		{
			if ( isInvalid ) return false;

			if ( HasInclude && HasExclude ) return false;

			return NoArgs || ParameterCount <= 1;
		}

        public override string GetHelpText()//2013.1.23修改,2013.6.2lq修改
		{
			return
				"命令:NUNIT-ArxNet\rargs:[inputfile] [options]\r\r" +
                "Runs a set of NUnit tests from the AutoCad command lines.You may specify\r" +
                "an assembly or a project file of type .nunit as input.\r\r" +
                "Options:\r" +
                base.GetHelpText() +
                "\rOptions that take values may use an equal sign, a colon\r" +
                "or a space to separate the option from its value."; //2013.6.2lq修改,2014.9.28改
		}

	}
}