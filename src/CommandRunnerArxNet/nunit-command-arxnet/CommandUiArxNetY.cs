// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
//2012年1月6日，雷群修改
// ****************************************************************

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

    using Autodesk.AutoCAD.Runtime;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.ApplicationServices;

    /// <summary>
    /// Summary description for  CommandUiArxNet.
    /// </summary>
    public class CommandUiArxNet : ConsoleUi
    {
        private string workDir;

        public CommandUiArxNet()
		{
		}        

        public int Execute(CommandOptionsArxNet options)
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

            TextWriter outWriter = new StringWriter();
            bool redirectOutput = options.output != null && options.output != string.Empty;
            if (redirectOutput)
            {
                StreamWriter outStreamWriter = new StreamWriter(Path.Combine(workDir, options.output));
                outStreamWriter.AutoFlush = true;
                outWriter = outStreamWriter;
            }

            TextWriter errorWriter = new StringWriter();
            bool redirectError = options.err != null && options.err != string.Empty;
            if (redirectError)
            {
                StreamWriter errorStreamWriter = new StreamWriter(Path.Combine(workDir, options.err));
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

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
#if CLR_2_0 || CLR_4_0
            ed.WriteMessage("\nProcessModel: {0}    DomainUsage: {1}", processModel, domainUsage);

            ed.WriteMessage("\nExecution Runtime: {0}", framework);
#else
            ed.WriteMessage("\nDomainUsage: {0}", domainUsage);

            if (processModel != ProcessModel.Default && processModel != ProcessModel.Single)
                ed.WriteMessage("\nWarning: Ignoring project setting 'processModel={0}'", processModel);

            if (!RuntimeFramework.CurrentFramework.Supports(framework))
                ed.WriteMessage("\nWarning: Ignoring project setting 'runtimeFramework={0}'", framework);
#endif
            using (TestRunner testRunner = new DefaultTestRunnerFactory().MakeTestRunner(package))
            {
                testRunner.Load(package);

                if (testRunner.Test == null)
                {
                    testRunner.Unload();
                    ed.WriteMessage("\nUnable to locate fixture {0}", options.fixture);
                    return FIXTURE_NOT_FOUND;
                }

                EventCollectorArxNet collector = new EventCollectorArxNet(options, outWriter, errorWriter);

                TestFilter testFilter = TestFilter.Empty;
                SimpleNameFilter nameFilter = new SimpleNameFilter();

                if (options.run != null && options.run != string.Empty)
                {
                    ed.WriteMessage("\nSelected test(s): " + options.run);
                    foreach (string name in TestNameParser.Parse(options.run))
                        nameFilter.Add(name);
                    testFilter = nameFilter;
                }

                if (options.runlist != null && options.runlist != string.Empty)
                {
                    ed.WriteMessage("\nRun list: " + options.runlist);
                    using (StreamReader rdr = new StreamReader(options.runlist))
                    {
                        // NOTE: We can't use rdr.EndOfStream because it's
                        // not present in .NET 1.x.
                        string line = rdr.ReadLine();
                        while (line != null)
                        {
                            if (line[0] != '#')
                                nameFilter.Add(line);
                            line = rdr.ReadLine();
                        }
                    }
                    testFilter = nameFilter;
                }

                if (options.include != null && options.include != string.Empty)
                {
                    TestFilter includeFilter = new CategoryExpression(options.include).Filter;
                    ed.WriteMessage("\nIncluded categories: " + includeFilter.ToString());

                    if (testFilter.IsEmpty)
                        testFilter = includeFilter;
                    else
                        testFilter = new AndFilter(testFilter, includeFilter);
                }

                if (options.exclude != null && options.exclude != string.Empty)
                {
                    TestFilter excludeFilter = new NotFilter(new CategoryExpression(options.exclude).Filter);
                    ed.WriteMessage("\nExcluded categories: " + excludeFilter.ToString());

                    if (testFilter.IsEmpty)
                        testFilter = excludeFilter;
                    else if (testFilter is AndFilter)
                        ((AndFilter)testFilter).Add(excludeFilter);
                    else
                        testFilter = new AndFilter(testFilter, excludeFilter);
                }

                if (testFilter is NotFilter)
                    ((NotFilter)testFilter).TopLevel = true;

                TestResult result = null;
                string savedDirectory = Environment.CurrentDirectory;
                TextWriter savedOut = Console.Out;
                TextWriter savedError = Console.Error;

                try
                {
                    result = testRunner.Run(collector, testFilter);
                    if (!redirectOutput) ed.WriteMessage("\n" + outWriter.ToString());
                    if (!redirectError) ed.WriteMessage("\n" + errorWriter.ToString());
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
                    Console.SetOut(savedOut);
                    Console.SetError(savedError);
                }

                ed.WriteMessage("\n");

                int returnCode = UNEXPECTED_ERROR;

                if (result != null)
                {
                    string xmlOutput = CreateXmlOutput(result);
                    ResultSummarizer summary = new ResultSummarizer(result);

                    if (options.xmlConsole)
                    {
                        ed.WriteMessage("\n" + xmlOutput);
                    }
                    else
                    {
                        WriteSummaryReport(summary);
                        if (summary.ErrorsAndFailures > 0 || result.IsError || result.IsFailure)
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

                    returnCode = summary.ErrorsAndFailures;
                }

                if (collector.HasExceptions)
                {
                    collector.WriteExceptions();
                    returnCode = UNEXPECTED_ERROR;
                }

                return returnCode;
            }            
        }

        #region Helper Methods
        private static TestPackage MakeTestPackage(ConsoleOptions options)
        {
            TestPackage package;
            DomainUsage domainUsage = DomainUsage.Default;
            ProcessModel processModel = ProcessModel.Default;
            RuntimeFramework framework = RuntimeFramework.CurrentFramework;

            if (options.IsTestProject)
            {
                NUnitProject project =
                    Services.ProjectService.LoadProject((string)options.Parameters[0]);

                string configName = options.config;
                if (configName != null)
                    project.SetActiveConfig(configName);

                package = project.ActiveConfig.MakeTestPackage();
                processModel = project.ProcessModel;
                domainUsage = project.DomainUsage;
                framework = project.ActiveConfig.RuntimeFramework;
            }
            else if (options.Parameters.Count == 1)
            {
                package = new TestPackage((string)options.Parameters[0]);
                domainUsage = DomainUsage.Single;
            }
            else
            {
                package = new TestPackage(null, options.Parameters);
                domainUsage = DomainUsage.Multiple;
            }

            if (options.process != ProcessModel.Default)
                processModel = options.process;

            if (options.domain != DomainUsage.Default)
                domainUsage = options.domain;

            if (options.framework != null)
                framework = RuntimeFramework.Parse(options.framework);

            package.TestName = options.fixture;

            package.Settings["ProcessModel"] = processModel;
            package.Settings["DomainUsage"] = domainUsage;
            if (framework != null)
                package.Settings["RuntimeFramework"] = framework;



            if (domainUsage == DomainUsage.None)
            {
                // Make sure that addins are available
                CoreExtensions.Host.AddinRegistry = Services.AddinRegistry;
            }

            package.Settings["ShadowCopyFiles"] = !options.noshadow;
            package.Settings["UseThreadedRunner"] = !options.nothread;
            package.Settings["DefaultTimeout"] = options.timeout;

            return package;
        }

        private static string CreateXmlOutput(TestResult result)
        {
            StringBuilder builder = new StringBuilder();
            new XmlResultWriter(new StringWriter(builder)).SaveTestResult(result);

            return builder.ToString();
        }

        private static void WriteSummaryReport(ResultSummarizer summary)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage(
                "\nTests run: {0}, Errors: {1}, Failures: {2}, Inconclusive: {3}, Time: {4} seconds",
                summary.TestsRun, summary.Errors, summary.Failures, summary.Inconclusive, summary.Time);
            ed.WriteMessage(
                "\n  Not run: {0}, Invalid: {1}, Ignored: {2}, Skipped: {3}",
                summary.TestsNotRun, summary.NotRunnable, summary.Ignored, summary.Skipped);
            ed.WriteMessage("\n");
        }

        private void WriteErrorsAndFailuresReport(TestResult result)
        {
            reportIndex = 0;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\nErrors and Failures:");
            WriteErrorsAndFailures(result);
            ed.WriteMessage("\n");
        }

        private void WriteErrorsAndFailures(TestResult result)
        {
            if (result.Executed)
            {
                if (result.HasResults)
                {
                    if ((result.IsFailure || result.IsError) && result.FailureSite == FailureSite.SetUp)
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
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\nTests Not Run:");
            WriteNotRunResults(result);
            ed.WriteMessage("\n"); ;
        }

        private int reportIndex = 0;
        private void WriteNotRunResults(TestResult result)
        {
            if (result.HasResults)
                foreach (TestResult childResult in result.Results)
                    WriteNotRunResults(childResult);
            else if (!result.Executed)
                WriteSingleResult(result);
        }

        private void WriteSingleResult(TestResult result)
        {
            string status = result.IsFailure || result.IsError
                ? string.Format("{0} {1}", result.FailureSite, result.ResultState)
                : result.ResultState.ToString();

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n{0}) {1} : {2}", ++reportIndex, status, result.FullName);

            if (result.Message != null && result.Message != string.Empty)
                ed.WriteMessage("\n   {0}", result.Message);

            if (result.StackTrace != null && result.StackTrace != string.Empty)
                ed.WriteMessage("\n" + (result.IsFailure
                    ? StackTraceFilter.Filter(result.StackTrace)
                    : result.StackTrace + Environment.NewLine));
        }
        #endregion

    }
}
