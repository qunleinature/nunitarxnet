// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2013, Lei Qun
// 2013.1.25：
//  在NUnit2.6.2基础上修改
// 2013.5.25：
//  使用EditorWritor类在Editor输出
// 2013.5.27：
//  1.Services改为ServicesArxNet
// ****************************************************************

// ****************************************************************
// Copyright 2014, Lei Qun
// 2014.8.22：
//  在NUnit2.6.3基础上修改
// ****************************************************************

namespace NUnit.CommandRunner.ArxNet
{
	using System;
	using Codeblast;

	using NUnit.Util;
    using NUnit.Core;
    using NUnit.ConsoleRunner;

    using NUnit.Util.ArxNet;

    using Autodesk.AutoCAD.Runtime;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.ApplicationServices;

	public class CommandOptionsArxNet : CommandLineOptions
	{
		[Option(Short="load", Description = "Test fixture or namespace to be loaded (Deprecated)")]
		public string fixture;

		[Option(Description = "Name of the test case(s), fixture(s) or namespace(s) to run")]
		public string run;

        [Option(Description = "Name of a file containing a list of the tests to run, one per line")]
        public string runlist;

		[Option(Description = "Project configuration (e.g.: Debug) to load")]
		public string config;

		[Option(Short="xml", Description = "Name of XML result file (Default: TestResult.xml)")]
		public string result;

		[Option(Description = "Display XML to the console (Deprecated)")]
		public bool xmlConsole;

        [Option(Short="noxml", Description = "Suppress XML result output")]
        public bool noresult;

		[Option(Short="out", Description = "File to receive test output")]
		public string output;

		[Option(Description = "File to receive test error output")]
		public string err;

        [Option(Description = "Work directory for output files")]
        public string work;

		[Option(Description = "Label each test in stdOut")]
		public bool labels = false;

        [Option(Description = "Set internal trace level: Off, Error, Warning, Info, Verbose")]
        public InternalTraceLevel trace;

		[Option(Description = "List of categories to include")]
		public string include;

		[Option(Description = "List of categories to exclude")]
		public string exclude;

#if CLR_2_0 || CLR_4_0
        [Option(Description = "Framework version to be used for tests")]
        public string framework;

        [Option(Description = "Process model for tests: Single, Separate, Multiple")]
		public ProcessModel process;
#endif

		[Option(Description = "AppDomain Usage for tests: None, Single, Multiple")]
		public DomainUsage domain;

        [Option(Description = "Apartment for running tests: MTA (Default), STA")]
        public System.Threading.ApartmentState apartment = System.Threading.ApartmentState.Unknown;

        [Option(Description = "Disable shadow copy when running in separate domain")]
		public bool noshadow;

		[Option (Description = "Disable use of a separate thread for tests")]
		public bool nothread;

		[Option(Description = "Base path to be used when loading the assemblies")]
 		public string basepath;
 
 		[Option(Description = "Additional directories to be probed when loading assemblies, separated by semicolons")]
 		public string privatebinpath;

        [Option(Description = "Set timeout for each test case in milliseconds")]
        public int timeout;

		[Option(Description = "Wait for input before closing console window")]
		public bool wait = false;

		[Option(Description = "Do not display the logo")]
		public bool nologo = false;

		[Option(Description = "Do not display progress" )]
		public bool nodots = false;

        [Option(Description = "Stop after the first test failure or error")]
        public bool stoponerror = false;

        [Option(Description = "Erase any leftover cache files and exit")]
        public bool cleanup;

        [Option(Short = "?", Description = "Display help")]
		public bool help = false;

		public CommandOptionsArxNet( params string[] args ) : base( args ) 
        {
            /*2013.1.25加*/
            process = ProcessModel.Single;
            domain = DomainUsage.None;
            nothread = true;
            /*2013.1.25加*/
        }

		public CommandOptionsArxNet( bool allowForwardSlash, params string[] args ) : base( allowForwardSlash, args ) 
        {
            /*2013.1.25加*/
            process = ProcessModel.Single;
            domain = DomainUsage.None;
            nothread = true;
            /*2013.1.25加*/
        }

		public bool Validate()
		{
			if(isInvalid) return false; 

			if(NoArgs) return true; 

			if(ParameterCount >= 1) return true; 

			return false;
		}

//		protected override bool IsValidParameter(string parm)
//		{
//			return ServicesArxNet.ProjectLoadService.CanLoadProject( parm ) || PathUtils.IsAssemblyFileType( parm );
//		}


        public bool IsTestProject
        {
            get
            {
                return ParameterCount == 1 && ServicesArxNet.ProjectService.CanLoadProject((string)Parameters[0]);
            }
        }

		public override void Help()
		{
            /*2013.1.25改*/
            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            /*
            ed.WriteMessage("\n");
            ed.WriteMessage("\nNUNIT-COMMAND");
            ed.WriteMessage("\nargs:[inputfiles] [options]");
            ed.WriteMessage("\n");
            ed.WriteMessage("\nRuns a set of NUnit tests from the AutoCad command lines.");
            ed.WriteMessage("\n");
            ed.WriteMessage("\nYou may specify one or more assemblies or a single");
            ed.WriteMessage("\nproject file of type .nunit.");
            ed.WriteMessage("\n");
            ed.WriteMessage("\nOptions:");
            ed.WriteMessage("\n" + GetHelpText());
            ed.WriteMessage("\n");
            ed.WriteMessage("\nOptions that take values may use an equal sign, a colon");
            ed.WriteMessage("\nor a space to separate the option from its value.");
            ed.WriteMessage("\n");*/
            /*2013.1.25改*/

            /*2013.5.25改*/
            Console.WriteLine();
            Console.WriteLine("NUNIT-COMMAND");
            Console.WriteLine("args:[inputfiles] [options]");
            Console.WriteLine();
            Console.WriteLine("Runs a set of NUnit tests from the AutoCad command lines.");
            Console.WriteLine();
            Console.WriteLine("You may specify one or more assemblies or a single");
            Console.WriteLine("project file of type .nunit.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            base.Help();
            Console.WriteLine();
            Console.WriteLine("Options that take values may use an equal sign, a colon");
            Console.WriteLine("or a space to separate the option from its value.");
            Console.WriteLine();
            /*2013.5.25改*/
		}        
	}
}