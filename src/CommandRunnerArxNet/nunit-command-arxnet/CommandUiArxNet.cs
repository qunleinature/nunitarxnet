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
//  线程中无法用Editor输出,用StringWriter记录Editor输出
// 2013.6.28
//  1.已改CommandOptionsArxNet
// ****************************************************************

using System.Diagnostics;

namespace NUnit.CommandRunner.ArxNet
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Resources;
	using System.Text;

	using NUnit.Core;
	using NUnit.Core.Filters;
	using NUnit.Util;
    using NUnit.ConsoleRunner;

    using NUnit.Util.ArxNet;

    using Autodesk.AutoCAD.Runtime;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.ApplicationServices;

	/// <summary>
	/// Summary description for ConsoleUi.
	/// </summary>
	public class CommandUiArxNet
	{
		public static readonly int OK = 0;
		public static readonly int INVALID_ARG = -1;
		public static readonly int FILE_NOT_FOUND = -2;
		public static readonly int FIXTURE_NOT_FOUND = -3;
		public static readonly int UNEXPECTED_ERROR = -100;

        private string workDir;

		public CommandUiArxNet()
		{
		}

        public int Execute(CommandOptionsArxNet options)//2013.1.25改//2013.5.25lq改
		{
            this.workDir = options.work;
            if (workDir == null || workDir == string.Empty)
                workDir = Environment.CurrentDirectory;
            else
            {
                workDir = Path.GetFullPath(workDir);
                if (!Directory.Exists(workDir))
                    Directory.CreateDirectory(workDir);
            }

            //TextWriter outWriter = new StringWriter();//2013.1.25改
            TextWriter outWriter = Console.Out;//2013.5.25lq改
			bool redirectOutput = options.output != null && options.output != string.Empty;
			if ( redirectOutput )
			{
				StreamWriter outStreamWriter = new StreamWriter( Path.Combine(workDir, options.output) );
				outStreamWriter.AutoFlush = true;
				outWriter = outStreamWriter;
			}

            //TextWriter errorWriter = new StringWriter();//2013.1.25改
            TextWriter errorWriter = Console.Error;//2013.5.25lq改
			bool redirectError = options.err != null && options.err != string.Empty;
			if ( redirectError )
			{
				StreamWriter errorStreamWriter = new StreamWriter( Path.Combine(workDir, options.err) );
				errorStreamWriter.AutoFlush = true;
				errorWriter = errorStreamWriter;
			}

            TestPackage package = MakeTestPackage(options);

            ProcessModel processModel = package.Settings.Contains("ProcessModel")
                ? (ProcessModel)package.Settings["ProcessModel"]
                : ProcessModel.Default;

            DomainUsage domainUsage = package.Settings.Contains("DomainUsage")
                ? (DomainUsage)package.Settings["DomainUsage"]
                : DomainUsage.Default;

            RuntimeFramework framework = package.Settings.Contains("RuntimeFramework")
                ? (RuntimeFramework)package.Settings["RuntimeFramework"]
                : RuntimeFramework.CurrentFramework;

            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;//2013.1.25加

#if CLR_2_0 || CLR_4_0
            //ed.WriteMessage("\nProcessModel: {0}    DomainUsage: {1}", processModel, domainUsage);//2013.1.25改

            //ed.WriteMessage("\nExecution Runtime: {0}", framework);//2013.1.25改

            Console.WriteLine("ProcessModel: {0}    DomainUsage: {1}", processModel, domainUsage);//2013.5.25lq改

            Console.WriteLine("Execution Runtime: {0}", framework);//2013.5.25lq改
#else
            /*2013.1.25改*/
            /*
            ed.WriteMessage("\nDomainUsage: {0}", domainUsage);

            if (processModel != ProcessModel.Default && processModel != ProcessModel.Single)
                ed.WriteMessage("\nWarning: Ignoring project setting 'processModel={0}'", processModel);

            if (!RuntimeFramework.CurrentFramework.Supports(framework))
                ed.WriteMessage("\nWarning: Ignoring project setting 'runtimeFramework={0}'", framework);
            */
            /*2013.1.25改*/

            /*2013.5.25lq改*/
            Console.WriteLine("DomainUsage: {0}", domainUsage);

            if (processModel != ProcessModel.Default && processModel != ProcessModel.Single)
                Console.WriteLine("Warning: Ignoring project setting 'processModel={0}'", processModel);

            if (!RuntimeFramework.CurrentFramework.Supports(framework))
                Console.WriteLine("Warning: Ignoring project setting 'runtimeFramework={0}'", framework);
            /*2013.5.25lq改*/
#endif

            using (TestRunner testRunner = new DefaultTestRunnerFactoryArxNet().MakeTestRunner(package))
			{
                testRunner.Load(package);

                if (testRunner.Test == null)
				{
					testRunner.Unload();
                    //ed.WriteMessage("\nUnable to locate fixture {0}", options.fixture);//2013.1.25改
                    Console.Error.WriteLine("Unable to locate fixture {0}", options.fixture);//2013.5.25lq改
					return FIXTURE_NOT_FOUND;
				}

                EventCollectorArxNet collector = new EventCollectorArxNet(options, outWriter, errorWriter);//2013.1.25改

				TestFilter testFilter;
					
				if(!CreateTestFilter(options, out testFilter))
					return INVALID_ARG;

				TestResult result = null;
				string savedDirectory = Environment.CurrentDirectory;
				TextWriter savedOut = Console.Out;
				TextWriter savedError = Console.Error;

				try
				{
					result = testRunner.Run( collector, testFilter, false, LoggingThreshold.Off );
                    /*2013.1.25加*/
                    //if (!redirectOutput) ed.WriteMessage("\n" + outWriter.ToString());
                    //if (!redirectError) ed.WriteMessage("\n" + errorWriter.ToString());
                    /*2013.1.25加*/                   
				}
				finally
				{
					outWriter.Flush();
					errorWriter.Flush();

					if (redirectOutput)
						outWriter.Close();

					if (redirectError)
						errorWriter.Close();

					Environment.CurrentDirectory = savedDirectory;
					Console.SetOut( savedOut );
					Console.SetError( savedError );
				}

                /*2013.5.25lq加*/
                Console.Write(collector.EditorStringWriter.ToString());//2013-5-25加，Editor输出StringWriter记录
                /*2013.5.25lq加*/

                //ed.WriteMessage("\n");//2013.1.25改
                Console.WriteLine();//2013.5.25lq改

                int returnCode = UNEXPECTED_ERROR;

                if (result != null)
                {
                    string xmlOutput = CreateXmlOutput(result);
                    ResultSummarizer summary = new ResultSummarizer(result);

                    if (options.xmlConsole)
                    {
                        //ed.WriteMessage("\n" + xmlOutput);//2013.1.25改
                        Console.WriteLine(xmlOutput);//2013.5.25lq改
                    }
                    else
                    {
                        WriteSummaryReport(summary);

                        bool hasErrors = summary.Errors > 0 || summary.Failures > 0 || result.IsError || result.IsFailure;

                        if (options.stoponerror && (hasErrors || summary.NotRunnable > 0))
                        {
                            //ed.WriteMessage("\nTest run was stopped after first error, as requested.");//2013.1.25改
                            //ed.WriteMessage("\n");//2013.1.25改
                            Console.WriteLine("Test run was stopped after first error, as requested.");//2013.5.25lq改
                            Console.WriteLine();//2013.5.25lq改
                        }

                        if (hasErrors)
                            WriteErrorsAndFailuresReport(result);

                        if (summary.TestsNotRun > 0)
                            WriteNotRunReport(result);

                        if (!options.noresult)
                        {
                            // Write xml output here
                            string xmlResultFile = options.result == null || options.result == string.Empty
                                ? "TestResult.xml" : options.result;

                            using (StreamWriter writer = new StreamWriter(Path.Combine(workDir, xmlResultFile)))
                            {
                                writer.Write(xmlOutput);
                            }
                        }
                    }

                    returnCode = summary.Errors + summary.Failures + summary.NotRunnable;
                }

				if (collector.HasExceptions)
				{
					collector.WriteExceptions();
					returnCode = UNEXPECTED_ERROR;
				}
            
				return returnCode;
			}
		}

        /*2013.5.25lq改*/
        /*2013.1.25改*/
        internal static bool CreateTestFilter(CommandOptionsArxNet options, out TestFilter testFilter)
		{
			testFilter = TestFilter.Empty;

			SimpleNameFilter nameFilter = new SimpleNameFilter();

            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;//2013.1.25加

			if (options.run != null && options.run != string.Empty)
			{
                //ed.WriteMessage("\nSelected test(s): " + options.run);//2013.1.25改
                Console.WriteLine("Selected test(s): " + options.run);//2013.5.25lq改

				foreach (string name in TestNameParser.Parse(options.run))
					nameFilter.Add(name);

				testFilter = nameFilter;
			}

			if (options.runlist != null && options.runlist != string.Empty)
			{
                //ed.WriteMessage("\nRun list: " + options.runlist);//2013.1.25改
                Console.WriteLine("Run list: " + options.runlist);//2013.5.25lq改
				
				try
				{
					using (StreamReader rdr = new StreamReader(options.runlist))
					{
						// NOTE: We can't use rdr.EndOfStream because it's
						// not present in .NET 1.x.
						string line = rdr.ReadLine();
						while (line != null && line.Length > 0)
						{
							if (line[0] != '#')
								nameFilter.Add(line);
							line = rdr.ReadLine();
						}
					}
				}
                /*2013.1.25改*/
                catch (System.Exception e)
                { 
                    if (e is FileNotFoundException || e is DirectoryNotFoundException)
                    {
                        //ed.WriteMessage("\nUnable to locate file: " + options.runlist);//2013.1.25改
                        Console.WriteLine("Unable to locate file: " + options.runlist);//2013.5.25lq改
                        return false;
                    }
                    throw;
                }
                /*2013.1.25改*/				

				testFilter = nameFilter;
			}

			if (options.include != null && options.include != string.Empty)
			{
				TestFilter includeFilter = new CategoryExpression(options.include).Filter;
                //ed.WriteMessage("\nIncluded categories: " + includeFilter.ToString());//2013.1.25改
                Console.WriteLine("Included categories: " + includeFilter.ToString());//2013.5.25lq改

				if (testFilter.IsEmpty)
					testFilter = includeFilter;
				else
					testFilter = new AndFilter(testFilter, includeFilter);
			}

			if (options.exclude != null && options.exclude != string.Empty)
			{
				TestFilter excludeFilter = new NotFilter(new CategoryExpression(options.exclude).Filter);
                //ed.WriteMessage("\nExcluded categories: " + excludeFilter.ToString());//2013.1.25改
                Console.WriteLine("Excluded categories: " + excludeFilter.ToString());//2013.5.25lq改

				if (testFilter.IsEmpty)
					testFilter = excludeFilter;
				else if (testFilter is AndFilter)
					((AndFilter) testFilter).Add(excludeFilter);
				else
					testFilter = new AndFilter(testFilter, excludeFilter);
			}

			if (testFilter is NotFilter)
				((NotFilter) testFilter).TopLevel = true;

			return true;
		}
        /*2013.1.25改*/
        /*2013.5.25lq改*/

		#region Helper Methods
        // TODO: See if this can be unified with the Gui's MakeTestPackage
        private TestPackage MakeTestPackage(CommandOptionsArxNet options)//2013.1.25改
        {
			TestPackage package;
			DomainUsage domainUsage = DomainUsage.Default;
            ProcessModel processModel = ProcessModel.Default;
            RuntimeFramework framework = null;

            string[] parameters = new string[options.ParameterCount];
            for (int i = 0; i < options.ParameterCount; i++)
                parameters[i] = Path.GetFullPath((string)options.Parameters[i]);

			if (options.IsTestProject)
			{
				NUnitProject project = 
					ServicesArxNet.ProjectService.LoadProject(parameters[0]);

				string configName = options.config;
				if (configName != null)
					project.SetActiveConfig(configName);

				package = project.ActiveConfig.MakeTestPackage();
                processModel = project.ProcessModel;
                domainUsage = project.DomainUsage;
                framework = project.ActiveConfig.RuntimeFramework;
			}
			else if (parameters.Length == 1)
			{
                package = new TestPackage(parameters[0]);
				domainUsage = DomainUsage.Single;
			}
			else
			{
                // TODO: Figure out a better way to handle "anonymous" packages
				package = new TestPackage(null, parameters);
                package.AutoBinPath = true;
				domainUsage = DomainUsage.Multiple;
			}

			if (options.basepath != null && options.basepath != string.Empty)
 			{
 				package.BasePath = options.basepath;
 			}
 
 			if (options.privatebinpath != null && options.privatebinpath != string.Empty)
 			{
 				package.AutoBinPath = false;
				package.PrivateBinPath = options.privatebinpath;
 			}

#if CLR_2_0 || CLR_4_0
            if (options.framework != null)
                framework = RuntimeFramework.Parse(options.framework);

            if (options.process != ProcessModel.Default)
                processModel = options.process;
#endif

			if (options.domain != DomainUsage.Default)
				domainUsage = options.domain;

			package.TestName = options.fixture;
            
            package.Settings["ProcessModel"] = processModel;
            package.Settings["DomainUsage"] = domainUsage;
            
			if (framework != null)
                package.Settings["RuntimeFramework"] = framework;

            if (domainUsage == DomainUsage.None)
            {
                // Make sure that addins are available
                CoreExtensions.Host.AddinRegistry = ServicesArxNet.AddinRegistry;
            }

            package.Settings["ShadowCopyFiles"] = !options.noshadow;
			package.Settings["UseThreadedRunner"] = !options.nothread;
            package.Settings["DefaultTimeout"] = options.timeout;
            package.Settings["WorkDirectory"] = this.workDir;
            package.Settings["StopOnError"] = options.stoponerror;

            if (options.apartment != System.Threading.ApartmentState.Unknown)
                package.Settings["ApartmentState"] = options.apartment;

            return package;
		}

		private static string CreateXmlOutput( TestResult result )
		{
			StringBuilder builder = new StringBuilder();
			new XmlResultWriter(new StringWriter( builder )).SaveTestResult(result);

			return builder.ToString();
		}

		private static void WriteSummaryReport( ResultSummarizer summary )
		{            
            /*2013.1.25改*/
            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage(
                //"\nTests run: {0}, Errors: {1}, Failures: {2}, Inconclusive: {3}, Time: {4} seconds",
                //summary.TestsRun, summary.Errors, summary.Failures, summary.Inconclusive, summary.Time);
            //ed.WriteMessage(
                //"\n  Not run: {0}, Invalid: {1}, Ignored: {2}, Skipped: {3}",
                //summary.TestsNotRun, summary.NotRunnable, summary.Ignored, summary.Skipped);
            //ed.WriteMessage("\n");

            /*2013.5.25lq改*/
            Console.WriteLine(
                "Tests run: {0}, Errors: {1}, Failures: {2}, Inconclusive: {3}, Time: {4} seconds",
                summary.TestsRun, summary.Errors, summary.Failures, summary.Inconclusive, summary.Time);
            Console.WriteLine(
                "  Not run: {0}, Invalid: {1}, Ignored: {2}, Skipped: {3}",
                summary.TestsNotRun, summary.NotRunnable, summary.Ignored, summary.Skipped);
            Console.WriteLine();

        }

        private void WriteErrorsAndFailuresReport(TestResult result)
        {
            reportIndex = 0;
            /*2013.1.25改*/
            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("\nErrors and Failures:");
            //WriteErrorsAndFailures(result);
            //ed.WriteMessage("\n");

            /*2013.5.25lq改*/
            Console.WriteLine("Errors and Failures:");
            WriteErrorsAndFailures(result);
            Console.WriteLine();
        }

        private void WriteErrorsAndFailures(TestResult result)
        {
            if (result.Executed)
            {
                if (result.HasResults)
                {
                    if (result.IsFailure || result.IsError)
                        if (result.FailureSite == FailureSite.SetUp || result.FailureSite == FailureSite.TearDown)
                            WriteSingleResult(result);

                    foreach (TestResult childResult in result.Results)
                        WriteErrorsAndFailures(childResult);
                }
                else if (result.IsFailure || result.IsError)
                {
                    WriteSingleResult(result);
                }
            }
        }

        private void WriteNotRunReport(TestResult result)
        {
            reportIndex = 0;
            /*2013.1.25改*/
            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("\nTests Not Run:");
            //WriteNotRunResults(result);
            //ed.WriteMessage("\n");

            /*2013.5.25lq改*/
            Console.WriteLine("Tests Not Run:");
            WriteNotRunResults(result);
            Console.WriteLine();
        }

	    private int reportIndex = 0;
        private void WriteNotRunResults(TestResult result)
        {
            if (result.HasResults)
                foreach (TestResult childResult in result.Results)
                    WriteNotRunResults(childResult);
            else if (!result.Executed)
                WriteSingleResult( result );
        }

        private void WriteSingleResult( TestResult result )
        {
            string status = result.IsFailure || result.IsError
                ? string.Format("{0} {1}", result.FailureSite, result.ResultState)
                : result.ResultState.ToString();

            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;//2013.1.25加
            //ed.WriteMessage("\n{0}) {1} : {2}", ++reportIndex, status, result.FullName);//2013.1.25改
            Console.WriteLine("{0}) {1} : {2}", ++reportIndex, status, result.FullName);//2013.5.25lq改

            if (result.Message != null && result.Message != string.Empty)
                //ed.WriteMessage("\n   {0}", result.Message);//2013.1.25改
                Console.WriteLine("   {0}", result.Message);//2013.5.25lq改

            if (result.StackTrace != null && result.StackTrace != string.Empty)
                //ed.WriteMessage("\n" + (result.IsFailure
                    //? StackTraceFilter.Filter(result.StackTrace)
                    //: result.StackTrace + Environment.NewLine));//2013.1.25改
                Console.WriteLine(result.IsFailure
                    ? StackTraceFilter.Filter(result.StackTrace)
                    : result.StackTrace + Environment.NewLine);//2013.5.25lq改
        }
	    #endregion
	}
}

